using System;

namespace Edgias.Inventory.Management.ApplicationCore.Entities
{
    public class ProductCategory : BaseEntity
    {
        public string Name { get; private set; }

        public string Description { get; private set; }

        private ProductCategory()
        {
        }

        public ProductCategory(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public void UpdateDetails(string name, string description)
        {
            Name = name;
            Description = description;
            LastModifiedDate = DateTimeOffset.UtcNow;
        }
    }
}
