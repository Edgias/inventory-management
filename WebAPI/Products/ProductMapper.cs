namespace EdgIQ.Inventory.WebAPI.Products;

public static class ProductMapper
{
    extension(Product entity)
    {
        public ProductResponse AsApiResponse()
        {
            ProductResponse response = new()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                ProductCategory = entity.ProductCategory != null ? 
                new ReferenceDataResponse<int>() { Id = entity.ProductCategory.Id, Name = entity.ProductCategory.Name } : 
                default!
            };

            return response;
        }
    }

    extension(ProductRequest request)
    {
        public Product ToEntity()
        {
            Product entity = new(request.Name, request.Description, request.ProductCategoryId);
            return entity;
        }
    }

    extension(Product entity)
    {
        public void Update(ProductRequest request)
        {
            entity.Update(request.Name, request.Description);
        }
    }
}


