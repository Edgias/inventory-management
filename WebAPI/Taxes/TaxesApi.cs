namespace EdgIQ.Inventory.WebAPI.Taxes;

internal static class TaxesApi
{
    public static RouteGroupBuilder MapTaxes(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("/taxes");
        group.WithTags("Taxes");

        group.RequireAuthorization(pb => pb.RequireCurrentUser());

        // Rate limit all of the APIs
        group.RequirePerUserRateLimit();

        group.MapGet("/", async (IHttpContextAccessor httpContext, InventoryDbContext dbContext, CurrentUser currentUser) =>
        {
            return await dbContext.Taxes
            .Where(t => t.TenantId == currentUser.User!.TenantId)
            .Select(t => t.AsApiResponse())
            .AsNoTracking()
            .ToListAsync();

        });

        group.MapGet("/{skip}/{take}", async (IHttpContextAccessor httpContext, 
            InventoryDbContext dbContext, int skip, int take, CurrentUser currentUser) =>
        {
            List<Tax> taxes = await dbContext.Taxes
            .Where(t => t.TenantId == currentUser.User!.TenantId)
            .AsNoTracking()
            .Skip(skip)
            .Take(take)
            .ToListAsync();

            int taxesCount = await dbContext.Taxes
            .AsNoTracking()
            .CountAsync(t => t.TenantId == currentUser.User!.TenantId);

            PaginatedResponse<TaxResponse> response = new([.. taxes.Select(t => t.AsApiResponse())], taxesCount, skip, take);
            
            return response;
        });

        group.MapGet("/{id}", async Task<Results<Ok<TaxResponse>, NotFound>>
            (InventoryDbContext dbContext, int id) =>
        {
            return await dbContext.Taxes.FindAsync(id) switch
            {
                Tax entity when entity is not null => TypedResults.Ok(entity.AsApiResponse()),
                _ => TypedResults.NotFound()
            };
        });

        group.MapPost("/", async Task<Created<TaxResponse>> (IHttpContextAccessor httpContext, 
            InventoryDbContext dbContext, TaxRequest request, CurrentUser currentUser) =>
        {
            Tax entity = request.ToEntity();
            //entity.OnCreate(currentUser.User!.TenantId, currentUser.User!.Id);

            dbContext.Add(entity);
            await dbContext.SaveChangesAsync();

            return TypedResults.Created($"/taxes/{entity.Id}", entity.AsApiResponse());
        });

        group.MapPut("/{id}", async Task<Results<Ok, NotFound, BadRequest>> (IHttpContextAccessor httpContext, 
            InventoryDbContext dbContext, int id, TaxRequest request, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            Tax? entity = await dbContext.Taxes.FindAsync(id);

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

        group.MapDelete("/{id}", async Task<Results<NotFound, Ok>> (InventoryDbContext dbContext, int id) =>
        {
            int rowsAffected = await dbContext.Taxes
            .Where(t => t.Id == id)
            .ExecuteDeleteAsync();

            return rowsAffected == 0 ? TypedResults.NotFound() : TypedResults.Ok();
        });

        return group;
    }

}
