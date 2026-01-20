namespace EdgIQ.Inventory.Domain.Entities.StockAdjustmentAggregate;

public class StockAdjustment : BaseEntity
{
    public string AdjustmentNumber { get; private set; } 

    public int WarehouseId { get; private set; }
    public Warehouse Warehouse { get; private set; } = default!;

    public DateTime AdjustmentDate { get; private set; }

    public StockAdjustmentReason Reason { get; private set; }

    public string? ReasonDescription { get; private set; }

    public string AdjusterId { get; private set; }

    public string? ApproverId { get; private set; }

    public DateTime? ApprovedDate { get; private set; }

    public ApprovalStatus Status { get; private set; } 

    public string? ReferenceDocument { get; private set; }

    public string? Notes { get; private set; }

    private readonly List<StockAdjustmentItem> _stockAdjustmentItems = [];
    public IReadOnlyCollection<StockAdjustmentItem> StockAdjustmentItems => _stockAdjustmentItems.AsReadOnly();

    public StockAdjustment(string adjustmentNumber, int warehouseId, StockAdjustmentReason reason, 
        string? reasonDescription, string adjusterId, string? referenceDocument, string? notes)
    {
        AdjustmentNumber = adjustmentNumber;
        WarehouseId = warehouseId;
        AdjustmentDate = DateTime.Now.ToUtc();
        Reason = reason;
        ReasonDescription = reasonDescription;
        AdjusterId = adjusterId;
        Status = ApprovalStatus.Pending;
        ReferenceDocument = referenceDocument;
        Notes = notes;
    }

    public StockAdjustment(string adjustmentNumber, int warehouseId, StockAdjustmentReason reason, 
        string? reasonDescription, string adjusterId, string? referenceDocument, string? notes,
        List<StockAdjustmentItem> stockAdjustmentItems)
    {
        AdjustmentNumber = adjustmentNumber;
        WarehouseId = warehouseId;
        AdjustmentDate = DateTime.Now.ToUtc();
        Reason = reason;
        ReasonDescription = reasonDescription;
        AdjusterId = adjusterId;
        Status = ApprovalStatus.Pending;
        ReferenceDocument = referenceDocument;
        Notes = notes;
        _stockAdjustmentItems = stockAdjustmentItems;
    }

    public void AddStockAdjustmentItem(StockAdjustmentItem stockAdjustmentItem)
    {
        _stockAdjustmentItems.Add(stockAdjustmentItem);
    }

    public void RemoveStockAdjustmentItem(StockAdjustmentItem stockAdjustmentItem)
    {
        _stockAdjustmentItems.Remove(stockAdjustmentItem);
    }

    public void Update(int warehouseId, StockAdjustmentReason reason, 
        string? reasonDescription, string? referenceDocument, string? notes)
    {
        WarehouseId = warehouseId;
        Reason = reason;
        ReasonDescription = reasonDescription;
        ReferenceDocument = referenceDocument;
        Notes = notes;
    }

    public void UpdateStatus(ApprovalStatus status)
    {
        Status = status;
    }

    public void Approve(string approverId)
    {
        Status = ApprovalStatus.Approved;
        ApproverId = approverId;
        ApprovedDate = DateTime.Now.ToUtc();
    }

    public void Reject(string approverId, string reason)
    {
        Status = ApprovalStatus.Rejected;
        ApproverId = approverId;
        ApprovedDate = DateTime.Now.ToUtc();
        Notes = $"{Notes}\nRejection reason: {reason}";
    }

}
