namespace EdgIQ.Inventory.Domain.Entities;

public class BinStock : BaseEntity
{
    public int BinId { get; private set; }
    public Bin Bin { get; private set; } = default!;

    public int ProductVariantId { get; private set; }
    public ProductVariant ProductVariant { get; private set; } = default!;

    public int Quantity { get; private set; }

    public DateTime? LastStockDate { get; private set; }

    public BinStock(int binId, int productVariantId, int quantity)
    {
        BinId = binId;
        ProductVariantId = productVariantId;
        Quantity = quantity;
        LastStockDate = DateTime.Now.ToUtc();
    }

    public void UpdateQuantity(int newQuantity)
    {
        Quantity = newQuantity;
        LastStockDate = DateTime.Now.ToUtc();
    }
}
