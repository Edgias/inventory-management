
namespace EdgIQ.Inventory.WebAPI.Suppliers;

internal static class SuppliersApi
{
    public static RouteGroupBuilder MapSuppliers(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("/suppliers");
        group.WithTags("Suppliers");

        group.RequireAuthorization(pb => pb.RequireCurrentUser());

        // Rate limit all of the APIs
        group.RequirePerUserRateLimit();

        group.MapGet("/", async (InventoryDbContext dbContext, CurrentUser currentUser) =>
        {
            return await dbContext.Suppliers
            .AsNoTracking()
            .Where(s => s.TenantId == currentUser.User!.TenantId)
            .Select(s => s.AsApiResponse())            
            .ToListAsync();

        });

        group.MapGet("/{skip}/{take}", async (InventoryDbContext dbContext, 
            int skip, int take, CurrentUser currentUser) =>
        {
            List<SupplierResponse> suppliers = await dbContext.Suppliers
            .AsNoTracking()
            .Where(s => s.TenantId == currentUser.User!.TenantId)
            .Select(s => s.AsApiResponse())
            .Skip(skip)
            .Take(take)
            .ToListAsync();

            int suppliersCount = await dbContext.Suppliers
            .AsNoTracking()
            .CountAsync(d => d.TenantId == currentUser.User!.TenantId);

            PaginatedResponse<SupplierResponse> response = new(suppliers, suppliersCount, skip, take);
            
            return response;
        });

        group.MapGet("/{id}", async Task<Results<Ok<SupplierResponse>, NotFound>>
            (InventoryDbContext dbContext, int id) =>
        {
            return await dbContext.Suppliers.FindAsync(id) switch
            {
                Supplier entity when entity is not null => TypedResults.Ok(entity.AsApiResponse()),
                _ => TypedResults.NotFound()
            };
        });

        group.MapPost("/", async Task<Created<SupplierResponse>> (InventoryDbContext dbContext, 
            SupplierRequest request, CurrentUser currentUser) =>
        {
            Supplier entity = request.ToEntity();
            //entity.OnCreate(currentUser.User!.TenantId, currentUser.User!.Id);

            dbContext.Add(entity);
            await dbContext.SaveChangesAsync();

            return TypedResults.Created($"/suppliers/{entity.Id}", entity.AsApiResponse());
        });

        group.MapPut("/{id}", async Task<Results<Ok, NotFound, BadRequest>> (InventoryDbContext dbContext, 
            int id, SupplierRequest request, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            Supplier? entity = await dbContext.Suppliers.FindAsync(id);

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
            int rowsAffected = await dbContext.Suppliers
            .Where(s => s.Id == id)
            .ExecuteDeleteAsync();

            return rowsAffected == 0 ? TypedResults.NotFound() : TypedResults.Ok();
        });

        return group;
    }

}
