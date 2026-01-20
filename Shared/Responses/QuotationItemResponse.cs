namespace EdgIQ.Inventory.Shared.Responses;

public record QuotationItemResponse : BaseResponse
{
    public decimal UnitPrice { get; init; }

    public int Units { get; init; }

    public ReferenceDataResponse<int> Product { get; init; } = default!;
}
