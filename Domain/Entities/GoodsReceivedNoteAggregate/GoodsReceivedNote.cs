
using EdgIQ.Inventory.Domain.Entities.PurchaseOrderAggregate;

namespace EdgIQ.Inventory.Domain.Entities.GoodsReceivedNoteAggregate;

public class GoodsReceivedNote : BaseEntity
{
    public string GRNNumber { get; private set; } = default!;

    public DateTime ReceivedDate { get; private set; } = DateTime.UtcNow;

    public int SupplierId { get; private set; }
    public Supplier Supplier { get; private set; } = default!;

    public int WarehouseId { get; private set; } 
    public Warehouse Warehouse { get; private set; } = default!;

    public int PurchaseOrderId { get; private set; }
    public PurchaseOrder PurchaseOrder { get; private set; } = default!;

    public string ReceiverId { get; private set; }

    public string? ExternalReference { get; private set; } 

    public GrnStatus Status { get; private set; } = GrnStatus.Draft;

    public string? Notes { get; private set; }

    private readonly List<GoodsReceivedNoteItem> _goodsReceivedNoteItems = [];
    public IReadOnlyCollection<GoodsReceivedNoteItem> GoodsReceivedNoteItems => _goodsReceivedNoteItems.AsReadOnly();

    public GoodsReceivedNote(string grnNumber, DateTime dateReceived, int supplierId, int warehouseId, 
        int purchaseOrderId, string receiverId, string? externalReference, string? notes)
    {
        GRNNumber = grnNumber;
        ReceivedDate = dateReceived;
        SupplierId = supplierId;
        WarehouseId = warehouseId;
        PurchaseOrderId = purchaseOrderId;
        ReceiverId = receiverId;
        ExternalReference = externalReference;
        Status = GrnStatus.Draft;
        Notes = notes;
    }

    public GoodsReceivedNote(string grnNumber, DateTime dateReceived, int supplierId, int warehouseId, 
        int purchaseOrderId, string receiverId, string? externalReference, 
        List<GoodsReceivedNoteItem> goodsReceivedNoteItems, string? notes)
    {
        GRNNumber = grnNumber;
        ReceivedDate = dateReceived;
        SupplierId = supplierId;
        WarehouseId = warehouseId;
        PurchaseOrderId = purchaseOrderId;
        ReceiverId = receiverId;
        ExternalReference = externalReference;
        Status = GrnStatus.Draft;
        _goodsReceivedNoteItems = goodsReceivedNoteItems;
        Notes = notes;
    }

    public void AddGoodsReceivedNoteItem(GoodsReceivedNoteItem item)
    {
        _goodsReceivedNoteItems.Add(item);
    }

    public void RemoveGoodsReceivedNoteItem(GoodsReceivedNoteItem item)
    {
        _goodsReceivedNoteItems.Remove(item);
    }

    public void Update(DateTime dateReceived, int supplierId, int warehouseId,
        int purchaseOrderId, string? externalReference, string? notes)
    {
        ReceivedDate = dateReceived;
        SupplierId = supplierId;
        WarehouseId = warehouseId;
        PurchaseOrderId = purchaseOrderId;
        ExternalReference = externalReference;
        Notes = notes;
    }

    public void UpdateSupplier(int supplierId)
    {
        SupplierId = supplierId;
    }

    public void UpdateStatus(GrnStatus status)
    {
        Status = status;
    }

    public void UpdateWarehouse(int warehouseId)
    {
        WarehouseId = warehouseId;
    }

}
