namespace EdgIQ.Inventory.Domain.Entities;

public class Location : BaseEntity
{
    public string Name { get; private set; }

    public Address LocationAddress { get; private set; }

    private Location()
    {
    }

    public Location(string name)
    {
        Name = name;
    }

    public Location(string name, Address locationAddress)
    {
        Name = name;
        LocationAddress = locationAddress;
    }

    public void Update(string name, Address locationAddress)
    {
        Name = name;
        LocationAddress = locationAddress;
    }
}
