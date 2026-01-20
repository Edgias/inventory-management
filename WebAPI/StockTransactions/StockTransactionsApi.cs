namespace EdgIQ.Inventory.WebAPI.StockTransactions;

internal static class StockTransactionsApi
{
    public static RouteGroupBuilder MapStockTransactions(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("/stock-transactions");
        group.WithTags("Stock Transactions");

        group.RequireAuthorization(pb => pb.RequireCurrentUser());

        // Rate limit all of the APIs
        group.RequirePerUserRateLimit();

        group.MapGet("/", async (InventoryDbContext dbContext, CurrentUser currentUser) =>
        {
            return await dbContext.StockTransactions
            .AsNoTracking()
            .Where(st => st.TenantId == currentUser.User!.TenantId)
            .Select(st => st.AsApiResponse())
            .ToListAsync();

        });

        group.MapGet("/{skip}/{take}", async (InventoryDbContext dbContext,
            int skip, int take, CurrentUser currentUser) =>
        {
            List<StockTransactionResponse> stockTransactions = await dbContext.StockTransactions
            .AsNoTracking()
            .Include(st => st.ProductVariant)
            .ThenInclude(st => st.Product)
            .Include(st => st.Batch)
            .Include(st => st.Warehouse)
            .Include(st => st.Bin)
            .Where(st => st.TenantId == currentUser.User!.TenantId)
            .Select(st => st.AsApiResponse())
            .Skip(skip)
            .Take(take)
            .ToListAsync();

            int stockTransactionsCount = await dbContext.StockTransactions
            .AsNoTracking()
            .CountAsync(st => st.TenantId == currentUser.User!.TenantId);

            PaginatedResponse<StockTransactionResponse> response = new(stockTransactions, stockTransactionsCount, skip, take);

            return response;
        });

        group.MapGet("/{skip}/{take}/{fromDate}/{toDate}", async (InventoryDbContext dbContext,
            int productVariantId, int? warehouseId, int skip, int take, 
            DateTime? fromDate, DateTime? toDate, CurrentUser currentUser) =>
        {
            IQueryable<StockTransaction> query = dbContext.StockTransactions
            .Include(st => st.ProductVariant)
            .Include(st => st.Batch)
            .Include(st => st.Warehouse)
            .Include(st => st.Bin)
            .Where(st => st.TenantId == currentUser.User!.TenantId && st.ProductVariantId == productVariantId);

            if (warehouseId.HasValue)
            {
                query = query.Where(st => st.WarehouseId == warehouseId);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(st => st.TransactionDate >= fromDate);
            }

            if (toDate.HasValue)
            {
                query = query.Where(st => st.TransactionDate <= toDate);
            }

            List<StockTransactionResponse> stockTransactions = await query
            .Select(st => st.AsApiResponse())
            .Skip(skip)
            .Take(take)
            .ToListAsync();

            int stockTransactionsCount = await dbContext.StockTransactions
            .AsNoTracking()
            .CountAsync(st => st.TenantId == currentUser.User!.TenantId);

            PaginatedResponse<StockTransactionResponse> response = new(stockTransactions, stockTransactionsCount, skip, take);

            return response;
        });

        return group;
    }

}

