namespace EdgIQ.Inventory.Domain.Entities;

public class MeasurementUnit : BaseEntity
{
    public string Name { get; private set; }

    public MeasurementUnit(string name)
    {
        Name = name;
    }

    public void Update(string name)
    {
        Name = name;
    }
}
