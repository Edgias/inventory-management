namespace EdgIQ.Inventory.Domain.Entities;

public class SerialNumber : BaseEntity
{
    public string SerialNo { get; private set; }

    public int ProductVariantId { get; private set; }
    public ProductVariant ProductVariant { get; private set; } = default!;

    public int? BatchId { get; private set; }
    public Batch? Batch { get; private set; }

    public int? WarehouseId { get; private set; }
    public Warehouse? Warehouse { get; private set; }

    public int? BinId { get; private set; }
    public Bin? Bin { get; private set; }

    public SerialNumberStatus Status { get; private set; } 

    public DateTime? PurchaseDate { get; private set; }

    public DateTime? SaleDate { get; private set; }

    public decimal? PurchaseCost { get; private set; }

    public string? WarrantyInformation { get; private set; }

    public string? Notes { get; private set; }

    public SerialNumber(string serialNo, int productVariantId, int? batchId, int? warehouseId,
        int? binId, DateTime? purchaseDate, DateTime? saleDate, decimal? purchaseCost, 
        string? warrantyInformation, string? notes)
    {
        SerialNo = serialNo;
        ProductVariantId = productVariantId;
        BatchId = batchId;
        WarehouseId = warehouseId;
        BinId = binId;
        Status = SerialNumberStatus.InStock;
        PurchaseDate = purchaseDate;
        SaleDate = saleDate;
        PurchaseCost = purchaseCost;
        WarrantyInformation = warrantyInformation;
        Notes = notes;
    }

    public void Update(int productVariantId, int? batchId, int? warehouseId,
        int? binId, DateTime? purchaseDate, DateTime? saleDate,
        decimal? purchaseCost, string? warrantyInformation, string? notes)
    {
        ProductVariantId = productVariantId;
        BatchId = batchId;
        WarehouseId = warehouseId;
        BinId = binId;
        PurchaseDate = purchaseDate;
        SaleDate = saleDate;
        PurchaseCost = purchaseCost;
        WarrantyInformation = warrantyInformation;
        Notes = notes;
    }

    public void UpdateStatus(SerialNumberStatus status)
    {
        Status = status;
        if(status == SerialNumberStatus.Sold)
        {
            SaleDate = DateTime.UtcNow;
        }
    }
}
