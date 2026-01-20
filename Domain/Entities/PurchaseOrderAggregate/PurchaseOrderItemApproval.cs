namespace EdgIQ.Inventory.Domain.Entities.PurchaseOrderAggregate;

public class PurchaseOrderItemApproval : BaseEntity
{
    public int PurchaseOrderItemId { get; private set; }
    public PurchaseOrderItem PurchaseOrderItem { get; private set; } = default!;

    public string ApproverId { get; private set; }
    
    public int ApprovalLevelId { get; private set; }
    public ApprovalLevel ApprovalLevel { get; private set; } = default!;

    public ApprovalStatus Status { get; private set; }

    public DateTime? ApprovedDate { get; private set; }

    public string? Comments { get; private set; }

    public PurchaseOrderItemApproval(string approverId, int approvalLevelId, ApprovalStatus status,
        DateTime? approvedDate, string? comments)
    {
        ApproverId = approverId;
        ApprovalLevelId = approvalLevelId;
        Status = status;
        ApprovedDate = approvedDate;
        Comments = comments;
    }

    public void Update(ApprovalStatus status, string? comments)
    {
        Status = status;
        ApprovedDate = DateTime.UtcNow;
        Comments = comments;
    }

    public void UpdateStatus(ApprovalStatus status)
    {
        Status = status;
    }

}
