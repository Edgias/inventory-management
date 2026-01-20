namespace EdgIQ.Inventory.WebAPI.StockAdjustments;

public static class StockAdjustmentItemMapper
{
    extension(StockAdjustmentItem entity)
    {
        public StockAdjustmentItemResponse AsApiResponse()
        {
            StockAdjustmentItemResponse response = new()
            {
                Id = entity.Id,
                ActualQuantity = entity.ActualQuantity,
                SystemQuantity = entity.SystemQuantity,
                Notes = entity.Notes,
                UnitPrice = entity.UnitPrice,
                Product = entity.AdjustedStock != null ?
                new ReferenceDataResponse<int>() { Id = entity.AdjustedStock.ProductVariantId, Name = entity.AdjustedStock.ProductName } :
                default!,
                Batch = entity.Batch != null ?
                new ReferenceDataResponse<int>() { Id = entity.Batch.Id, Name = entity.Batch.BatchNumber } :
                default!,
                Bin = entity.Bin != null ?
                new ReferenceDataResponse<int>() { Id = entity.Bin.Id, Name = entity.Bin.Name } :
                default!,
                SerialNumber = entity.SerialNumber != null ?
                new ReferenceDataResponse<int>() { Id = entity.SerialNumber.Id, Name = entity.SerialNumber.SerialNo } :
                default!
            };

            return response;
        }
    }

    extension(StockAdjustmentItemRequest request)
    {
        public StockAdjustmentItem ToEntity(List<ProductVariant> productVariants)
        {
            ProductVariant? productVariant = productVariants.FirstOrDefault(p => p.Id == request.ProductVariantId) ??
                throw new ArgumentException("Product not found");

            AdjustedStock adjustedStock = new(request.ProductVariantId, productVariant.Product.Name);

            StockAdjustmentItem entity = new(adjustedStock, request.BatchId, request.SerialNumberId, 
                request.BinId, request.SystemQuantity, request.ActualQuantity, request.UnitPrice, request.Notes);
            
            return entity;
        }
    }

    extension(StockAdjustmentItemRequest request)
    {
        public StockAdjustmentItem ToEntity(ProductVariant productVariant)
        {
            AdjustedStock adjustedStock = new(request.ProductVariantId, productVariant.Product.Name);

            StockAdjustmentItem entity = new(adjustedStock, request.BatchId, request.SerialNumberId,
                request.BinId, request.SystemQuantity, request.ActualQuantity, request.UnitPrice, request.Notes);

            return entity;
        }
    }

    extension(StockAdjustmentItem entity)
    {
        public void Update(StockAdjustmentItemRequest request, ProductVariant productVariant)
        {
            AdjustedStock adjustedStock = new(request.ProductVariantId, productVariant.Product.Name);

            entity.Update(adjustedStock, request.BatchId, request.SerialNumberId,
                request.BinId, request.SystemQuantity, request.ActualQuantity, request.UnitPrice, request.Notes);
        }
    }
}
