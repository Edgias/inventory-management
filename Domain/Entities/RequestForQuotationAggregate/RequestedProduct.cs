namespace EdgIQ.Inventory.Domain.Entities.RequestForQuotationAggregate;

public class RequestedProduct
{
    public int ProductVariantId { get; private set; }

    public string ProductName { get; private set; } = default!;

    private RequestedProduct()
    {

    }

    public RequestedProduct(int productVariantId, string productName)
    {
        ProductVariantId = productVariantId;
        ProductName = productName;
    }
}
