namespace EdgIQ.Inventory.Domain.Entities;

public class Warehouse : BaseEntity
{
    public string Name { get; private set; } = default!;

    public string? Phone {  get; private set; } 

    public string? ManagerId { get; private set; }

    public Address? LocationAddress { get; private set; } = default!;

    private readonly List<WarehouseStock> _warehouseStocks = [];
    public IReadOnlyCollection<WarehouseStock> WarehouseStocks => _warehouseStocks.AsReadOnly();

    private Warehouse()
    {
    }

    public Warehouse(string name, string? phone, string? managerId, Address? locationAddress)
    {
        Name = name;
        Phone = phone;
        ManagerId = managerId;
        LocationAddress = locationAddress;
    }

    public void Update(string name, string? phone, string? managerId, Address? locationAddress)
    {
        Name = name;
        Phone = phone;
        ManagerId = managerId;
        LocationAddress = locationAddress;
    }

    public void ChangeManager(string managerId)
    { 
        ManagerId = managerId; 
    }
}
