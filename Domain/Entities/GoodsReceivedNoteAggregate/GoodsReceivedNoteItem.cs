using EdgIQ.Inventory.Domain.Entities.PurchaseOrderAggregate;

namespace EdgIQ.Inventory.Domain.Entities.GoodsReceivedNoteAggregate;

public class GoodsReceivedNoteItem : BaseEntity 
{
    public int GoodsReceivedNoteId { get; private set; }
    public GoodsReceivedNote GoodsReceivedNote { get; private set; } = default!;

    public ReceivedProduct ReceivedProduct { get; private set; } = default!;

    public int PurchaseOrderItemId { get; private set; }
    public PurchaseOrderItem PurchaseOrderItem { get; set; } = default!;
    
    public int? BinId { get; private set; }
    public Bin? Bin { get; private set; }

    public int QuantityOrdered { get; private set; } 

    public int QuantityReceived { get; private set; } 

    public string? BatchNumber { get; private set; }

    public DateTime? ExpiryDate { get; private set; }

    public string? Notes { get; private set; }

    public GoodsReceivedNoteItem(ReceivedProduct receivedProduct, int quantityOrdered, int quantityReceived, 
        int purchaseOrderItemId, int? binId, string? batchNumber, DateTime? expiryDate, string? notes)
    {
        ReceivedProduct = receivedProduct;
        QuantityOrdered = quantityOrdered;
        QuantityReceived = quantityReceived;
        PurchaseOrderItemId = purchaseOrderItemId;
        BinId = binId;
        BatchNumber = batchNumber;
        ExpiryDate = expiryDate;
        Notes = notes;
    }

    public void Update(ReceivedProduct receivedProduct, int quantityOrdered, int quantityReceived,
        int purchaseOrderItemId, int? binId, string? batchNumber, DateTime? expiryDate, string? notes)
    {
        ReceivedProduct = receivedProduct;
        QuantityOrdered = quantityOrdered;
        QuantityReceived = quantityReceived;
        PurchaseOrderItemId = purchaseOrderItemId;
        BinId = binId;
        BatchNumber = batchNumber;
        ExpiryDate = expiryDate;
        Notes = notes;
    }
}
