using EdgIQ.Inventory.Shared.Enums;

namespace EdgIQ.Inventory.Shared.Responses;

public record InvoiceResponse : BaseResponse
{
    public string InvoiceNumber { get; init; } = default!;

    public string SupplierInvoiceNumber { get; init; } = default!;

    public ReferenceDataResponse<int> Supplier { get; init; } = default!;

    public int GoodsReceivedNoteId { get; init; }

    public DateTime InvoiceDate { get; init; }

    public DateTime DueDate { get; init; }

    public InvoiceStatus Status { get; init; }

    public string? PaymentTerms { get; init; }

    public ReferenceDataResponse<int>? Tax { get; init; }

    public IEnumerable<InvoiceItemResponse> InvoiceItems { get; init; } = [];
}
