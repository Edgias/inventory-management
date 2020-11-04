using System;

namespace RESTAPI.Models.Form
{
    public class ProductFormApiModel : BaseFormApiModel
    {
        public string Name { get; set; }

        public string ProductCode { get; set; }

        public string Description { get; set; }

        public Guid ProductCategoryId { get; set; }
    }
}
