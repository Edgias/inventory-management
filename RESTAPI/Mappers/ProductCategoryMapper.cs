using Edgias.Inventory.Management.ApplicationCore.Entities;
using Edgias.Inventory.Management.RESTAPI.Interfaces;
using Edgias.Inventory.Management.RESTAPI.Models.Requests;
using Edgias.Inventory.Management.RESTAPI.Models.Responses;

namespace Edgias.Inventory.Management.RESTAPI.Mappers
{
    public class ProductCategoryMapper : IMapper<ProductCategory, ProductCategoryRequest, ProductCategoryResponse>
    {
        public ProductCategory Map(ProductCategoryRequest request)
        {
            ProductCategory entity = new(request.Name, request.Description);

            return entity;
        }

        public ProductCategoryResponse Map(ProductCategory entity)
        {
            ProductCategoryResponse response = new()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            };

            return response;
        }

        public void Map(ProductCategory entity, ProductCategoryRequest request)
        {
            entity.UpdateDetails(request.Name, request.Description);
        }
    }
}
