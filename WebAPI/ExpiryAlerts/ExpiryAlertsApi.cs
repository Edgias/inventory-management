namespace EdgIQ.Inventory.WebAPI.ExpiryAlerts;

internal static class ExpiryAlertsApi
{
    public static RouteGroupBuilder MapExpiryAlerts(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("/expiry-alerts");
        group.WithTags("Expiry Alerts");

        group.RequireAuthorization(pb => pb.RequireCurrentUser());

        // Rate limit all of the APIs
        group.RequirePerUserRateLimit();

        group.MapGet("/", async (IHttpContextAccessor httpContext, InventoryDbContext dbContext, CurrentUser currentUser) =>
        {
            return await dbContext.ExpiryAlerts
            .AsNoTracking()
            .Include(ea => ea.Batch)
            .ThenInclude(b => b.ProductVariant)
            .ThenInclude(pv => pv.Product)
            .Where(ea => ea.TenantId == currentUser.User!.TenantId && !ea.IsAcknowledged)
            .Select(ea => ea.AsApiResponse())
            .ToListAsync();

        });

        group.MapGet("/{skip}/{take}", async (IHttpContextAccessor httpContext,
            InventoryDbContext dbContext, int skip, int take, CurrentUser currentUser) =>
        {
            List<ExpiryAlertResponse> expiryAlerts = await dbContext.ExpiryAlerts
            .AsNoTracking()
            .Include(ea => ea.Batch)
            .ThenInclude(b => b.ProductVariant)
            .ThenInclude(pv => pv.Product)
            .Where(ea => ea.TenantId == currentUser.User!.TenantId && !ea.IsAcknowledged)            
            .Skip(skip)
            .Take(take)
            .Select(ea => ea.AsApiResponse())
            .ToListAsync();

            int expiryAlertsCount = await dbContext.ExpiryAlerts
            .AsNoTracking()
            .CountAsync(ea => ea.TenantId == currentUser.User!.TenantId && !ea.IsAcknowledged);

            PaginatedResponse<ExpiryAlertResponse> response = new(expiryAlerts, expiryAlertsCount, skip, take);

            return response;
        });

        group.MapGet("/{id}", async Task<Results<Ok<ExpiryAlertResponse>, NotFound>>
            (InventoryDbContext dbContext, int id) =>
        {
            return await dbContext.ExpiryAlerts.FindAsync(id) switch
            {
                ExpiryAlert entity when entity is not null => TypedResults.Ok(entity.AsApiResponse()),
                _ => TypedResults.NotFound()
            };
        });

        group.MapPost("/", async Task<Created<ExpiryAlertResponse>> (IHttpContextAccessor httpContext,
            InventoryDbContext dbContext, ExpiryAlertRequest request, CurrentUser currentUser) =>
        {
            ExpiryAlert entity = request.ToEntity();
            //entity.OnCreate(currentUser.User!.TenantId, currentUser.User!.Id);

            dbContext.Add(entity);
            await dbContext.SaveChangesAsync();

            return TypedResults.Created($"/expiry-alerts/{entity.Id}", entity.AsApiResponse());
        });

        group.MapPut("/{id}", async Task<Results<Ok, NotFound, BadRequest>> (IHttpContextAccessor httpContext,
            InventoryDbContext dbContext, int id, ExpiryAlertRequest request, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            ExpiryAlert? entity = await dbContext.ExpiryAlerts.FindAsync(id);

            if (entity is null)
            {
                return TypedResults.NotFound();
            }

            entity.Update(request);
            //entity.OnUpdate(currentUser.User!.Id);

            dbContext.Update(entity);
            await dbContext.SaveChangesAsync();

            return TypedResults.Ok();
        });

        group.MapPatch("/{id}/acknowledge/{action}", async Task<Results<NotFound, Ok>> (InventoryDbContext dbContext, 
            int id, string action, CurrentUser currentUser) =>
        {
            var alert = await dbContext.ExpiryAlerts.FindAsync(id);
            if (alert == null)
            {
                return TypedResults.NotFound();
            }

            alert.Acknowledge(currentUser.Id, action);
            dbContext.Update(alert);

            await dbContext.SaveChangesAsync();
            return TypedResults.Ok();
        });

        return group;
    }

}
