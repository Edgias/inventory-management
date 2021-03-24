using System;

namespace Edgias.Inventory.Management.RESTAPI.Models.Responses
{
    public class ProductResponse 
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string ProductCode { get; set; }

        public string Description { get; set; }

        public Guid ProductCategoryId { get; set; }

        public string ProductCategory { get; set; }
    }
}
