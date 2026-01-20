namespace EdgIQ.Inventory.Shared.Requests;

public record StockTransferRequest
{
    public int FromWarehouseId { get; init; }

    public int ToWarehouseId { get; init; }

    public DateTime TransferDate { get; init; }

    public DateTime? ShipmentDate { get; init; }

    public DateTime? ReceivedDate { get; init; }

    public string? ApproverId { get; init; }

    public string? Notes { get; init; }

    public IEnumerable<StockTransferItemRequest> StockTransferItems { get; init; } = [];
}
