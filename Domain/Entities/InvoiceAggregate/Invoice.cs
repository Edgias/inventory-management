using EdgIQ.Inventory.Domain.Entities.GoodsReceivedNoteAggregate;

namespace EdgIQ.Inventory.Domain.Entities.InvoiceAggregate;

public class Invoice : BaseEntity
{
    public string InvoiceNumber { get; private set; }

    public string SupplierInvoiceNumber { get; private set; }

    public int SupplierId { get; private set; }
    public Supplier Supplier { get; private set; } = default!;

    public int GoodsReceivedNoteId { get; private set; }
    public GoodsReceivedNote GoodsReceivedNote { get; private set; } = default!;

    public DateTime InvoiceDate { get; private set; }

    public DateTime DueDate { get; private set; }

    public InvoiceStatus Status { get; private set; }

    public string? PaymentTerms { get; private set; }

    public int? TaxId {  get; private set; }
    public Tax? Tax { get; private set; }

    private readonly List<InvoiceItem> _invoiceItems = [];
    public IReadOnlyCollection<InvoiceItem> InvoiceItems => _invoiceItems.AsReadOnly();

    public Invoice(string invoiceNumber, string supplierInvoiceNumber, int supplierId,
        int goodsReceivedNoteId, DateTime invoiceDate, DateTime dueDate, InvoiceStatus status,
        string? paymentTerms, int? taxId)
    {
        InvoiceNumber = invoiceNumber;
        SupplierInvoiceNumber = supplierInvoiceNumber;
        SupplierId = supplierId;
        GoodsReceivedNoteId = goodsReceivedNoteId;
        InvoiceDate = invoiceDate;
        DueDate = dueDate;
        Status = status;
        PaymentTerms = paymentTerms;
        TaxId = taxId;
    }

    public Invoice(string invoiceNumber, string supplierInvoiceNumber, int supplierId,
        int goodsReceivedNoteId, DateTime invoiceDate, DateTime dueDate, InvoiceStatus status,
        string? paymentTerms, int? taxId, List<InvoiceItem> invoiceItems)
    {
        InvoiceNumber = invoiceNumber;
        SupplierInvoiceNumber = supplierInvoiceNumber;
        SupplierId = supplierId;
        GoodsReceivedNoteId = goodsReceivedNoteId;
        InvoiceDate = invoiceDate;
        DueDate = dueDate;
        Status = status;
        PaymentTerms = paymentTerms;
        TaxId = taxId;
        _invoiceItems = invoiceItems;
    }

    public void AddInvoiceItem(InvoiceItem invoiceItem)
    {
        _invoiceItems.Add(invoiceItem);
    }

    public void RemoveInvoiceItem(InvoiceItem invoiceItem)
    {
        _invoiceItems.Remove(invoiceItem);
    }

    public void Update(string supplierInvoiceNumber, int supplierId, int goodsReceivedNoteId, 
        DateTime invoiceDate, DateTime dueDate, string? paymentTerms, int? taxId)
    {
        SupplierInvoiceNumber = supplierInvoiceNumber;
        SupplierId = supplierId;
        GoodsReceivedNoteId = goodsReceivedNoteId;
        InvoiceDate = invoiceDate;
        DueDate = dueDate;
        PaymentTerms = paymentTerms;
        TaxId = taxId;
    }

    public void UpdateStatus(InvoiceStatus status)
    {
        Status = status;
    }

}
