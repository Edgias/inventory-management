namespace EdgIQ.Inventory.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; private set; }

    public string ProductCode { get; private set; }

    public string Description { get; private set; }

    public int ProductCategoryId { get; private set; }

    public ProductCategory ProductCategory { get; private set; }

    private Product()
    {
        // Required by EF
    }

    public Product(string name, string productCode, string description, int productCategoryId)
    {
        Name = name;
        ProductCode = productCode;
        Description = description;
        ProductCategoryId = productCategoryId;
    }

    public void Update(string name, string productCode, string description)
    {
        Name = name;
        ProductCode = productCode;
        Description = description;
    }

    public void ChangeCategory(int productCategoryId)
    {
        ProductCategoryId = productCategoryId;
    }
}
