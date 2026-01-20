namespace EdgIQ.Inventory.Domain.Entities;

public class ProductVariant : BaseEntity
{
    public int ProductId { get; private set; }
    public Product Product { get; private set; } = default!;

    public string SKU { get; private set; } = default!; 

    public string? Barcode { get; private set; } 

    public decimal UnitPrice { get; private set; }

    public string? Color { get; private set; } 

    public string? Size { get; private set; } 

    public int ReorderPoint { get; private set; }

    public bool IsBatchTracked { get; private set; }

    public bool IsSerialTracked { get; private set; }

    public bool HasExpiryDate { get; private set; }

    public int? ShelfLifeDays { get; private set; }

    public CostingMethod CostingMethod { get; private set; }

    public int? ExpiryAlertDays { get; private set; }

    public int MeasurementUnitId { get; private set; }
    public MeasurementUnit MeasurementUnit { get; private set; } = default!;

    private readonly List<WarehouseStock> _inventoryLevels = [];
    public IReadOnlyCollection<WarehouseStock> InventoryLevels => _inventoryLevels.AsReadOnly();

    public ProductVariant(int productId, string sku, string? barCode, decimal unitPrice, string? color, 
        string? size, int reorderPoint, bool isBatchTracked, bool isSerialTracked, bool hasExpiryDate, 
        int? shelfLifeDays, int? expiryAlertDays, CostingMethod costingMethod, int measurementUnitId)
    {
        ProductId = productId;
        SKU = sku;
        Barcode = barCode;
        UnitPrice = unitPrice;
        Color = color;
        Size = size;
        ReorderPoint = reorderPoint;
        IsBatchTracked = isBatchTracked;
        IsSerialTracked = isSerialTracked;
        HasExpiryDate = hasExpiryDate;
        ShelfLifeDays = shelfLifeDays;
        ExpiryAlertDays = expiryAlertDays;
        CostingMethod = costingMethod;
        MeasurementUnitId = measurementUnitId;
    }

    public ProductVariant(int productId, string sku, string? barCode, decimal unitPrice, string? color,
        string? size, int reorderPoint, bool isBatchTracked, bool isSerialTracked, bool hasExpiryDate,
        int? shelfLifeDays, int? expiryAlertDays, CostingMethod costingMethod, int measurementUnitId, 
        List<WarehouseStock> inventoryLevels)
    {
        ProductId = productId;
        SKU = sku;
        Barcode = barCode;
        UnitPrice = unitPrice;
        Color = color;
        Size = size;
        ReorderPoint = reorderPoint;
        IsBatchTracked = isBatchTracked;
        IsSerialTracked = isSerialTracked;
        HasExpiryDate = hasExpiryDate;
        ShelfLifeDays = shelfLifeDays;
        ExpiryAlertDays = expiryAlertDays;
        CostingMethod = costingMethod;
        MeasurementUnitId = measurementUnitId;
        _inventoryLevels = inventoryLevels;
    }

    public void Update(string sku, string? barCode, string? color, string? size, int reorderPoint, 
        bool isBatchTracked, bool isSerialTracked, bool hasExpiryDate, int? shelfLifeDays, 
        int? expiryAlertDays, CostingMethod costingMethod, int measurementUnitId)
    {
        SKU = sku;
        Barcode = barCode;
        Color = color;
        Size = size;
        ReorderPoint = reorderPoint;
        IsBatchTracked = isBatchTracked;
        IsSerialTracked = isSerialTracked;
        HasExpiryDate = hasExpiryDate;
        ShelfLifeDays = shelfLifeDays;
        ExpiryAlertDays = expiryAlertDays;
        CostingMethod = costingMethod;
        MeasurementUnitId = measurementUnitId;
    }

    public void AddInventoryLevel(WarehouseStock inventoryLevel)
    {
        _inventoryLevels.Add(inventoryLevel);
    }

    public void RemoveInventoryLevel(WarehouseStock inventoryLevel)
    {
        _inventoryLevels.Remove(inventoryLevel);
    }

    public void UpdateReorderPoint(int newReorderPoint)
    {
        ReorderPoint = newReorderPoint;
    }

    public void UpdatePrice(decimal newPrice)
    {
        UnitPrice = newPrice;
    }
}
