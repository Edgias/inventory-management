using Edgias.Inventory.Management.ApplicationCore.Entities;
using RESTAPI.Interfaces;
using RESTAPI.Models.Form;
using RESTAPI.Models.View;
using System;

namespace RESTAPI.Mappers
{
    public class ProductMapper : IMapper<Product, ProductFormApiModel, ProductApiModel>
    {
        public Product Map(ProductFormApiModel apiModel)
        {
            Product entity = new Product
            {
                CreatedBy = apiModel.UserId
            };

            Map(entity, apiModel);

            return entity;
        }

        public ProductApiModel Map(Product entity)
        {
            ProductApiModel apiModel = new ProductApiModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                CreatedDate = entity.CreatedDate,
                IsActive = entity.IsActive,
                LastModifiedDate = entity.LastModifiedDate,
                ProductCategory = entity.ProductCategory?.Name,
                ProductCategoryId = entity.ProductCategoryId,
                ProductCode = entity.ProductCode
            };

            return apiModel;
        }

        public void Map(Product entity, ProductFormApiModel apiModel)
        {
            entity.Name = apiModel.Name;
            entity.Description = apiModel.Description;
            entity.ProductCategoryId = apiModel.ProductCategoryId;
            entity.ProductCode = apiModel.ProductCode;
            entity.LastModifiedBy = apiModel.UserId;
            entity.LastModifiedDate = DateTimeOffset.Now;
        }
    }
}
