using Edgias.Inventory.Management.ApplicationCore.Entities;
using Edgias.Inventory.Management.RESTAPI.Interfaces;
using Edgias.Inventory.Management.RESTAPI.Models.Requests;
using Edgias.Inventory.Management.RESTAPI.Models.Responses;

namespace Edgias.Inventory.Management.RESTAPI.Mappers
{
    public class ProductMapper : IMapper<Product, ProductRequest, ProductResponse>
    {
        public Product Map(ProductRequest request)
        {
            Product entity = new(request.Name, request.ProductCode, request.Description, request.ProductCategoryId);

            return entity;
        }

        public ProductResponse Map(Product entity)
        {
            ProductResponse response = new()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                ProductCategory = entity.ProductCategory?.Name,
                ProductCategoryId = entity.ProductCategoryId,
                ProductCode = entity.ProductCode
            };

            return response;
        }

        public void Map(Product entity, ProductRequest request)
        {
            entity.UpdateDetails(request.Name, request.ProductCode, request.Description);
        }
    }
}
