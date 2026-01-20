namespace EdgIQ.Inventory.WebAPI.StockTransfers;

public static class StockTransferItemMapper
{
    extension(StockTransferItem entity)
    {
        public StockTransferItemResponse AsApiResponse()
        {
            StockTransferItemResponse response = new()
            {
                Id = entity.Id,
                QuantityReceived = entity.QuantityReceived,
                QuantityRequested = entity.QuantityRequested,
                QuantityShipped = entity.QuantityShipped,
                Notes = entity.Notes,
                FromBin = entity.FromBin != null ?
                new ReferenceDataResponse<int>() { Id = entity.FromBin.Id, Name = entity.FromBin.Name } :
                default!,
                ToBin = entity.ToBin != null ?
                new ReferenceDataResponse<int>() { Id = entity.ToBin.Id, Name = entity.ToBin.Name } :
                default!
            };

            return response;
        }
    }

    extension(StockTransferItemRequest request)
    {
        public StockTransferItem ToEntity(List<ProductVariant> productVariants)
        {
            ProductVariant? productVariant = productVariants.FirstOrDefault(p => p.Id == request.ProductVariantId) ??
                throw new ArgumentException("Product not found");

            TransferredProduct transferredProduct = new(productVariant.Id, productVariant.Product.Name);

            StockTransferItem entity = new(transferredProduct, request.QuantityRequested, request.QuantityShipped, 
                request.QuantityReceived, request.FromBinId, request.ToBinId, request.Notes);
            
            return entity;
        }
    }

    extension(StockTransferItemRequest request)
    {
        public StockTransferItem ToEntity(ProductVariant productVariant)
        {
            TransferredProduct transferredProduct = new(productVariant.Id, productVariant.Product.Name);

            StockTransferItem entity = new(transferredProduct, request.QuantityRequested, request.QuantityShipped,
                request.QuantityReceived, request.FromBinId, request.ToBinId, request.Notes);

            return entity;
        }
    }

    extension(StockTransferItem entity)
    {
        public void Update(StockTransferItemRequest request, ProductVariant productVariant)
        {
            TransferredProduct transferredProduct = new(productVariant.Id, productVariant.Product.Name);

            entity.Update(transferredProduct, request.QuantityRequested, request.QuantityShipped,
                request.QuantityReceived, request.FromBinId, request.ToBinId, request.Notes);
        }
    }
}
