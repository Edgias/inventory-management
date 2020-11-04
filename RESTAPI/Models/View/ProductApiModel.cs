using System;

namespace RESTAPI.Models.View
{
    public class ProductApiModel : BaseApiModel
    {
        public string Name { get; set; }

        public string ProductCode { get; set; }

        public string Description { get; set; }

        public Guid ProductCategoryId { get; set; }

        public string ProductCategory { get; set; }
    }
}
