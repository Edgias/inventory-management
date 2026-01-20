namespace EdgIQ.Inventory.Domain.Entities;

public class WarehouseStock : BaseEntity
{
    public int ProductVariantId { get; private set; }
    public ProductVariant ProductVariant { get; private set; } = default!;

    public int WarehouseId { get; private set; }
    public Warehouse Warehouse { get; private set; } = default!;

    public int? BatchId { get; private set; }
    public Batch? Batch { get; private set; }

    public int QuantityOnHand { get; private set; }

    // Stock sold but not yet shipped
    public int QuantityReserved { get; private set; }

    public DateTime? LastStockDate { get; private set; }

    public WarehouseStock(int productVariantId, int warehouseId, int? batchId, 
        int quantityOnHand, int quantityReserved)
    {
        ProductVariantId = productVariantId;
        WarehouseId = warehouseId;
        BatchId = batchId;
        QuantityOnHand = quantityOnHand;
        QuantityReserved = quantityReserved;
        LastStockDate = DateTime.Now.ToUtc();
    }

    public void UpdateQuantityOnHand(int newQuantityOnHand)
    {
        QuantityOnHand = newQuantityOnHand;
        LastStockDate = DateTime.Now.ToUtc();
    }

    public int GetQuantityAvailable()
    {
        return QuantityOnHand - QuantityReserved;
    }
}
