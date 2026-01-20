namespace EdgIQ.Inventory.WebAPI.PurchaseRequisitions;
public static class PurchaseRequisitionItemMapper
{
    public static PurchaseRequisitionItemResponse AsApiResponse(this PurchaseRequisitionItem entity)
    {
        PurchaseRequisitionItemResponse response = new()
        {
            Id = entity.Id,
            Product = entity.RequisitionedProduct != null ? 
            new ReferenceDataResponse<int>() { Id = entity.RequisitionedProduct.ProductVariantId, Name = entity.RequisitionedProduct.ProductName } : null!,
            UnitPrice = entity.UnitPrice,
            Units = entity.Units,
            Notes = entity.Notes,
            Status = entity.Status
        };

        return response;
    }

    public static PurchaseRequisitionItem ToEntity(this PurchaseRequisitionItemRequest request, List<ProductVariant> productVariants)
    {
        ProductVariant? productVariant = productVariants.FirstOrDefault(p => p.Id == request.ProductVariantId) ??
                throw new ArgumentException("Product not found");

        RequisitionedProduct requisitionProduct = new(request.ProductVariantId, productVariant.Product.Name);

        PurchaseRequisitionItem entity = new(requisitionProduct, request.UnitPrice, request.Units);
        
        return entity;
    }

    public static PurchaseRequisitionItem ToEntity(this PurchaseRequisitionItemRequest request, ProductVariant productVariant)
    {
        RequisitionedProduct requisitionProduct = new(productVariant.Id, productVariant.Product.Name);
        PurchaseRequisitionItem entity = new(requisitionProduct, request.UnitPrice, request.Units);
        
        return entity;
    }

    public static void Update(this PurchaseRequisitionItem entity, PurchaseRequisitionItemRequest request, ProductVariant productVariant)
    {
        RequisitionedProduct requisitionedProduct = new(request.ProductVariantId, productVariant.Product.Name);
        entity.Update(requisitionedProduct, request.UnitPrice, request.Units);
    }
}
