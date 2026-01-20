namespace EdgIQ.Inventory.Infrastructure.Services;

public class StockService(InventoryDbContext dbContext) : IStockService
{
    public async Task<WarehouseStock> GetWarehouseStockAsync(int warehouseId, int productVariantId)
    {
        WarehouseStock? stocks = await dbContext.WarehouseStocks
            .Include(ws => ws.Warehouse)
            .Include(ws => ws.ProductVariant)
            .ThenInclude(pv => pv.Product)
            .FirstOrDefaultAsync(ws => ws.WarehouseId == warehouseId && ws.ProductVariantId == productVariantId);

        return stocks!;
    }

    public async Task<bool> UpdateWarehouseStockAsync(int warehouseId, int productVariantId, int quantity)
    {
        WarehouseStock? stock = await dbContext.WarehouseStocks
            .FirstOrDefaultAsync(ws => ws.WarehouseId == warehouseId && ws.ProductVariantId == productVariantId);

        if (stock == null)
        {
            stock = new WarehouseStock(productVariantId, warehouseId, null, quantity, 0);
            dbContext.WarehouseStocks.Add(stock);
        }
        else
        {
            int quantityOnHand = stock.QuantityOnHand + quantity;
            stock.UpdateQuantityOnHand(quantityOnHand);
        }

        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateBinStockAsync(int binId, int productVariantId, int quantity)
    {
        BinStock? binStock = await dbContext.BinStocks
            .FirstOrDefaultAsync(bs => bs.BinId == binId && bs.ProductVariantId == productVariantId);

        if (binStock == null)
        {
            binStock = new(binId, productVariantId, quantity); ;
            dbContext.BinStocks.Add(binStock);
        }
        else
        {
            int toBinQuantity = binStock.Quantity + quantity;
            binStock.UpdateQuantity(toBinQuantity);
        }

        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> TransferStockBetweenBinsAsync(int fromBinId, int toBinId, int productVariantId, int quantity)
    {
        BinStock? fromBinStock = await dbContext.BinStocks
            .FirstOrDefaultAsync(bs => bs.BinId == fromBinId && bs.ProductVariantId == productVariantId);

        if (fromBinStock == null || fromBinStock.Quantity < quantity)
        {
            return false;
        }

        // Deduct from source bin
        int fromBinQuantity = fromBinStock.Quantity - quantity;
        fromBinStock.UpdateQuantity(quantity);

        // Add to destination bin
        BinStock? toBinStock = await dbContext.BinStocks
            .FirstOrDefaultAsync(bs => bs.BinId == toBinId && bs.ProductVariantId == productVariantId);

        if (toBinStock == null)
        {
            toBinStock = new(toBinId, productVariantId, quantity);
            dbContext.BinStocks.Add(toBinStock);
        }
        else
        {
            int toBinQuantity = toBinStock.Quantity + quantity;
            toBinStock.UpdateQuantity(toBinQuantity);
        }

        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<WarehouseStock>> GetLowStockItemsAsync(int warehouseId)
    {
        return await dbContext.WarehouseStocks
            .Include(ws => ws.ProductVariant)
            .ThenInclude(pv => pv.Product)
            .Include(ws => ws.Warehouse)
            .Where(ws => ws.WarehouseId == warehouseId &&
                        ws.QuantityOnHand <= ws.ProductVariant.ReorderPoint)
            .ToListAsync();
    }

    public async Task<List<BinStock>> GetBinStocksByWarehouseAsync(int warehouseId)
    {
        return await dbContext.BinStocks
            .Include(bs => bs.Bin)
            .Include(bs => bs.ProductVariant)
            .ThenInclude(pv => pv.Product)
            .Where(bs => bs.Bin.WarehouseId == warehouseId)
            .ToListAsync();
    }
}