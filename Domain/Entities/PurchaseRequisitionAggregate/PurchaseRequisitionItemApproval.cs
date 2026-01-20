namespace EdgIQ.Inventory.Domain.Entities.PurchaseRequisitionAggregate;

public class PurchaseRequisitionItemApproval : BaseEntity
{
    public int PurchaseRequisitionLineId { get; private set; }
    public PurchaseRequisitionItem PurchaseRequisitionLine { get; private set; } = default!;

    public string ApproverId { get; private set; }

    public int ApprovalLevelId { get; private set; }
    public ApprovalLevel ApprovalLevel { get; private set; } = default!;

    public ApprovalStatus Status { get; private set; }

    public DateTime? ApprovedDate { get; private set; }

    public string? Comments { get; private set; }

    public PurchaseRequisitionItemApproval(string approverId, int approvalLevelId, ApprovalStatus status,
        DateTime? approvedDate, string? comments)
    {
        ApproverId = approverId;
        ApprovalLevelId = approvalLevelId;
        Status = status;
        ApprovedDate = approvedDate;
        Comments = comments;
    }

    public void Update(ApprovalStatus status, DateTime? approvedDate, string? comments)
    {
        Status = status;
        ApprovedDate = approvedDate;
        Comments = comments;
    }
}
