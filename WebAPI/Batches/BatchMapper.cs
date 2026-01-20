namespace EdgIQ.Inventory.WebAPI.Batches;

public static class BatchMapper
{
    extension(Batch entity)
    {
        public BatchResponse AsApiResponse()
        {
            BatchResponse response = new()
            {
                Id = entity.Id,
                SerialNumber = entity.SerialNumber,
                ManufactureDate = entity.ManufactureDate,
                ExpiryDate = entity.ExpiryDate,
                ReceivedDate = entity.ReceivedDate,
                InitialQuantity = entity.InitialQuantity,
                UnitPrice = entity.UnitPrice,
                QualityCertificate = entity.QualityCertificate,
                Notes = entity.Notes,
                Status = entity.Status,
                Product = entity.ProductVariant != null ?
                new ReferenceDataResponse<int>() { Id = entity.ProductVariant.Id, Name = entity.ProductVariant.Product.Name } :
                default!,
                Supplier = entity.Supplier != null ?
                new ReferenceDataResponse<int>() { Id = entity.Supplier.Id, Name = entity.Supplier.Name } :
                default!
            };

            return response;
        }
    }

    extension(BatchRequest request)
    {
        public Batch ToEntity()
        {
            Batch entity = new(request.ProductVariantId, request.SupplierId, request.SerialNumber, request.ManufactureDate, 
                request.ExpiryDate, request.ReceivedDate, request.InitialQuantity, request.UnitPrice, request.QualityCertificate, 
                request.Notes);

            return entity;
        }
    }

    extension(Batch entity)
    {
        public void Update(BatchRequest request)
        {
            entity.Update(request.ProductVariantId, request.SupplierId, request.SerialNumber, request.ManufactureDate,
                request.ExpiryDate, request.ReceivedDate, request.InitialQuantity, request.UnitPrice, request.QualityCertificate,
                request.Notes);
        }
    }
}


