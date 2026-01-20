namespace EdgIQ.Inventory.Shared.Requests;

public record QuotationRequest
{
    public DateTime QuotationDate { get; init; }

    public DateTime? ExpiryDate { get; init; }

    public string? Terms { get; init; }

    public int PurchaseRequisitionId { get; init; }

    public int? SupplierId { get; init; }

    public int? RequestForQuotationId { get; set; }

    public IEnumerable<QuotationItemRequest> QuotationItems { get; init; } = [];
}
