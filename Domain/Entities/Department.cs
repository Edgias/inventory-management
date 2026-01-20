namespace EdgIQ.Inventory.Domain.Entities;

public class Department : BaseEntity
{
    public string Name { get; private set; }

    public Department(string name)
    {
        Name = name;
    }

    public void Update(string name)
    {
        Name = name;
    }
}
