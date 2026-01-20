namespace EdgIQ.Inventory.Domain.Entities;

public class Bin : BaseEntity
{
    public string Name { get; private set; } = default!;

    public string? SerialNumber { get; private set; }

    public string? Color { get; private set; } 

    public int? Width { get; private set; }

    public int? Depth { get; private set; }

    public decimal? Height { get; private set; }

    public int? DividerSlots { get; private set; }

    public decimal? Weight { get; private set; }

    public int WarehouseId { get; private set; }

    public Warehouse Warehouse { get; private set; } = default!;

    private readonly List<BinStock> _binStocks = [];
    public IReadOnlyCollection<BinStock> BinStocks => _binStocks.AsReadOnly();

    private Bin()
    {
    }

    public Bin(string name, string? serialNumber, string? color, int? width, int? depth, decimal? height, 
        int? dividerSlots, decimal? weight, int warehouseId)
    {
        Name = name;
        SerialNumber = serialNumber;
        Color = color;
        Width = width;
        Depth = depth;
        Height = height;
        DividerSlots = dividerSlots;
        Weight = weight;
        WarehouseId = warehouseId;
    }

    public Bin(string name, string? serialNumber, string? color, int? width, int? depth, decimal? height, 
        int? dividerSlots, decimal? weight, int warehouseId, List<BinStock> binStocks)
    {
        Name = name;
        SerialNumber = serialNumber;
        Color = color;
        Width = width;
        Depth = depth;
        Height = height;
        DividerSlots = dividerSlots;
        Weight = weight;
        WarehouseId = warehouseId;
        _binStocks = binStocks;
    }

    public void AddInventoryLevel(BinStock binStock)
    {
        _binStocks.Add(binStock);
    }

    public void RemoveInventoryLevel(BinStock binStock)
    {
        _binStocks.Remove(binStock);
    }

    public void Update(string name, string? serialNumber, string? color, int? width, int? depth, decimal? height, 
        int? dividerSlots, decimal? weight)
    {
        Name = name;
        SerialNumber = serialNumber;
        Color = color;
        Width = width;
        Depth = depth;
        Height = height;
        DividerSlots = dividerSlots;
        Weight = weight;
    }

    public void ChangeWarehouse(int warehouseId)
    {
        WarehouseId = warehouseId;
    }
}
