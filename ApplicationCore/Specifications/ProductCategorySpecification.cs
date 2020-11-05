using Edgias.Inventory.Management.ApplicationCore.Entities;

namespace Edgias.Inventory.Management.ApplicationCore.Specifications
{
    public class ProductCategorySpecification : BaseSpecification<ProductCategory>
    {
        public ProductCategorySpecification(string searchQuery)
            : base(pc => string.IsNullOrEmpty(searchQuery) || pc.Name.Contains(searchQuery) || pc.Description.Contains(searchQuery))
        {
        }

        public ProductCategorySpecification(int skip, int take, string searchQuery) 
            : base(pc => string.IsNullOrEmpty(searchQuery) || pc.Name.Contains(searchQuery) || pc.Description.Contains(searchQuery))
        {
            ApplyOrderBy(pc => pc.Name);
            ApplyPaging(skip, take);
        }
    }
}
