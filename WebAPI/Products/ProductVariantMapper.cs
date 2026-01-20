namespace EdgIQ.Inventory.WebAPI.Products;

public static class ProductVariantMapper
{
    extension(ProductVariant entity)
    {
        public ProductVariantResponse AsApiResponse()
        {
            ProductVariantResponse response = new()
            {
                Id = entity.Id,
                SKU = entity.SKU,
                Barcode = entity.Barcode,
                UnitPrice = entity.UnitPrice,
                Color = entity.Color,
                Size = entity.Size,
                ReorderPoint = entity.ReorderPoint,
                IsBatchTracked = entity.IsBatchTracked,
                IsSerialTracked = entity.IsSerialTracked,
                HasExpiryDate = entity.HasExpiryDate,
                ShelfLifeDays = entity.ShelfLifeDays,
                ExpiryAlertDays = entity.ExpiryAlertDays,
                CostingMethod = entity.CostingMethod,
                Product = entity.Product != null ?
                new ReferenceDataResponse<int>() { Id = entity.Product.Id, Name = entity.Product.Name } :
                default!,
                MeasurementUnit = entity.MeasurementUnit != null ?
                new ReferenceDataResponse<int>() { Id = entity.MeasurementUnit.Id, Name = entity.MeasurementUnit.Name } :
                default!
            };

            return response;
        }
    }

    extension(ProductVariantRequest request)
    {
        public ProductVariant ToEntity()
        {
            ProductVariant entity = new(request.ProductId, request.SKU, request.Barcode, request.UnitPrice, request.Color, 
                request.Size, request.ReorderPoint, request.IsBatchTracked, request.IsSerialTracked, request.HasExpiryDate, 
                request.ShelfLifeDays, request.ExpiryAlertDays, request.CostingMethod, request.MeasurementUnitId);
            return entity;
        }
    }

    extension(ProductVariant entity)
    {
        public void Update(ProductVariantRequest request)
        {
            entity.Update(request.SKU, request.Barcode, request.Color, request.Size, request.ReorderPoint, 
                request.IsBatchTracked, request.IsSerialTracked, request.HasExpiryDate, request.ShelfLifeDays, 
                request.ExpiryAlertDays, request.CostingMethod, request.MeasurementUnitId);
        }
    }
}


