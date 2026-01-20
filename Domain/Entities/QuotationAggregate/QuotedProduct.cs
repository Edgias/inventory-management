namespace EdgIQ.Inventory.Domain.Entities.QuotationAggregate;

public class QuotedProduct
{
    public int ProductVariantId { get; private set; }

    public string ProductName { get; private set; } = default!;

    private QuotedProduct()
    {

    }

    public QuotedProduct(int productVariantId, string productName)
    {
        ProductVariantId = productVariantId;
        ProductName = productName;
    }
}
