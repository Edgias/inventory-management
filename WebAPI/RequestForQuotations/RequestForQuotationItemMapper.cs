namespace EdgIQ.Inventory.WebAPI.RequestForQuotations;

public static class RequestForQuotationItemMapper
{
    public static RequestForQuotationItemResponse AsApiResponse(this RequestForQuotationItem entity)
    {
        RequestForQuotationItemResponse response = new()
        {
            Id = entity.Id,
            Product = entity.RequestedProduct != null ? 
            new ReferenceDataResponse<int>() { Id = entity.RequestedProduct.ProductVariantId, Name = entity.RequestedProduct.ProductName } : null!,
            QuantityRequired = entity.QuantityRequired,
            DateRequired = entity.DateRequired,
            Specifications = entity.Specifications,
            Notes = entity.Notes
        };

        return response;
    }

    public static RequestForQuotationItem ToEntity(this RequestForQuotationItemRequest request, List<ProductVariant> productVariants)
    {
        ProductVariant? productVariant = productVariants.FirstOrDefault(p => p.Id == request.ProductVariantId) ??
                throw new ArgumentException("Product not found");

        RequestedProduct requestedProduct = new(request.ProductVariantId, productVariant.Product.Name);

        RequestForQuotationItem entity = new(requestedProduct, request.QuantityRequired, request.Specifications, request.DateRequired, request.Notes);
        
        return entity;
    }

    public static RequestForQuotationItem ToEntity(this RequestForQuotationItemRequest request, ProductVariant productVariant)
    {
        RequestedProduct requestedProduct = new(productVariant.Id, productVariant.Product.Name);
        RequestForQuotationItem entity = new(requestedProduct, request.QuantityRequired, request.Specifications, request.DateRequired, request.Notes);
        
        return entity;
    }

    public static void Update(this RequestForQuotationItem entity, RequestForQuotationItemRequest request, ProductVariant productVariant)
    {
        RequestedProduct requestedProduct = new(request.ProductVariantId, productVariant.Product.Name);
        entity.Update(requestedProduct, request.QuantityRequired, request.Specifications, request.DateRequired, request.Notes);
    }
}
