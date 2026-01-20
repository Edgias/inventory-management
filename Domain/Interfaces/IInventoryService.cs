namespace EdgIQ.Inventory.Domain.Interfaces;

public interface IInventoryService
{
    Task<bool> TransferStockAsync(int variantId, int fromWarehouseId, int toWarehouseId, int quantity, string userId);
}
