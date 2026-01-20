namespace EdgIQ.Inventory.WebAPI.SerialNumbers;

public static class SerialNumberMapper
{
    extension(SerialNumber entity)
    {
        public SerialNumberResponse AsApiResponse()
        {
            SerialNumberResponse response = new()
            {
                Id = entity.Id,
                SerialNo = entity.SerialNo,
                PurchaseDate = entity.PurchaseDate,
                SaleDate = entity.SaleDate,
                PurchaseCost = entity.PurchaseCost,
                WarrantyInformation = entity.WarrantyInformation,
                Notes = entity.Notes,
                Status = entity.Status,
                Product = entity.ProductVariant != null ?
                new ReferenceDataResponse<int>() { Id = entity.ProductVariant.Id, Name = entity.ProductVariant.Product.Name } :
                default!,
                Batch = entity.Batch != null ?
                new ReferenceDataResponse<int>() { Id = entity.Batch.Id, Name = entity.Batch.BatchNumber } :
                default!,
                Warehouse = entity.Warehouse != null ?
                new ReferenceDataResponse<int>() { Id = entity.Warehouse.Id, Name = entity.Warehouse.Name } :
                default!,
                Bin = entity.Bin != null ?
                new ReferenceDataResponse<int>() { Id = entity.Bin.Id, Name = entity.Bin.Name } :
                default!
            };

            return response;
        }
    }

    extension(SerialNumberRequest request)
    {
        public SerialNumber ToEntity()
        {
            SerialNumber entity = new(request.SerialNo, request.ProductVariantId, request.BatchId, request.WarehouseId,
                request.BinId, request.PurchaseDate, request.SaleDate, request.PurchaseCost, request.WarrantyInformation,
                request.Notes);

            return entity;
        }
    }

    extension(SerialNumber entity)
    {
        public void Update(SerialNumberRequest request)
        {
            entity.Update(request.ProductVariantId, request.BatchId, request.WarehouseId, request.BinId, request.PurchaseDate, 
                request.SaleDate, request.PurchaseCost, request.WarrantyInformation, request.Notes);
        }
    }
}


