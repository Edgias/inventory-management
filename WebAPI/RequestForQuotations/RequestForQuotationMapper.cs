namespace EdgIQ.Inventory.WebAPI.RequestForQuotations;

public static class RequestForQuotationMapper
{
    public static RequestForQuotationResponse AsApiResponse(this RequestForQuotation entity)
    {
        RequestForQuotationResponse response = new()
        {
            Id = entity.Id,
            RFQNumber = entity.RFQNumber,
            RFQDate = entity.RFQDate,
            ResponseDeadline = entity.ResponseDeadline,
            Terms = entity.Terms,
            Description = entity.Description,
            Street = entity.DeliveryLocation?.Street,
            City = entity.DeliveryLocation?.City,
            State = entity.DeliveryLocation?.State,
            ZipCode = entity.DeliveryLocation?.ZipCode,
            Country = entity.DeliveryLocation?.Country,
            Status = entity.Status,
            PurchaseRequisition = entity.PurchaseRequisition != null ?
            new ReferenceDataResponse<int>() { Id = entity.PurchaseRequisition.Id, Name = entity.PurchaseRequisition.RequisitionNumber } : 
            default!,
            RequestForQuotationItems = entity.RequestForQuotationItems.Select(qi => qi.AsApiResponse())
        };

        return response;
    }

    public static RequestForQuotation ToEntity(this RequestForQuotationRequest request, string rfqNumber, string requesterId, List<ProductVariant> productVariants)
    {
        Address? address = null;
        if (!string.IsNullOrEmpty(request.Street))
        {
            address = new Address(request.Street, request.City, request.State, request.ZipCode, request.Country);
        }

        List<RequestForQuotationItem> requestForQuotationItems = [.. request.RequestForQuotationItems.Select(qi => qi.ToEntity(productVariants))];

        RequestForQuotation entity = new(rfqNumber, request.PurchaseRequisitionId, request.RFQDate.ToUtc(), request.ResponseDeadline.ToUtc(), 
            request.Description, request.Terms, address, requesterId, requestForQuotationItems);
        
        return entity;
    }

    public static void Update(this RequestForQuotation entity, RequestForQuotationRequest request)
    {
        Address? address = null;
        if (!string.IsNullOrEmpty(request.Street))
        {
            address = new Address(request.Street, request.City, request.State, request.ZipCode, request.Country);
        }

        entity.Update(request.PurchaseRequisitionId, request.RFQDate.ToUtc(), request.ResponseDeadline.ToUtc(), request.Description,
            request.Terms, address);
    }
}

