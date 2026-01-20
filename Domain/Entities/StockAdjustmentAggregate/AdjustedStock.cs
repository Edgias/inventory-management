namespace EdgIQ.Inventory.Domain.Entities.StockAdjustmentAggregate;

public class AdjustedStock
{
    public int ProductVariantId { get; private set; }

    public string ProductName { get; private set; } = default!;

    private AdjustedStock()
    {

    }

    public AdjustedStock(int productVariantId, string productName)
    {
        ProductVariantId = productVariantId;
        ProductName = productName;
    }
}
