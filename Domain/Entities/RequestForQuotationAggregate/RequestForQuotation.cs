using EdgIQ.Inventory.Domain.Entities.PurchaseRequisitionAggregate;
using EdgIQ.Inventory.Domain.Entities.QuotationAggregate;

namespace EdgIQ.Inventory.Domain.Entities.RequestForQuotationAggregate;

public class RequestForQuotation : BaseEntity
{
    public string RFQNumber { get; private set; }

    public int? PurchaseRequisitionId { get; private set; }
    public PurchaseRequisition? PurchaseRequisition { get; private set; }

    public DateTime RFQDate { get; private set; }

    public DateTime ResponseDeadline { get; private set; }

    public string? Description { get; private set; }

    public string? Terms { get; private set; }

    public Address? DeliveryLocation { get; private set; }

    public string RequesterId { get; private set; }

    public RFQStatus Status { get; private set; }

    private readonly List<RequestForQuotationItem> _requestForQuotationItems = [];
    public IReadOnlyCollection<RequestForQuotationItem> RequestForQuotationItems => _requestForQuotationItems.AsReadOnly();

    private readonly List<RequestForQuotationSupplier> _requestForQuotationSuppliers = [];
    public IReadOnlyCollection<RequestForQuotationSupplier> RequestForQuotationSuppliers => _requestForQuotationSuppliers.AsReadOnly();

    private readonly List<Quotation> _quotations = [];
    public IReadOnlyCollection<Quotation> Quotations => _quotations.AsReadOnly();

    public RequestForQuotation(string rfqNumber, int? purchaseRequisitionId, DateTime rfqDate,
        DateTime responseDeadline, string? description, string? terms, Address? deliveryLocation,
        string requesterId)
    {
        RFQNumber = rfqNumber;
        PurchaseRequisitionId = purchaseRequisitionId;
        RFQDate = rfqDate;
        ResponseDeadline = responseDeadline;
        Description = description;
        Terms = terms;
        DeliveryLocation = deliveryLocation;
        RequesterId = requesterId;
        Status = RFQStatus.Draft;
    }

    public RequestForQuotation(string rfqNumber, int? purchaseRequisitionId, DateTime rfqDate,
        DateTime responseDeadline, string? description, string? terms, Address? deliveryLocation,
        string requesterId, List<RequestForQuotationItem> requestForQuotationItems)
    {
        RFQNumber = rfqNumber;
        PurchaseRequisitionId = purchaseRequisitionId;
        RFQDate = rfqDate;
        ResponseDeadline = responseDeadline;
        Description = description;
        Terms = terms;
        DeliveryLocation = deliveryLocation;
        RequesterId = requesterId;
        Status = RFQStatus.Draft;
        _requestForQuotationItems = requestForQuotationItems;
    }

    public void AddRequestForQuotationItem(RequestForQuotationItem requestForQuotationItem)
    {
        _requestForQuotationItems.Add(requestForQuotationItem);
    }

    public void RemoveRequestForQuotationItem(RequestForQuotationItem requestForQuotationItem)
    {
        _requestForQuotationItems.Remove(requestForQuotationItem);
    }

    public void AddRequestForQuotationSupplier(RequestForQuotationSupplier requestForQuotationSupplier)
    {
        _requestForQuotationSuppliers.Add(requestForQuotationSupplier);
    }

    public void RemoveRequestForQuotationSupplier(RequestForQuotationSupplier requestForQuotationSupplier)
    {
        _requestForQuotationSuppliers.Remove(requestForQuotationSupplier);
    }

    public void AddQuotation(Quotation quotation)
    {
        _quotations.Add(quotation);
    }

    public void RemoveQuotation(Quotation quotation)
    {
        _quotations.Remove(quotation);
    }

    public void Update(int? purchaseRequisitionId, DateTime rfqDate,
        DateTime responseDeadline, string? description, string? terms, Address? deliveryLocation)
    {
        PurchaseRequisitionId = purchaseRequisitionId;
        RFQDate = rfqDate;
        ResponseDeadline = responseDeadline;
        Description = description;
        Terms = terms;
        DeliveryLocation = deliveryLocation;
    }

    public void UpdateStatus(RFQStatus status)
    {
        Status = status;
    }
}