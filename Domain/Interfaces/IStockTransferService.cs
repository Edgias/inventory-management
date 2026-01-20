using EdgIQ.Inventory.Shared.Requests;

namespace EdgIQ.Inventory.Domain.Interfaces;

public interface IStockTransferService
{
    Task<bool> ApproveStockTransferAsync(int transferId, string approverId);

    Task<bool> ShipStockTransferAsync(int transferId);

    Task<bool> ReceiveStockTransferAsync(int transferId, List<StockTransferItemRequest> stockTransferItems);

    Task<bool> CancelStockTransferAsync(int transferId);
}
