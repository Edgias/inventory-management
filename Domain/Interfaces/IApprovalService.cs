namespace EdgIQ.Inventory.Domain.Interfaces;

public interface IApprovalService
{
    Task<bool> ApprovePurchaseRequisitionItemAsync(int purchaseRequisitionItemId, string approverId, string comments);

    Task<bool> RejectPurchaseRequisitionItemAsync(int purchaseRequisitionItemId, string approverId, string comments);

    Task<bool> ApprovePurchaseOrderItemAsync(int purchaseOrderItemId, string approverId, string comments);

    Task<bool> RejectPurchaseOrderItemAsync(int purchaseOrderItemId, string approverId, string comments);

    Task<List<ApprovalLevel>> GetRequiredApproversAsync(decimal amount, int? department, DocumentType documentType);

    Task<bool> UserHasRoleAsync(string userId, string roleId);

    //Task<List<string>> GetUsersInRoleAsync(string roleId);
}
