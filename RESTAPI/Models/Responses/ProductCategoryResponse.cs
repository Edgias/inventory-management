using System;

namespace Edgias.Inventory.Management.RESTAPI.Models.Responses
{
    public class ProductCategoryResponse 
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
