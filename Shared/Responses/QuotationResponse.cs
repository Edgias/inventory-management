namespace EdgIQ.Inventory.Shared.Responses;

public record QuotationResponse : BaseResponse
{
    public string QuotationNumber { get; init; } = default!;

    public DateTime QuotationDate { get; init; }

    public DateTime? ExpiryDate { get; init; }

    public string? Terms { get; init; }

    public decimal TotalAmount { get; init; }

    public ReferenceDataResponse<int> PurchaseRequisition { get; init; } = default!;

    public ReferenceDataResponse<int>? Supplier { get; init; }

    public ReferenceDataResponse<int>? RequestForQuotation { get; set; }

    public IEnumerable<QuotationItemResponse> QuotationItems { get; init; } = [];
}
