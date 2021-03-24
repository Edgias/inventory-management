using System;

namespace Edgias.Inventory.Management.RESTAPI.Models.Requests
{
    public class ProductRequest 
    {
        public string Name { get; set; }

        public string ProductCode { get; set; }

        public string Description { get; set; }

        public Guid ProductCategoryId { get; set; }
    }
}
