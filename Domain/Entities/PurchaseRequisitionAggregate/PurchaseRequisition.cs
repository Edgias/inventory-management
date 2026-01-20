namespace EdgIQ.Inventory.Domain.Entities.PurchaseRequisitionAggregate;

public class PurchaseRequisition : BaseEntity
{
    public string RequisitionNumber { get; private set; }

    public DateTime RequestDate { get; private set; }

    public DateTime RequiredDate { get; private set; }

    public string? Purpose { get; private set; }

    public string RequesterId { get; private set; }

    public int? DepartmentId { get; private set; }
    public Department? Department { get; private set; }

    public int? WarehouseId { get; private set; }
    public Warehouse? Warehouse { get; private set; }

    public ApprovalStatus Status { get; private set; }

    private readonly List<PurchaseRequisitionApproval> _purchaseRequisitionApprovals = [];
    public IReadOnlyCollection<PurchaseRequisitionApproval> PurchaseRequisitionApprovals => _purchaseRequisitionApprovals.AsReadOnly();


    private readonly List<PurchaseRequisitionItem> _purchaseRequisitionItems  = [];
    public IReadOnlyCollection<PurchaseRequisitionItem> PurchaseRequisitionItems => _purchaseRequisitionItems.AsReadOnly();

    public PurchaseRequisition(string requisitionNumber, DateTime requestDate, DateTime requiredDate, string? purpose,
        string requesterId, int? departmentId, int? warehouseId)
    {
        RequisitionNumber = requisitionNumber;
        RequestDate = requestDate;
        RequiredDate = requiredDate;
        Purpose = purpose;
        RequesterId = requesterId;
        DepartmentId = departmentId;
        WarehouseId = warehouseId;
        Status = ApprovalStatus.Pending;
    }

    public PurchaseRequisition(string requisitionNumber, DateTime requestDate, DateTime requiredDate, string? purpose,
        string requesterId, int? departmentId, int? warehouseId, List<PurchaseRequisitionItem> purchaseRequisitionItems)
    {
        RequisitionNumber = requisitionNumber;
        RequestDate = requestDate;
        RequiredDate = requiredDate;
        Purpose = purpose;
        RequesterId = requesterId;
        DepartmentId = departmentId;
        WarehouseId = warehouseId;
        Status = ApprovalStatus.Pending;
        _purchaseRequisitionItems = purchaseRequisitionItems;
    }

    public void AddPurchaseRequisitionApproval(PurchaseRequisitionApproval purchaseRequisitionApproval)
    {
        _purchaseRequisitionApprovals.Add(purchaseRequisitionApproval);
    }

    public void RemovePurchaseRequisitionApproval(PurchaseRequisitionApproval purchaseRequisitionApproval)
    {
        _purchaseRequisitionApprovals.Remove(purchaseRequisitionApproval);
    }

    public void AddPurchaseRequisitionItem(PurchaseRequisitionItem purchaseRequisitionItem)
    {
        _purchaseRequisitionItems.Add(purchaseRequisitionItem);
    }

    public void RemovePurchaseRequisitionItem(PurchaseRequisitionItem purchaseRequisitionItem)
    {
        _purchaseRequisitionItems.Remove(purchaseRequisitionItem);
    }

    public void Update(DateTime requiredDate, string? purpose, int? departmentId, int? warehouseId)
    {
        RequiredDate = requiredDate;
        Purpose = purpose;
        DepartmentId = departmentId;
        WarehouseId = warehouseId;
    }

    public void UpdateStatus(ApprovalStatus status)
    {
        Status = status;
    }

    public decimal GetTotal()
    {
        decimal total = 0m;

        foreach (PurchaseRequisitionItem requisitionItem in _purchaseRequisitionItems)
        {
            decimal lineTotal = requisitionItem.UnitPrice * requisitionItem.Units;
            total += lineTotal;
        }

        return total;
    }
}
