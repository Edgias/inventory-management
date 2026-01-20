namespace EdgIQ.Inventory.Domain.Entities;

public class BatchStock : BaseEntity
{
    public int BatchId { get; private set; }
    public Batch Batch { get; private set; } = default!;

    public int WarehouseId { get; private set; }
    public Warehouse Warehouse { get; private set; } = default!;

    public int? BinId { get; private set; }
    public Bin? Bin { get; private set; }

    public int Quantity { get; private set; }

    public int QuantityReserved { get; private set; }

    public int QuantityAvailable => Quantity - QuantityReserved;

    public DateTime LastStockDate { get; private set; }

    public BatchStock(int batchId, int warehouseId, int? binId, int quantity, int quantityReserved)
    {
        BatchId = batchId;
        WarehouseId = warehouseId;
        BinId = binId;
        Quantity = quantity;
        QuantityReserved = quantityReserved;
        LastStockDate = DateTime.Now.ToUtc();
    }

    public int GetAvailableQuantity()
    {
        return Quantity - QuantityReserved;
    }

    public void UpdateQuantity(int newQuantity)
    {
        Quantity = newQuantity;
        LastStockDate = DateTime.Now.ToUtc();
    }

}
