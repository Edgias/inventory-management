namespace EdgIQ.Inventory.Shared.Requests;

public record QuotationItemRequest
{
    public decimal UnitPrice { get; init; }

    public int Units { get; init; }

    public int ProductVariantId { get; init; } 

    public int? TaxId { get; init; }
}
