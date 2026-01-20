namespace EdgIQ.Inventory.Shared.Responses;

public record RequestForQuotationItemResponse : BaseResponse
{
    public ReferenceDataResponse<int> Product { get; init; } = default!;

    public int QuantityRequired { get; init; }

    public string? Specifications { get; init; }

    public DateTime DateRequired { get; init; }

    public string? Notes { get; init; }
}
