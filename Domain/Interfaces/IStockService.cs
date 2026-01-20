namespace EdgIQ.Inventory.Domain.Interfaces;

public interface IStockService
{
    Task<WarehouseStock> GetWarehouseStockAsync(int warehouseId, int productVariantId);

    Task<bool> UpdateWarehouseStockAsync(int warehouseId, int productVariantId, int quantity);

    Task<bool> UpdateBinStockAsync(int binId, int productVariantId, int quantity);

    Task<bool> TransferStockBetweenBinsAsync(int fromBinId, int toBinId, int productVariantId, int quantity);

    Task<List<WarehouseStock>> GetLowStockItemsAsync(int warehouseId);

    Task<List<BinStock>> GetBinStocksByWarehouseAsync(int warehouseId);
}
