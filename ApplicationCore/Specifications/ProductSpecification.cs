using Edgias.Inventory.Management.ApplicationCore.Entities;
using System;

namespace Edgias.Inventory.Management.ApplicationCore.Specifications
{
    public class ProductSpecification : BaseSpecification<Product>
    {
        public ProductSpecification(string searchQuery)
            : base(p => string.IsNullOrEmpty(searchQuery) ||
            p.Name.Contains(searchQuery) ||
            p.Description.Contains(searchQuery) ||
            p.ProductCode.Contains(searchQuery))
        {
        }

        public ProductSpecification(Guid productCategoryId)
            : base(p => p.ProductCategoryId == productCategoryId)
        {
            AddInclude(p => p.ProductCategory);
        }

        public ProductSpecification(int skip, int take, string searchQuery) 
            : base(p => string.IsNullOrEmpty(searchQuery) || 
            p.Name.Contains(searchQuery) || 
            p.Description.Contains(searchQuery) || 
            p.ProductCode.Contains(searchQuery))
        {
            AddInclude(p => p.ProductCategory);
            ApplyOrderBy(p => p.Name);
            ApplyPaging(skip, take);
        }

        public ProductSpecification(Guid productCategoryId, string searchQuery)
            : base(p => p.ProductCategoryId == productCategoryId && string.IsNullOrEmpty(searchQuery) ||
            p.Name.Contains(searchQuery) ||
            p.Description.Contains(searchQuery) ||
            p.ProductCode.Contains(searchQuery))
        {
        }

        public ProductSpecification(Guid productCategoryId, int skip, int take, string searchQuery)
            : base(p => p.ProductCategoryId == productCategoryId && string.IsNullOrEmpty(searchQuery) ||
            p.Name.Contains(searchQuery) ||
            p.Description.Contains(searchQuery) ||
            p.ProductCode.Contains(searchQuery))
        {
            ApplyOrderBy(p => p.Name);
            ApplyPaging(skip, take);
        }
    }
}
