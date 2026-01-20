namespace EdgIQ.Inventory.Shared.Responses;

public record RequestForQuotationSupplierResponse : BaseResponse
{
    public string Name { get; init; } = default!;

    public string? Email { get; init; } = default!;

    public string? Phone { get; init; } = default!;

    public DateTime DateSent { get; init; }

    public DateTime? DateResponded { get; init; }

    public string? Notes { get; init; }
}
