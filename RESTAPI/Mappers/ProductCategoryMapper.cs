using Edgias.Inventory.Management.ApplicationCore.Entities;
using RESTAPI.Interfaces;
using RESTAPI.Models.Form;
using RESTAPI.Models.View;
using System;

namespace RESTAPI.Mappers
{
    public class ProductCategoryMapper : IMapper<ProductCategory, ProductCategoryFormApiModel, ProductCategoryApiModel>
    {
        public ProductCategory Map(ProductCategoryFormApiModel apiModel)
        {
            ProductCategory entity = new ProductCategory
            {
                CreatedBy = apiModel.UserId
            };

            Map(entity, apiModel);

            return entity;
        }

        public ProductCategoryApiModel Map(ProductCategory entity)
        {
            ProductCategoryApiModel apiModel = new ProductCategoryApiModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                CreatedDate = entity.CreatedDate,
                IsActive = entity.IsActive,
                LastModifiedDate = entity.LastModifiedDate
            };

            return apiModel;
        }

        public void Map(ProductCategory entity, ProductCategoryFormApiModel apiModel)
        {
            entity.Name = apiModel.Name;
            entity.Description = apiModel.Description;
            entity.LastModifiedBy = apiModel.UserId;
            entity.LastModifiedDate = DateTimeOffset.Now;
        }
    }
}
