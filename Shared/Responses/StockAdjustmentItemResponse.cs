namespace EdgIQ.Inventory.Shared.Responses;

public record StockAdjustmentItemResponse : BaseResponse
{
    public ReferenceDataResponse<int> Product { get; init; } = default!;

    public ReferenceDataResponse<int>? Batch { get; init; }

    public ReferenceDataResponse<int>? SerialNumber { get; init; }

    public ReferenceDataResponse<int>? Bin { get; init; }

    public int SystemQuantity { get; init; }

    public int ActualQuantity { get; init; }

    public decimal UnitPrice { get; init; }

    public string? Notes { get; init; }
}
