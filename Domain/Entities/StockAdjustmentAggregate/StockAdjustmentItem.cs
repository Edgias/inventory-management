namespace EdgIQ.Inventory.Domain.Entities.StockAdjustmentAggregate;

public class StockAdjustmentItem : BaseEntity
{
    public int StockAdjustmentId { get; private set; }
    public StockAdjustment StockAdjustment { get; private set; } = default!;

    public AdjustedStock AdjustedStock { get; private set; }

    public int? BatchId { get; private set; }
    public Batch? Batch { get; private set; }

    public int? SerialNumberId { get; private set; }
    public SerialNumber? SerialNumber { get; private set; }

    public int? BinId { get; private set; }
    public Bin? Bin { get; private set; }

    public int SystemQuantity { get; private set; }

    public int ActualQuantity { get; private set; }

    public decimal UnitPrice { get; private set; }

    public string? Notes { get; private set; }

    public StockAdjustmentItem(AdjustedStock adjustedStock, int? batchId, int? serialNumberId,
        int? binId, int systemQuantity, int actualQuantity, decimal unitPrice, string? notes)
    {
        AdjustedStock = adjustedStock;
        BatchId = batchId;
        SerialNumberId = serialNumberId;
        BinId = binId;
        SystemQuantity = systemQuantity;
        ActualQuantity = actualQuantity;
        UnitPrice = unitPrice;
        Notes = notes;
    }

    public void Approve()
    {
        if(Batch != null)
        {
            WarehouseStock? batchStock = Batch.BatchStocks
                .FirstOrDefault(bs => bs.BatchId == Batch.Id && 
                bs.WarehouseId == StockAdjustment.WarehouseId);

            batchStock?.UpdateQuantityOnHand(ActualQuantity);
        }
        else
        {
            WarehouseStock? warehouseStock = StockAdjustment.Warehouse.WarehouseStocks
                .FirstOrDefault(ws => ws.ProductVariantId == AdjustedStock.ProductVariantId &&
                                          ws.WarehouseId == StockAdjustment.WarehouseId);

            warehouseStock?.UpdateQuantityOnHand(GetAdjustedQuantity());
        }
    }

    public void Update(AdjustedStock adjustedStock, int? batchId, int? serialNumberId,
        int? binId, int systemQuantity, int actualQuantity, decimal unitPrice, string? notes)
    {
        AdjustedStock = adjustedStock;
        BatchId = batchId;
        SerialNumberId = serialNumberId;
        BinId = binId;
        SystemQuantity = systemQuantity;
        ActualQuantity = actualQuantity;
        UnitPrice = unitPrice;
        Notes = notes;
    }

    public int GetAdjustedQuantity()
    {
        return ActualQuantity - SystemQuantity;
    }

    public decimal GetTotalCostImpact()
    {
        return GetAdjustedQuantity() * UnitPrice;
    }
}
