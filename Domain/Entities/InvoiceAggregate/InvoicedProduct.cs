namespace EdgIQ.Inventory.Domain.Entities.InvoiceAggregate;

public class InvoicedProduct
{
    public int ProductVariantId { get; private set; }

    public string ProductName { get; private set; } = default!;

    private InvoicedProduct()
    {

    }

    public InvoicedProduct(int productVariantId, string productName)
    {
        ProductVariantId = productVariantId;
        ProductName = productName;
    }
}
