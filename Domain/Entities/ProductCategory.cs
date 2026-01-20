namespace EdgIQ.Inventory.Domain.Entities;

public class ProductCategory : BaseEntity
{
    public string Name { get; private set; } = default!;

    public string? Description { get; private set; }

    private ProductCategory()
    {
    }

    public ProductCategory(string name, string? description)
    {
        Name = name;
        Description = description;
    }

    public void Update(string name, string? description)
    {
        Name = name;
        Description = description;
    }
}
