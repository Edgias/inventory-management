using EdgIQ.Inventory.Domain.Entities.StockTransferAggregate;

namespace EdgIQ.Inventory.Infrastructure.Services;

public class StockTransferService(InventoryDbContext dbContext,
    IStockService stockService) : IStockTransferService
{
    public async Task<bool> ApproveStockTransferAsync(int transferId, string approverId)
    {
        StockTransfer? stockTransfer = await dbContext.StockTransfers.FindAsync(transferId);

        if (stockTransfer == null || stockTransfer.Status != StockTransferStatus.Pending)
        {
            return false;
        }

        stockTransfer.Approve(approverId);

        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ShipStockTransferAsync(int transferId)
    {
        StockTransfer? stockTransfer = await dbContext.StockTransfers
            .Include(st => st.StockTransferItems)
            .ThenInclude(sti => sti.TransferredProduct)
            .FirstOrDefaultAsync(st => st.Id == transferId);

        if (stockTransfer == null || stockTransfer.Status != StockTransferStatus.Received)
        {
            return false;
        }

        // Deduct stock from source warehouse
        foreach (var stockTransferItem in stockTransfer.StockTransferItems)
        {
            WarehouseStock sourceStock = await stockService.GetWarehouseStockAsync(stockTransfer.FromWarehouseId, 
                stockTransferItem.TransferredProduct.ProductVariantId);

            if (sourceStock == null || sourceStock.GetQuantityAvailable() < stockTransferItem.QuantityRequested)
            {
                return false;
            }

            // Update warehouse stock
            await stockService.UpdateWarehouseStockAsync(stockTransfer.FromWarehouseId,
                stockTransferItem.TransferredProduct.ProductVariantId,
                -stockTransferItem.QuantityRequested);

            // Update bin stock if specified
            if (stockTransferItem.FromBinId.HasValue)
            {
                await stockService.UpdateBinStockAsync(stockTransferItem.FromBinId.Value,
                    stockTransferItem.TransferredProduct.ProductVariantId, -stockTransferItem.QuantityRequested);
            }

            stockTransferItem.UpdateShippedQuantity(stockTransferItem.QuantityRequested);
        }

        stockTransfer.Ship();

        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ReceiveStockTransferAsync(int transferId, List<StockTransferItemRequest> lines)
    {
        StockTransfer? transfer = await dbContext.StockTransfers
            .Include(st => st.StockTransferItems)
            .FirstOrDefaultAsync(st => st.Id == transferId);

        if (transfer == null || transfer.Status != StockTransferStatus.InTransit)
        {
            return false;
        }

        // Add stock to destination warehouse
        foreach (StockTransferItemRequest lineDto in lines)
        {
            StockTransferItem? stockTransferItem = transfer.StockTransferItems.FirstOrDefault(sti => sti.Id == lineDto.Id);
            if (stockTransferItem == null)
            {
                continue;
            }

            // Update warehouse stock
            await stockService.UpdateWarehouseStockAsync(transfer.ToWarehouseId,
                stockTransferItem.TransferredProduct.ProductVariantId,
                lineDto.QuantityReceived);

            // Update bin stock if specified
            if (stockTransferItem.ToBinId.HasValue)
            {
                await stockService.UpdateBinStockAsync(stockTransferItem.ToBinId.Value,
                    stockTransferItem.TransferredProduct.ProductVariantId,
                    lineDto.QuantityReceived);
            }

            stockTransferItem.UpdateReceivedQuantity(lineDto.QuantityReceived);
        }

        transfer.Receive();

        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CancelStockTransferAsync(int transferId)
    {
        StockTransfer? stockTransfer = await dbContext.StockTransfers
            .Include(st => st.StockTransferItems)
            .ThenInclude(sti => sti.TransferredProduct)
            .FirstOrDefaultAsync(st => st.Id == transferId);

        if (stockTransfer == null || stockTransfer.Status == StockTransferStatus.Received)
        {
            return false;
        }

        // If already shipped, need to return stock to source warehouse
        if (stockTransfer.Status == StockTransferStatus.InTransit)
        {
            foreach (StockTransferItem stockTransferItem in stockTransfer.StockTransferItems)
            {
                await stockService.UpdateWarehouseStockAsync(
                    stockTransfer.FromWarehouseId,
                    stockTransferItem.TransferredProduct.ProductVariantId,
                    stockTransferItem.QuantityShipped);

                if (stockTransferItem.FromBinId.HasValue)
                {
                    await stockService.UpdateBinStockAsync(
                        stockTransferItem.FromBinId.Value,
                        stockTransferItem.TransferredProduct.ProductVariantId,
                        stockTransferItem.QuantityShipped);
                }
            }
        }

        stockTransfer.UpdateStatus(StockTransferStatus.Cancelled);

        await dbContext.SaveChangesAsync();
        return true;
    }
}
