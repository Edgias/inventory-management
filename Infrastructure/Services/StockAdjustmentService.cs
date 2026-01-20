using EdgIQ.Inventory.Domain.Entities.StockAdjustmentAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdgIQ.Inventory.Infrastructure.Services;

public class StockAdjustmentService(InventoryDbContext dbContext) : IStockAdjustmentService
{
    public async Task<bool> ApproveStockAdjustmentAsync(int adjustmentId, string approverId)
    {
        StockAdjustment? stockAdjustment = await dbContext.StockAdjustments
            .Include(sa => sa.StockAdjustmentItems)
            .FirstOrDefaultAsync(sa => sa.Id == adjustmentId);

        if (stockAdjustment == null || stockAdjustment.Status != ApprovalStatus.Pending)
        {
            return false;
        }

        // Update stock levels
        foreach (StockAdjustmentItem stockAdjustmentItem in stockAdjustment.StockAdjustmentItems)
        {
            if (stockAdjustmentItem.BatchId.HasValue)
            {
                var batchStock = await dbContext.BatchStocks
                    .FirstOrDefaultAsync(bs => bs.BatchId == stockAdjustmentItem.BatchId.Value &&
                                              bs.WarehouseId == stockAdjustment.WarehouseId &&
                                              bs.BinId == stockAdjustmentItem.BinId);

                batchStock?.UpdateQuantity(stockAdjustmentItem.ActualQuantity);
            }
            else
            {
                WarehouseStock? warehouseStock = await dbContext.WarehouseStocks
                    .FirstOrDefaultAsync(ws => ws.ProductVariantId == stockAdjustmentItem.AdjustedStock.ProductVariantId &&
                                              ws.WarehouseId == stockAdjustment.WarehouseId);

                if (warehouseStock != null)
                {
                    warehouseStock.QuantityOnHand += stockAdjustmentItem.AdjustmentQuantity;
                    warehouseStock.LastStockDate = DateTime.UtcNow;
                }
            }

            // Create stock transaction
            await _transactionService.CreateTransaction(new StockTransactionDto
            {
                TransactionType = "Adjustment",
                ProductVariantId = stockAdjustmentItem.ProductVariantId,
                BatchId = stockAdjustmentItem.BatchId,
                SerialNumberId = stockAdjustmentItem.SerialNumberId,
                WarehouseId = stockAdjustment.WarehouseId,
                BinId = stockAdjustmentItem.BinId,
                Quantity = stockAdjustmentItem.AdjustmentQuantity,
                UnitCost = stockAdjustmentItem.UnitCost,
                ReferenceType = "StockAdjustment",
                ReferenceId = adjustmentId
            });
        }

        stockAdjustment.Status = "Approved";
        stockAdjustment.ApprovedById = approverId;
        stockAdjustment.ApprovedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RejectStockAdjustmentAsync(int adjustmentId, string approverId, string reason)
    {
        StockAdjustment? stockAdjustment = await dbContext.StockAdjustments.FindAsync(adjustmentId);

        if (stockAdjustment == null || stockAdjustment.Status != ApprovalStatus.Pending)
        {
            return false;
        }

        stockAdjustment.Reject(approverId, reason);

        await dbContext.SaveChangesAsync();

        return true;
    }
}
