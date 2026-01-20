using EdgIQ.Inventory.Domain.Entities.PurchaseRequisitionAggregate;
using EdgIQ.Inventory.Domain.Entities.RequestForQuotationAggregate;

namespace EdgIQ.Inventory.Domain.Entities.QuotationAggregate;

public class Quotation : BaseEntity
{
    public string QuotationNumber { get; private set; } = default!;

    public DateTime QuotationDate { get; private set; }

    public DateTime? ExpiryDate { get; private set; }

    public string? Terms { get; private set; }

    public int PurchaseRequisitionId { get; private set; }
    public PurchaseRequisition PurchaseRequisition { get; private set; } = default!;

    public int? SupplierId { get; private set; }
    public Supplier? Supplier { get; private set; }

    public int? RequestForQuotationId { get; set; }
    public RequestForQuotation? RequestForQuotation { get; set; }

    private readonly List<QuotationItem> _quotationItems = [];
    public IReadOnlyCollection<QuotationItem> QuotationItems => _quotationItems.AsReadOnly();

    private Quotation()
    {
    }

    public Quotation(string quotationNumber, int purchaseRequisitionId, DateTime quotationDate, 
        DateTime? expiryDate, int? supplierId, int? requestForQuotationId, string? terms)
    {
        QuotationNumber = quotationNumber;
        PurchaseRequisitionId = purchaseRequisitionId;
        QuotationDate = quotationDate;
        ExpiryDate = expiryDate;
        SupplierId = supplierId;
        RequestForQuotationId = requestForQuotationId;
        Terms = terms;
    }

    public Quotation(string quotationNumber, int purchaseRequisitionId, DateTime quotationDate,
        DateTime? expiryDate, int? supplierId, int? requestForQuotationId, string? terms, 
        List<QuotationItem> quotationItems)
    {
        QuotationNumber = quotationNumber;
        PurchaseRequisitionId = purchaseRequisitionId;
        QuotationDate = quotationDate;
        ExpiryDate = expiryDate;
        SupplierId = supplierId;
        RequestForQuotationId = requestForQuotationId;
        Terms = terms;
        _quotationItems = quotationItems;
    }

    public void Update(DateTime quotationDate, DateTime? expiryDate, 
        int? supplierId, int? requestForQuotationId, string? terms)
    {
        QuotationDate = quotationDate;
        ExpiryDate = expiryDate;
        SupplierId = supplierId;
        RequestForQuotationId = requestForQuotationId;
        Terms = terms;
    }

    public void AddQuotationItem(QuotationItem quotationItem)
    {
        _quotationItems.Add(quotationItem);
    }

    public void RemoveQuotationItem(QuotationItem quotationItem)
    {
        _quotationItems.Remove(quotationItem);
    }

    public void UpdateQuotationItem(QuotationItem quotationItem)
    {
        QuotationItem? item = _quotationItems.Find(qi => qi.Id == quotationItem.Id);
        if(item != null)
        {
            int itemIndex = _quotationItems.IndexOf(item);
            _quotationItems.Insert(itemIndex, quotationItem);
        }
    }

    public void UpdateQuotationDate(DateTime quotationDate)
    {
        QuotationDate = quotationDate;
    }

    public void UpdateExpiryDate(DateTime expiryDate)
    {
        ExpiryDate = expiryDate;
    }

    public decimal GetTotal()
    {
        decimal total = 0m;

        foreach (QuotationItem quotationItem in _quotationItems)
        {
            decimal lineTotal = quotationItem.UnitPrice * quotationItem.Units;
            total += lineTotal;
        }

        return total;
    }

}
