namespace EdgIQ.Inventory.WebAPI.PurchaseOrders;

public static class PurchaseOrderItemMapper
{
    public static PurchaseOrderItemResponse AsApiResponse(this PurchaseOrderItem entity)
    {
        PurchaseOrderItemResponse response = new()
        {
            Id = entity.Id,
            Product = entity.OrderedProduct != null ? 
            new ReferenceDataResponse<int>() { Id = entity.OrderedProduct.ProductVariantId, Name = entity.OrderedProduct.ProductName } : null!,
            UnitPrice = entity.UnitPrice,
            Units = entity.Units,
            Status = entity.Status
        };

        return response;
    }

    public static PurchaseOrderItem ToEntity(this PurchaseOrderItemRequest request, List<ProductVariant> productVariants)
    {
        ProductVariant? productVariant = productVariants.FirstOrDefault(p => p.Id == request.ProductVariantId) ??
                throw new ArgumentException("Product not found");

        OrderedProduct orderedProduct = new(request.ProductVariantId, productVariant.Product.Name);

        PurchaseOrderItem entity = new(orderedProduct, request.UnitPrice, request.Units);
        
        return entity;
    }

    public static PurchaseOrderItem ToEntity(this PurchaseOrderItemRequest request, ProductVariant productVariant)
    {
        OrderedProduct orderedProduct = new(productVariant.Id, productVariant.Product.Name);
        PurchaseOrderItem entity = new(orderedProduct, request.UnitPrice, request.Units);
        
        return entity;
    }

    public static void Update(this PurchaseOrderItem entity, PurchaseOrderItemRequest request, ProductVariant productVariant)
    {
        OrderedProduct orderedProduct = new(request.ProductVariantId, productVariant.Product.Name);
        entity.Update(orderedProduct, request.UnitPrice, request.Units);
    }
}
