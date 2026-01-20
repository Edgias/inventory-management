namespace EdgIQ.Inventory.WebAPI.Quotations;

public static class QuotationMapper
{
    public static QuotationResponse AsApiResponse(this Quotation entity)
    {
        QuotationResponse response = new()
        {
            Id = entity.Id,
            QuotationNumber = entity.QuotationNumber,
            ExpiryDate = entity.ExpiryDate,
            QuotationDate = entity.QuotationDate,
            Terms = entity.Terms,
            TotalAmount = entity.GetTotal(),
            Supplier = entity.Supplier != null ?
            new ReferenceDataResponse<int>() { Id = entity.Supplier.Id, Name = entity.Supplier.Name } :
            default!,
            PurchaseRequisition = entity.PurchaseRequisition != null ?
            new ReferenceDataResponse<int>() { Id = entity.PurchaseRequisition.Id, Name = entity.PurchaseRequisition.RequisitionNumber } : 
            default!,
            QuotationItems = entity.QuotationItems.Select(qi => qi.AsApiResponse()),
            RequestForQuotation = entity.RequestForQuotation != null ?
                new ReferenceDataResponse<int>() { Id = entity.RequestForQuotation.Id, Name = entity.RequestForQuotation.RFQNumber } :
                default!
        };

        return response;
    }

    public static Quotation ToEntity(this QuotationRequest request, string quotationNumber, List<ProductVariant> productVariants)
    {
        List<QuotationItem> quotationItems = [.. request.QuotationItems.Select(qi => qi.ToEntity(productVariants))];

        Quotation entity = new(quotationNumber, request.PurchaseRequisitionId, request.QuotationDate.ToUtc(), request.ExpiryDate.ToUtc(), 
            request.SupplierId, request.RequestForQuotationId, request.Terms, quotationItems);
        
        return entity;
    }

    public static void Update(this Quotation entity, QuotationRequest request)
    {
        entity.Update(request.QuotationDate.ToUtc(), request.ExpiryDate.ToUtc(), request.SupplierId,
            request.RequestForQuotationId, request.Terms);
    }
}

