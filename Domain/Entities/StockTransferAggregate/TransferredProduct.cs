namespace EdgIQ.Inventory.Domain.Entities.StockTransferAggregate;

public class TransferredProduct
{
    public int ProductVariantId { get; private set; }

    public string ProductName { get; private set; } = default!;

    private TransferredProduct()
    {

    }

    public TransferredProduct(int productVariantId, string productName)
    {
        ProductVariantId = productVariantId;
        ProductName = productName;
    }
}
