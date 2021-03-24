using System;

namespace Edgias.Inventory.Management.ApplicationCore.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; private set; }

        public string ProductCode { get; private set; }

        public string Description { get; private set; }

        public Guid ProductCategoryId { get; private set; }

        public ProductCategory ProductCategory { get; private set; }

        private Product()
        {
            // Required by EF
        }

        public Product(string name, string productCode, string description, Guid productCategoryId)
        {
            Name = name;
            ProductCode = productCode;
            Description = description;
            ProductCategoryId = productCategoryId;
        }

        public void UpdateDetails(string name, string productCode, string description)
        {
            Name = name;
            ProductCode = productCode;
            Description = description;
            LastModifiedDate = DateTimeOffset.UtcNow;
        }

        public void ChangeCategory(Guid productCategoryId)
        {
            ProductCategoryId = productCategoryId;
            LastModifiedDate = DateTimeOffset.UtcNow;
        }
    }
}
