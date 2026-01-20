namespace EdgIQ.Inventory.Shared.Requests;

public record RequestForQuotationItemRequest
{
    public int ProductVariantId { get; init; }

    public int QuantityRequired { get; init; }

    public string? Specifications { get; init; }

    public DateTime DateRequired { get; init; }

    public string? Notes { get; init; }
}
