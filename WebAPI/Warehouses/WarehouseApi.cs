namespace EdgIQ.Inventory.WebAPI.Warehouses;

internal static class WarehousesApi
{
    public static RouteGroupBuilder MapWarehouses(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("/warehouses");
        group.WithTags("Warehouses");

        group.RequireAuthorization(pb => pb.RequireCurrentUser());

        // Rate limit all of the APIs
        group.RequirePerUserRateLimit();

        group.MapGet("/", async (InventoryDbContext dbContext, CurrentUser currentUser) =>
        {
            return await dbContext.Warehouses
            .AsNoTracking()
            .Where(w => w.TenantId == currentUser.User!.TenantId)
            .Select(w => w.AsApiResponse())
            .ToListAsync();

        });

        group.MapGet("/{skip}/{take}", async (InventoryDbContext dbContext,
            int skip, int take, CurrentUser currentUser) =>
        {
            List<WarehouseResponse> warehouses = await dbContext.Warehouses
            .AsNoTracking()
            .Where(w => w.TenantId == currentUser.User!.TenantId)
            .Select(w => w.AsApiResponse())
            .Skip(skip)
            .Take(take)
            .ToListAsync();

            int warehousesCount = await dbContext.Warehouses
            .AsNoTracking()
            .CountAsync(w => w.TenantId == currentUser.User!.TenantId);

            PaginatedResponse<WarehouseResponse> response = new(warehouses, warehousesCount, skip, take);

            return response;
        });

        group.MapGet("/{id}/stock", async (InventoryDbContext dbContext, int id, CurrentUser currentUser) =>
        {
            return await dbContext.WarehouseStocks
            .Include(ws => ws.ProductVariant)
            .ThenInclude(pv => pv.Product)
            .Where(ws => ws.WarehouseId == id)
            .Select(ws => ws.AsApiResponse())
            .ToListAsync();

        });

        group.MapGet("/{id}/bins", async (InventoryDbContext dbContext, int id, CurrentUser currentUser) =>
        {
            return await dbContext.Bins
            .Include(b => b.BinStocks)
            .ThenInclude(bs => bs.ProductVariant)
            .ThenInclude(pv => pv.Product)
            .Where(ws => ws.WarehouseId == id)
            .Select(ws => ws.AsApiResponse())
            .ToListAsync();

        });

        group.MapGet("/{id}/low-stock", async (InventoryDbContext dbContext, int id, 
            IStockService stockService, CurrentUser currentUser) =>
        {
            List<WarehouseStock> lowStock = await stockService.GetLowStockItemsAsync(id);

            return lowStock.Select(ws => ws.AsApiResponse());
        });

        group.MapGet("/{id}", async Task<Results<Ok<WarehouseResponse>, NotFound>>
            (InventoryDbContext dbContext, int id) =>
        {
            return await dbContext.Warehouses.FindAsync(id) switch
            {
                Warehouse entity when entity is not null => TypedResults.Ok(entity.AsApiResponse()),
                _ => TypedResults.NotFound()
            };
        });

        group.MapPost("/", async Task<Created<WarehouseResponse>> (InventoryDbContext dbContext,
            WarehouseRequest request, CurrentUser currentUser) =>
        {
            Warehouse entity = request.ToEntity();
            //entity.OnCreate(currentUser.User!.TenantId, currentUser.User!.Id);

            dbContext.Add(entity);
            await dbContext.SaveChangesAsync();

            return TypedResults.Created($"/warehouses/{entity.Id}", entity.AsApiResponse());
        });

        group.MapPut("/{id}", async Task<Results<Ok, NotFound, BadRequest>> (InventoryDbContext dbContext,
            int id, WarehouseRequest request, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            Warehouse? entity = await dbContext.Warehouses.FindAsync(id);

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
            int rowsAffected = await dbContext.Warehouses
            .Where(w => w.Id == id)
            .ExecuteDeleteAsync();

            return rowsAffected == 0 ? TypedResults.NotFound() : TypedResults.Ok();
        });

        return group;
    }

}

