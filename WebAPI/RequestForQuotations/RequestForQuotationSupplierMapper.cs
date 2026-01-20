namespace EdgIQ.Inventory.WebAPI.RequestForQuotations;

public static class RequestForQuotationSupplierMapper
{
    extension(RequestForQuotationSupplier entity)
    {
        public RequestForQuotationSupplierResponse AsApiResponse()
        {
            RequestForQuotationSupplierResponse response = new()
            {
                Id = entity.Id,
                Name = entity.Supplier.Name,
                Email = entity.Supplier.Email,
                Phone = entity.Supplier.Phone,
                DateSent = entity.DateSent,
                DateResponded = entity.DateResponded
            };

            return response;
        }
    }

    extension(RequestForQuotationSupplierRequest request)
    {
        public RequestForQuotationSupplier ToEntity()
        {
            RequestForQuotationSupplier entity = new(request.SupplierId, request.DateSent.ToUtc(), request.DateResponded.ToUtc(), request.Notes);

            return entity;
        }
    }

    extension(RequestForQuotationSupplier entity)
    {
        public void Update(RequestForQuotationSupplierRequest request)
        {
            entity.Update(request.SupplierId, request.DateSent.ToUtc(), request.DateResponded.ToUtc(), request.Notes);
        }
    }
}


