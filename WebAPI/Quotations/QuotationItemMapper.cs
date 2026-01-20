namespace EdgIQ.Inventory.WebAPI.Quotations;

public static class QuotationItemMapper
{
    public static QuotationItemResponse AsApiResponse(this QuotationItem entity)
    {
        QuotationItemResponse response = new()
        {
            Id = entity.Id,
            Product = entity.QuotedProduct != null ? 
            new ReferenceDataResponse<int>() { Id = entity.QuotedProduct.ProductVariantId, Name = entity.QuotedProduct.ProductName } : null!,
            UnitPrice = entity.UnitPrice,
            Units = entity.Units
        };

        return response;
    }

    public static QuotationItem ToEntity(this QuotationItemRequest request, List<ProductVariant> productVariants)
    {
        ProductVariant? productVariant = productVariants.FirstOrDefault(p => p.Id == request.ProductVariantId) ??
                throw new ArgumentException("Product not found");

        QuotedProduct quotedProduct = new(request.ProductVariantId, productVariant.Product.Name);

        QuotationItem entity = new(quotedProduct, request.UnitPrice, request.Units, request.TaxId);
        
        return entity;
    }

    public static QuotationItem ToEntity(this QuotationItemRequest request, ProductVariant productVariant)
    {
        QuotedProduct quotedProduct = new(productVariant.Id, productVariant.Product.Name);
        QuotationItem entity = new(quotedProduct, request.UnitPrice, request.Units, request.TaxId);
        
        return entity;
    }

    public static void Update(this QuotationItem entity, QuotationItemRequest request, ProductVariant productVariant)
    {
        QuotedProduct quotedProduct = new(request.ProductVariantId, productVariant.Product.Name);
        entity.Update(quotedProduct, request.UnitPrice, request.Units, request.TaxId);
    }
}
