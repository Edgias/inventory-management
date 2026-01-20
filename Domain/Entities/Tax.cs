namespace EdgIQ.Inventory.Domain.Entities;

public class Tax : BaseEntity
{
    public string Name { get; private set; }

    public decimal Percentage { get; private set; }

    public Tax(string name, decimal percentage)
    {
        Name = name;
        Percentage = percentage;
    }

    public void Update(string name, decimal percentage)
    {
        Name = name;
        Percentage = percentage;
    }
}
