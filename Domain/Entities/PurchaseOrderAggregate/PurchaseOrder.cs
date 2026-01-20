using EdgIQ.Inventory.Domain.Entities.QuotationAggregate;

namespace EdgIQ.Inventory.Domain.Entities.PurchaseOrderAggregate;

public class PurchaseOrder : BaseEntity
{
    public string PONumber { get; private set; }
    
    public DateTime OrderDate { get; private set; }

    public DateTime ExpectedDeliveryDate { get; private set; }

    public ApprovalStatus Status { get; private set; }

    public Address? ShippingAddress { get; private set; }

    public string? Terms { get; private set; }

    public int QuotationId { get; private set; }
    public Quotation Quotation { get; private set; } = default!;

    public int SupplierId { get; private set; }
    public Supplier Supplier { get; private set; } = default!;

    public int? WarehouseId { get; private set; }
    public Warehouse? Warehouse { get; private set; }

    private readonly List<PurchaseOrderApproval> _purchaseOrderApprovals = [];
    public IReadOnlyCollection<PurchaseOrderApproval> PurchaseOrderApprovals => _purchaseOrderApprovals.AsReadOnly();

    private readonly List<PurchaseOrderItem> _purchaseOrderItems = [];
    public IReadOnlyCollection<PurchaseOrderItem> PurchaseOrderItems => _purchaseOrderItems.AsReadOnly();

    public PurchaseOrder(string poNumber, DateTime orderDate, DateTime expectedDeliveryDate, int quotationId, 
        int supplierId, int? warehouseId, Address? shippingAddress, string? terms)
    {
        PONumber = poNumber;
        OrderDate = orderDate;
        ExpectedDeliveryDate = expectedDeliveryDate;
        QuotationId = quotationId;
        SupplierId = supplierId;
        WarehouseId = warehouseId;
        Status = ApprovalStatus.Pending;
        ShippingAddress = shippingAddress;
        Terms = terms;
    }

    public PurchaseOrder(string poNumber, DateTime orderDate, DateTime expectedDeliveryDate, int quotationId,
        int supplierId, int? warehouseId, Address? shippingAddress, string? terms,
        List<PurchaseOrderItem> purchaseOrderItems)
    {
        PONumber = poNumber;
        OrderDate = orderDate;
        ExpectedDeliveryDate = expectedDeliveryDate;
        QuotationId = quotationId;
        SupplierId = supplierId;
        WarehouseId = warehouseId;
        Status = ApprovalStatus.Pending;
        ShippingAddress = shippingAddress;
        Terms = terms;
        _purchaseOrderItems = purchaseOrderItems;
    }

    public void AddPurchaseOrderApproval(PurchaseOrderApproval purchaseOrderApproval)
    {
        _purchaseOrderApprovals.Add(purchaseOrderApproval);
    }

    public void RemovePurchaseOrderApproval(PurchaseOrderApproval purchaseOrderApproval)
    {
        _purchaseOrderApprovals.Remove(purchaseOrderApproval);
    }

    public void AddPurchaseOrderItem(PurchaseOrderItem purchaseOrderItem)
    {
        _purchaseOrderItems.Add(purchaseOrderItem);
    }

    public void RemovePurchaseOrderItem(PurchaseOrderItem purchaseOrderItem)
    {
        _purchaseOrderItems.Remove(purchaseOrderItem);
    }

    public void UpdatePurchaseOrderItem(PurchaseOrderItem purchaseOrderItem)
    {
        PurchaseOrderItem? item = _purchaseOrderItems.Find(qi => qi.Id == purchaseOrderItem.Id);
        if (item != null)
        {
            int itemIndex = _purchaseOrderItems.IndexOf(item);
            _purchaseOrderItems.Insert(itemIndex, purchaseOrderItem);
        }
    }

    public void Update(DateTime expectedDeliveryDate, int quotationId, int supplierId, 
        int? warehouseId, Address? shippingAddress, string? terms)
    {
        ExpectedDeliveryDate = expectedDeliveryDate;
        QuotationId = quotationId;
        SupplierId = supplierId;
        WarehouseId = warehouseId;
        ShippingAddress = shippingAddress;
        Terms = terms;
    }

    public void UpdateStatus(ApprovalStatus status)
    {
        Status = status;
    }

    public decimal GetTotal()
    {
        decimal total = 0m;

        foreach (PurchaseOrderItem purchaseOrderItem in _purchaseOrderItems)
        {
            decimal lineTotal = purchaseOrderItem.UnitPrice * purchaseOrderItem.Units;
            total += lineTotal;
        }

        return total;
    }
}
