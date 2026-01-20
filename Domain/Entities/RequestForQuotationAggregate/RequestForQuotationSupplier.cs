namespace EdgIQ.Inventory.Domain.Entities.RequestForQuotationAggregate;

public class RequestForQuotationSupplier : BaseEntity
{
    public int RequestForQuotationId { get; private set; }
    public RequestForQuotation RequestForQuotation { get; private set; } = default!;

    public int SupplierId { get; private set; }
    public Supplier Supplier { get; private set; } = default!;

    public DateTime DateSent { get; private set; }

    public DateTime? DateResponded { get; private set; }

    public RFQStatus Status { get; private set; } 

    public string? Notes { get; private set; }

    public RequestForQuotationSupplier(int supplierId, DateTime dateSent,
        DateTime? dateResponded, string? notes)
    {
        SupplierId = supplierId;
        DateSent = dateSent;
        DateResponded = dateResponded;
        Status = RFQStatus.Draft;
        Notes = notes;
    }

    public void Update(int supplierId, DateTime dateSent,
        DateTime? dateResponded, string? notes)
    {
        SupplierId = supplierId;
        DateSent = dateSent;
        DateResponded = dateResponded;
        Notes = notes;
    }

    public void UpdateStatus(RFQStatus status)
    {
        Status = status;
    }

}
