namespace EdgIQ.Inventory.Domain.Entities.PurchaseRequisitionAggregate;

public class PurchaseRequisitionItem : BaseEntity
{
    public decimal UnitPrice { get; private set; }

    public int Units { get; private set; }

    public RequisitionedProduct RequisitionedProduct { get; private set; } = default!;

    public int PurchaseRequisitionId { get; private set; }
    public PurchaseRequisition PurchaseRequisition { get; private set; } = default!;

    public ApprovalStatus Status { get; set; }

    public string? Notes { get; set; }

    private readonly List<PurchaseRequisitionItemApproval> _purchaseRequisitionItemApprovals = [];
    public IReadOnlyCollection<PurchaseRequisitionItemApproval> PurchaseRequisitionItemApprovals => _purchaseRequisitionItemApprovals.AsReadOnly();

    private PurchaseRequisitionItem()
    {
    }

    public PurchaseRequisitionItem(RequisitionedProduct requisitionedProduct, decimal unitPrice, int units)
    {
        RequisitionedProduct = requisitionedProduct;
        UnitPrice = unitPrice;
        Units = units;
        Status = ApprovalStatus.Pending;
    }

    public void AddPurchaseRequisitionItemApproval(PurchaseRequisitionItemApproval purchaseRequisitionItemApproval)
    {
        _purchaseRequisitionItemApprovals.Add(purchaseRequisitionItemApproval);
    }

    public void RemovePurchaseRequisitionItemApproval(PurchaseRequisitionItemApproval purchaseRequisitionItemApproval)
    {
        _purchaseRequisitionItemApprovals.Remove(purchaseRequisitionItemApproval);
    }

    public decimal GetTotal()
    {
        return Units * UnitPrice;
    }

    public void Update(RequisitionedProduct requisitionedProduct, decimal unitPrice, int units)
    {
        RequisitionedProduct = requisitionedProduct;
        UnitPrice = unitPrice;
        Units = units;
    }

    public void UpdateQuantity(int newQuantity)
    {
        Units = newQuantity;
    }

    public void UpdateStatus(ApprovalStatus status)
    {
        Status = status;
    }

    public void UpdateUnitPrice(decimal newPrice)
    {
        UnitPrice = newPrice;
    }
}
