namespace EdgIQ.Inventory.Shared.Responses;

public record StockResponse : BaseResponse
{
    public ReferenceDataResponse<int> Product { get; init; } = default!;

    public ReferenceDataResponse<int> Warehouse { get; init; } = default!;

    public ReferenceDataResponse<int>? Batch { get; init; }

    public ReferenceDataResponse<int>? Bin { get; init; }

    public int QuantityOnHand { get; init; }

    // Stock sold but not yet shipped
    public int QuantityReserved { get; init; }

    public DateTime? LastStockDate { get; init; }
}
