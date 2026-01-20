using EdgIQ.Inventory.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace EdgIQ.Inventory.Shared.Requests;

public record InvoiceRequest
{
    [Required]
    public string SupplierInvoiceNumber { get; init; } = default!;

    public int SupplierId { get; init; }

    public int GoodsReceivedNoteId { get; init; }

    public DateTime InvoiceDate { get; init; }

    public DateTime DueDate { get; init; }

    public InvoiceStatus Status { get; init; }

    public string? PaymentTerms { get; init; }

    public int? TaxId { get; init; }

    public IEnumerable<InvoiceItemRequest> InvoiceItems { get; init; } = [];
}
