namespace EdgIQ.Inventory.Domain.Entities;

public class StockTransaction : BaseEntity
{
    public string TransactionNumber { get; private set; }

    public DateTime TransactionDate { get; private set; }

    public int ProductVariantId { get; private set; }
    public ProductVariant ProductVariant { get; private set; } = default!;

    public int? BatchId { get; private set; }
    public Batch? Batch { get; private set; }

    public int? SerialNumberId { get; private set; }
    public SerialNumber? SerialNumber { get; private set; }

    public int? WarehouseId { get; private set; }
    public Warehouse? Warehouse { get; private set; }

    public int? BinId { get; private set; }
    public Bin? Bin { get; private set; }

    // Positive for restock, negative for sales
    public int Quantity { get; private set; }

    public decimal UnitPrice { get; private set; }

    public TransactionType Type { get; private set; }

    public DocumentType DocumentType { get; private set; } 

    public int? DocumentId { get; private set; }

    public string? Notes { get; private set; }

    public StockTransaction(string transactionNumber, DateTime transactionDate, int productVariantId, int? batchId, 
        int? serialNumberId, int? warehouseId, int? binId, int quantity, decimal unitPrice, 
        TransactionType transactionType, DocumentType documentType, int? documentId)
    {
        TransactionNumber = transactionNumber;
        TransactionDate = transactionDate;
        ProductVariantId = productVariantId;
        BatchId = batchId;
        SerialNumberId = serialNumberId;
        WarehouseId = warehouseId;
        BinId = binId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Type = transactionType;
        DocumentType = documentType;
        DocumentId = documentId;
    }
}
