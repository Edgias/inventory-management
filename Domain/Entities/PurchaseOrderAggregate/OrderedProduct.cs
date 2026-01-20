namespace EdgIQ.Inventory.Domain.Entities.PurchaseOrderAggregate;

public class OrderedProduct
{
    public int ProductVariantId { get; private set; }

    public string ProductName { get; private set; } = default!;

    private OrderedProduct()
    {

    }

    public OrderedProduct(int productVariantId, string productName)
    {
        ProductVariantId = productVariantId;
        ProductName = productName;
    }
}
