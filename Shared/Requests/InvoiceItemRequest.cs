namespace EdgIQ.Inventory.Shared.Requests;

public record InvoiceItemRequest
{
    public int ProductVariantId { get; init; }
    
    public int Quantity { get; init; }

    public decimal UnitPrice { get; init; }
}
