namespace EdgIQ.Inventory.Shared.Responses;

public record StockTransferItemResponse : BaseResponse
{
    public int QuantityRequested { get; init; }

    public int QuantityShipped { get; init; }

    public int QuantityReceived { get; init; }

    public ReferenceDataResponse<int>? FromBin { get; init; }

    public ReferenceDataResponse<int>? ToBin { get; init; }

    public string? Notes { get; init; }

}
