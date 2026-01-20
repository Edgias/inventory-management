namespace EdgIQ.Inventory.Domain.Entities.GoodsReceivedNoteAggregate;

public class ReceivedProduct
{
    public int ProductVariantId { get; private set; }

    public string ProductName { get; private set; } = default!;

    private ReceivedProduct()
    {

    }

    public ReceivedProduct(int productVariantId, string productName)
    {
        ProductVariantId = productVariantId;
        ProductName = productName;
    }
}
