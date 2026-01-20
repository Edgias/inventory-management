namespace EdgIQ.Inventory.Shared.Requests;

public record RequestForQuotationSupplierRequest
{
    public int RequestForQuotationId { get; init; }

    public int SupplierId { get; init; }

    public DateTime DateSent { get; init; }

    public DateTime? DateResponded { get; init; }

    public string? Notes { get; init; }
}
