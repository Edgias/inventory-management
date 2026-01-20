using Microsoft.EntityFrameworkCore;

namespace EdgIQ.Inventory.WebAPI.StockTransfers;

internal static class StockTransfersApi
{
    public static RouteGroupBuilder MapStockTransfers(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("/stock-transfers");
        group.WithTags("Stock Transfers");

        group.RequireAuthorization(pb => pb.RequireCurrentUser());

        // Rate limit all of the APIs
        group.RequirePerUserRateLimit();

        group.MapGet("/", async (InventoryDbContext dbContext, CurrentUser currentUser) =>
        {
            return await dbContext.StockTransfers
            .AsNoTracking()
            .Include(st => st.FromWarehouse)
            .Include(st => st.ToWarehouse)
            .Include(st => st.StockTransferItems)
            .ThenInclude(sti => sti.TransferredProduct)
            .Include(st => st.StockTransferItems)
            .ThenInclude(sti => sti.FromBin)
            .Include(st => st.StockTransferItems)
            .ThenInclude(sti => sti.ToBin)
            .Where(st => st.TenantId == currentUser.User!.TenantId)
            .Select(st => st.AsApiResponse())
            .ToListAsync();

        });

        group.MapGet("/{skip}/{take}", async (InventoryDbContext dbContext,
            int skip, int take, CurrentUser currentUser) =>
        {
            List<StockTransferResponse> stockTransfers = await dbContext.StockTransfers
            .AsNoTracking()
            .Include(st => st.FromWarehouse)
            .Include(st => st.ToWarehouse)
            .Include(st => st.StockTransferItems)
            .ThenInclude(sti => sti.TransferredProduct)
            .Include(st => st.StockTransferItems)
            .ThenInclude(sti => sti.FromBin)
            .Include(st => st.StockTransferItems)
            .ThenInclude(sti => sti.ToBin)
            .Where(st => st.TenantId == currentUser.User!.TenantId)
            .Select(st => st.AsApiResponse())
            .Skip(skip)
            .Take(take)
            .ToListAsync();

            int stockTransfersCount = await dbContext.StockTransfers
            .AsNoTracking()
            .CountAsync(st => st.TenantId == currentUser.User!.TenantId);

            PaginatedResponse<StockTransferResponse> response = new(stockTransfers, stockTransfersCount, skip, take);

            return response;
        });

        group.MapGet("/warehouses/{warehouseId}/incoming", async (InventoryDbContext dbContext,
            int warehouseId, CurrentUser currentUser) =>
        {
            return await dbContext.StockTransfers
            .Include(st => st.FromWarehouse)
            .Include(st => st.StockTransferItems)
            .ThenInclude(sti => sti.TransferredProduct)
            .Where(st => st.ToWarehouseId == warehouseId)
            .Select(st => st.AsApiResponse())
            .ToListAsync();

        });

        group.MapGet("/warehouses/{warehouseId}/outgoing", async (InventoryDbContext dbContext,
            int warehouseId, CurrentUser currentUser) =>
        {
            return await dbContext.StockTransfers
            .Include(st => st.ToWarehouse)
            .Include(st => st.StockTransferItems)
            .ThenInclude(sti => sti.TransferredProduct)
            .Where(st => st.FromWarehouseId == warehouseId)
            .Select(st => st.AsApiResponse())
            .ToListAsync();

        });

        group.MapGet("/{id}", async Task<Results<Ok<StockTransferResponse>, NotFound>>
            (InventoryDbContext dbContext, int id) =>
        {
            StockTransfer? stockTransfer = await dbContext.StockTransfers
            .Include(st => st.FromWarehouse)
            .Include(st => st.ToWarehouse)
            .Include(st => st.StockTransferItems)
            .ThenInclude(sti => sti.TransferredProduct)
            .Include(st => st.StockTransferItems)
            .ThenInclude(sti => sti.FromBin)
            .Include(st => st.StockTransferItems)
            .ThenInclude(sti => sti.ToBin)
            .FirstOrDefaultAsync(st => st.Id == id);

            if (stockTransfer == null)
            {
                TypedResults.NotFound();
            }

            return TypedResults.Ok(stockTransfer?.AsApiResponse());
        });

        group.MapPost("/", async Task<Created<StockTransferResponse>> (InventoryDbContext dbContext,
            StockTransferRequest request, INumberGeneratorService numberGeneratorService, CurrentUser currentUser) =>
        {
            IEnumerable<int> productIds = request.StockTransferItems.Select(p => p.ProductVariantId);

            List<ProductVariant> products = await dbContext.ProductVariants
            .Include(pv => pv.Product)
            .AsNoTracking()
            .Include(pv => pv.Product)
            .Where(pv => pv.TenantId == currentUser.User!.TenantId && productIds.Contains(pv.Id))
            .ToListAsync();

            string TransferNumber = await numberGeneratorService.GenerateNextNumberAsync();

            StockTransfer entity = request.ToEntity(TransferNumber, currentUser.Id, products);
            //entity.OnCreate(currentUser.User!.TenantId, currentUser.User!.Id);

            dbContext.Add(entity);
            await dbContext.SaveChangesAsync();

            return TypedResults.Created($"/stock-transfers/{entity.Id}", entity.AsApiResponse());
        });

        group.MapPut("/{id}", async Task<Results<Ok, NotFound, BadRequest>> (InventoryDbContext dbContext,
            int id, StockTransferRequest request, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            StockTransfer? entity = await dbContext.StockTransfers.FindAsync(id);

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

        group.MapPatch("/{id}/approve", async Task<Results<Ok, NotFound, BadRequest<string>>> (IStockTransferService stockTransferService,
            int id, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest("Invalid id");
            }

            bool result = await stockTransferService.ApproveStockTransferAsync(id, currentUser.Id);

            if (!result) 
            {
                return TypedResults.BadRequest("Unable to approve stock transfer");
            }

            return TypedResults.Ok();
        });

        group.MapPatch("/{id}/ship", async Task<Results<Ok, NotFound, BadRequest<string>>> (IStockTransferService stockTransferService,
            int id, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest("Invalid id");
            }

            bool result = await stockTransferService.ShipStockTransferAsync(id);

            if (!result)
            {
                return TypedResults.BadRequest("Unable to ship stock transfer. Check stock availability.");
            }

            return TypedResults.Ok();
        });

        group.MapPost("/{id}/receive", async Task<Results<Ok, NotFound, BadRequest<string>>> (IStockTransferService stockTransferService,
            int id, List<StockTransferItemRequest> stockTransferItems, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest("Invalid id");
            }

            bool result = await stockTransferService.ReceiveStockTransferAsync(id, stockTransferItems);

            if (!result)
            {
                return TypedResults.BadRequest("Unable to receive stock transfer");
            }

            return TypedResults.Ok();
        });

        group.MapPatch("/{id}/cancel", async Task<Results<Ok, NotFound, BadRequest<string>>> (IStockTransferService stockTransferService,
            int id, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest("Invalid id");
            }

            bool result = await stockTransferService.CancelStockTransferAsync(id);

            if (!result)
            {
                return TypedResults.BadRequest("Unable to cancel stock transfer.");
            }

            return TypedResults.Ok();
        });

        group.MapDelete("/{id}", async Task<Results<NotFound, Ok>> (InventoryDbContext dbContext, int id) =>
        {
            int rowsAffected = await dbContext.StockTransfers
            .Where(st => st.Id == id)
            .ExecuteDeleteAsync();

            return rowsAffected == 0 ? TypedResults.NotFound() : TypedResults.Ok();
        });

        return group;
    }

}
