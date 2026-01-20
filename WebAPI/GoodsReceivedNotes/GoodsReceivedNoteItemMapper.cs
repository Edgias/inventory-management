namespace EdgIQ.Inventory.WebAPI.GoodsReceivedNotes;

public static class GoodsReceivedNoteItemMapper
{
    public static GoodsReceivedNoteItemResponse AsApiResponse(this GoodsReceivedNoteItem entity)
    {
        GoodsReceivedNoteItemResponse response = new()
        {
            Id = entity.Id,
            QuantityReceived = entity.QuantityReceived,
            QuantityOrdered = entity.QuantityOrdered,
            PurchaseOrderItemId = entity.PurchaseOrderItemId,
            Bin = entity.Bin != null ?
            new ReferenceDataResponse<int>() { Id = entity.Bin.Id, Name = entity.Bin.Name } : null!,
            Product = entity.ReceivedProduct != null ? 
            new ReferenceDataResponse<int>() { Id = entity.ReceivedProduct.ProductVariantId, Name = entity.ReceivedProduct.ProductName } : null!,
            BatchNumber = entity.BatchNumber,
            ExpiryDate = entity.ExpiryDate,
            Notes = entity.Notes
        };

        return response;
    }

    public static GoodsReceivedNoteItem ToEntity(this GoodsReceivedNoteItemRequest request, List<ProductVariant> productVariants)
    {
        ProductVariant? productVariant = productVariants.FirstOrDefault(p => p.Id == request.ProductVariantId) ??
                throw new ArgumentException("Product not found");

        ReceivedProduct receivedProduct = new(request.ProductVariantId, productVariant.Product.Name);

        GoodsReceivedNoteItem entity = new(receivedProduct, request.QuantityOrdered, request.QuantityReceived, request.PurchaseOrderItemId,
            request.BinId, request.BatchNumber, request.ExpiryDate.ToUtc(), request.Notes);
        
        return entity;
    }

    public static GoodsReceivedNoteItem ToEntity(this GoodsReceivedNoteItemRequest request, ProductVariant productVariant)
    {
        ReceivedProduct receivedProduct = new(productVariant.Id, productVariant.Product.Name);

        GoodsReceivedNoteItem entity = new(receivedProduct, request.QuantityOrdered, request.QuantityReceived, request.PurchaseOrderItemId,
            request.BinId, request.BatchNumber, request.ExpiryDate.ToUtc(), request.Notes);

        return entity;
    }

    public static void Update(this GoodsReceivedNoteItem entity, GoodsReceivedNoteItemRequest request, ProductVariant productVariant)
    {
        ReceivedProduct receivedProduct = new(request.ProductVariantId, productVariant.Product.Name);

        entity.Update(receivedProduct, request.QuantityOrdered, request.QuantityReceived, request.PurchaseOrderItemId,
            request.BinId, request.BatchNumber, request.ExpiryDate.ToUtc(), request.Notes);
    }
}
