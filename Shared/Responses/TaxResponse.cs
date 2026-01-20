namespace EdgIQ.Inventory.Shared.Responses;

public record TaxResponse : BaseResponse
{
    public string Name { get; init; } = default!;

    public decimal Percentage { get; init; }
}
