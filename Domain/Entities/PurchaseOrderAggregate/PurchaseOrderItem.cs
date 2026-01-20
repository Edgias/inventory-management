namespace EdgIQ.Inventory.Domain.Entities.PurchaseOrderAggregate;

public class PurchaseOrderItem : BaseEntity
{
    public decimal UnitPrice { get; private set; }

    public int Units { get; private set; }

    public OrderedProduct OrderedProduct { get; private set; } = default!;

    public ApprovalStatus Status { get; private set; }

    public int PurchaseOrderId { get; private set; }
    public PurchaseOrder PurchaseOrder { get; private set; } = default!;

    private readonly List<PurchaseOrderItemApproval> _purchaseOrderItemApprovals = [];
    public IReadOnlyCollection<PurchaseOrderItemApproval> PurchaseOrderItemApprovals => _purchaseOrderItemApprovals.AsReadOnly();

    private PurchaseOrderItem()
    {
    }

    public PurchaseOrderItem(OrderedProduct orderedProduct, decimal unitPrice, int units)
    {
        OrderedProduct = orderedProduct;
        UnitPrice = unitPrice;
        Units = units;
        Status = ApprovalStatus.Pending;
    }

    public void AddPurchaseOrderItemApproval(PurchaseOrderItemApproval purchaseOrderItemApproval)
    {
        _purchaseOrderItemApprovals.Add(purchaseOrderItemApproval);
    }

    public void RemovePurchaseOrderItemApproval(PurchaseOrderItemApproval purchaseOrderItemApproval)
    {
        _purchaseOrderItemApprovals.Remove(purchaseOrderItemApproval);
    }

    public decimal GetTotal()
    {
        return Units * UnitPrice;
    }

    public void Update(OrderedProduct orderedProduct, decimal unitPrice, int units)
    {
        OrderedProduct = orderedProduct;
        UnitPrice = unitPrice;
        Units = units;
    }

    public void UpdateQuantity(int units)
    {
        Units = units;
    }

    public void UpdateStatus(ApprovalStatus status)
    {
        Status = status;
    }

    public void UpdateUnitPrice(decimal unitPrice)
    {
        UnitPrice = unitPrice;
    }
}
