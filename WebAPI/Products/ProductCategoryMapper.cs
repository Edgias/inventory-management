namespace EdgIQ.Inventory.WebAPI.Products;

public static class ProductCategoryMapper
{
    extension(ProductCategory entity)
    {
        public ProductCategoryResponse AsApiResponse()
        {
            ProductCategoryResponse response = new()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            };

            return response;
        }
    }

    extension(ProductCategoryRequest request)
    {
        public ProductCategory ToEntity()
        {
            ProductCategory entity = new(request.Name, request.Description);
            return entity;
        }
    }

    extension(ProductCategory entity)
    {
        public void Update(ProductCategoryRequest request)
        {
            entity.Update(request.Name, request.Description);
        }
    }
}


