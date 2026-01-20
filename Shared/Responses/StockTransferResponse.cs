namespace EdgIQ.Inventory.Shared.Responses;

public record StockTransferResponse : BaseResponse
{
    public string TransferNumber { get; init; } = default!;

    public ReferenceDataResponse<int> FromWarehouse { get; init; } = default!;

    public ReferenceDataResponse<int> ToWarehouse { get; init; } = default!;

    public DateTime TransferDate { get; init; }

    public DateTime? ShipmentDate { get; init; }

    public DateTime? ReceivedDate { get; init; }

    public ReferenceDataResponse<string>? Approver { get; init; }

    public string? Notes { get; init; }

    public IEnumerable<StockTransferItemResponse> StockTransferItems { get; init; } = [];
}
