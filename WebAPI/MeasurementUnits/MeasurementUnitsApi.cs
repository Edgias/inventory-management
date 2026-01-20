
namespace EdgIQ.Inventory.WebAPI.MeasurementUnits;

internal static class MeasurementUnitsApi
{
    public static RouteGroupBuilder MapMeasurementUnits(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("/measurement-units");
        group.WithTags("Measurement Units");

        group.RequireAuthorization(pb => pb.RequireCurrentUser());

        // Rate limit all of the APIs
        group.RequirePerUserRateLimit();

        group.MapGet("/", async (InventoryDbContext dbContext, CurrentUser currentUser) =>
        {
            return await dbContext.MeasurementUnits
            .AsNoTracking()
            .Where(mu => mu.TenantId == currentUser.User!.TenantId)
            .Select(mu => mu.AsApiResponse())            
            .ToListAsync();

        });

        group.MapGet("/{skip}/{take}", async (InventoryDbContext dbContext, 
            int skip, int take, CurrentUser currentUser) =>
        {
            List<MeasurementUnitResponse> measurementUnits = await dbContext.MeasurementUnits
            .AsNoTracking()
            .Where(mu => mu.TenantId == currentUser.User!.TenantId)
            .Select(mu => mu.AsApiResponse())
            .Skip(skip)
            .Take(take)
            .ToListAsync();

            int measurementUnitsCount = await dbContext.MeasurementUnits
            .AsNoTracking()
            .CountAsync(mu => mu.TenantId == currentUser.User!.TenantId);

            PaginatedResponse<MeasurementUnitResponse> response = new(measurementUnits, measurementUnitsCount, skip, take);
            
            return response;
        });

        group.MapGet("/{id}", async Task<Results<Ok<MeasurementUnitResponse>, NotFound>>
            (InventoryDbContext dbContext, int id) =>
        {
            return await dbContext.MeasurementUnits.FindAsync(id) switch
            {
                MeasurementUnit entity when entity is not null => TypedResults.Ok(entity.AsApiResponse()),
                _ => TypedResults.NotFound()
            };
        });

        group.MapPost("/", async Task<Created<MeasurementUnitResponse>> (InventoryDbContext dbContext, 
            MeasurementUnitRequest request, CurrentUser currentUser) =>
        {
            MeasurementUnit entity = request.ToEntity();
            //entity.OnCreate(currentUser.User!.TenantId, currentUser.User!.Id);

            dbContext.Add(entity);
            await dbContext.SaveChangesAsync();

            return TypedResults.Created($"/measurement-units/{entity.Id}", entity.AsApiResponse());
        });

        group.MapPut("/{id}", async Task<Results<Ok, NotFound, BadRequest>> (InventoryDbContext dbContext, 
            int id, MeasurementUnitRequest request, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            MeasurementUnit? entity = await dbContext.MeasurementUnits.FindAsync(id);

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
            int rowsAffected = await dbContext.MeasurementUnits
            .Where(mu => mu.Id == id)
            .ExecuteDeleteAsync();

            return rowsAffected == 0 ? TypedResults.NotFound() : TypedResults.Ok();
        });

        return group;
    }

}
