namespace EdgIQ.Inventory.Domain.Interfaces;

public interface IStockAdjustmentService
{
    Task<bool> ApproveStockAdjustmentAsync(int stockAdjustmentId, string approverId);

    Task<bool> RejectStockAdjustmentAsync(int stockAdjustmentId, string approverId, string reason);
}
