namespace EdgIQ.Inventory.Domain.Entities.PurchaseRequisitionAggregate;

public class PurchaseRequisitionApproval : BaseEntity
{
    public int PurchaseRequisitionId { get; private set; }
    public PurchaseRequisition PurchaseRequisition { get; private set; } = default!;

    public string ApproverId { get; private set; }

    public int ApprovalLevelId { get; private set; }

    public ApprovalLevel ApprovalLevel { get; private set; } = default!;

    public ApprovalStatus Status { get; private set; }

    public DateTime? ApprovedDate { get; private set; }

    public string? Comments { get; private set; }

    public PurchaseRequisitionApproval(string approverId, int approvalLevelId, ApprovalStatus status,
        DateTime? approvedDate, string? comments)
    {
        ApproverId = approverId;
        ApprovalLevelId = approvalLevelId;
        Status = status;
        ApprovedDate = approvedDate;
        Comments = comments;
    }
}
