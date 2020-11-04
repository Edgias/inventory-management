using Edgias.Inventory.Management.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Edgias.Inventory.Management.ApplicationCore.Specifications
{
    public class ProductCategorySpecification : BaseSpecification<ProductCategory>
    {
        public ProductCategorySpecification(Expression<Func<ProductCategory, bool>> criteria) 
            : base(criteria)
        {
        }
    }
}
