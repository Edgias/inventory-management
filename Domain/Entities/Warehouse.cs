namespace EdgIQ.Inventory.Domain.Entities;

public class Warehouse : BaseEntity
{
    public string Name { get; private set; }

    public int LocationId { get; private set; }

    public Location Location { get; private set; }

    private readonly List<Bin> _bins = [];

    public IReadOnlyCollection<Bin> Bins => _bins.AsReadOnly();

    private Warehouse()
    {

    }

    public Warehouse(string name, int locationId)
    {
        Name = name;
        LocationId = locationId;
    }

    public void Update(string name)
    {
        Name = name;
    }

    public void ChangeLocation(int locationId)
    {
        LocationId = locationId;
    }
}
