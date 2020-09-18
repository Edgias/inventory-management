using System;

namespace Edgias.Inventory.Management.ApplicationCore.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }

        public string ProductCode { get; set; }

        public string Description { get; set; }

        public Guid ProductCategoryId { get; set; }

        public ProductCategory ProductCategory { get; set; }
    }
}
