namespace EdgIQ.Inventory.Domain.Entities.PurchaseRequisitionAggregate;

public class RequisitionedProduct
{
    public int ProductVariantId { get; private set; }

    public string ProductName { get; private set; } = default!;

    private RequisitionedProduct()
    {

    }

    public RequisitionedProduct(int productVariantId, string productName)
    {
        ProductVariantId = productVariantId;
        ProductName = productName;
    }
}
