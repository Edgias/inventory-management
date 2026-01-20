namespace EdgIQ.Inventory.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; private set; } = default!;

    public string? Description { get; private set; }

    public int? ProductCategoryId { get; private set; }

    public ProductCategory? ProductCategory { get; private set; }

    private readonly List<ProductVariant> _variants = [];
    public IReadOnlyCollection<ProductVariant> Variants => _variants.AsReadOnly();

    private Product()
    {
        // Required by EF
    }

    public Product(string name, string? description)
    {
        Name = name;
        Description = description;
    }

    public Product(string name, string? description, int? productCategoryId)
    {
        Name = name;
        Description = description;
        ProductCategoryId = productCategoryId;
    }

    public Product(string name, string? description, int? productCategoryId, List<ProductVariant> variants)
    {
        Name = name;
        Description = description;
        ProductCategoryId = productCategoryId;
        _variants = variants;
    }

    public void AddVariant(ProductVariant variant)
    {
        _variants.Add(variant); 
    }

    public void RemoveVariant(ProductVariant variant)
    {
        _variants.Remove(variant);
    }

    public void Update(string name, string? description)
    {
        Name = name;
        Description = description;
    }

    public void ChangeCategory(int productCategoryId)
    {
        ProductCategoryId = productCategoryId;
    }
}
