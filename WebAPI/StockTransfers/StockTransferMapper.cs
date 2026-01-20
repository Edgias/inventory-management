namespace EdgIQ.Inventory.WebAPI.StockTransfers;

public static class StockTransferMapper
{
    extension(StockTransfer entity)
    {
        public StockTransferResponse AsApiResponse()
        {
            StockTransferResponse response = new()
            {
                Id = entity.Id,
                TransferNumber = entity.TransferNumber,
                TransferDate = entity.TransferDate,
                ShipmentDate = entity.ShipmentDate,
                ReceivedDate = entity.ReceivedDate,
                Approver = !string.IsNullOrEmpty(entity.ApproverId) ? 
                new ReferenceDataResponse<string>() { Id = entity.ApproverId } :
                default!,
                Notes = entity.Notes,
                FromWarehouse = entity.FromWarehouse != null ?
                new ReferenceDataResponse<int>() { Id = entity.FromWarehouse.Id, Name = entity.FromWarehouse.Name } :
                default!,
                ToWarehouse = entity.ToWarehouse != null ?
                new ReferenceDataResponse<int>() { Id = entity.ToWarehouse.Id, Name = entity.ToWarehouse.Name } :
                default!,
                StockTransferItems = entity.StockTransferItems.Select(sai => sai.AsApiResponse())
            };

            return response;
        }
    }

    extension(StockTransferRequest request)
    {
        public StockTransfer ToEntity(string transferNumber, string userId, List<ProductVariant> productVariants)
        {
            List<StockTransferItem> stockTransferItems = [.. request.StockTransferItems.Select(sai => sai.ToEntity(productVariants))];

            StockTransfer entity = new(transferNumber, request.FromWarehouseId, request.ToWarehouseId, request.TransferDate, request.ShipmentDate,
                request.ReceivedDate, userId, request.Notes, stockTransferItems);

           return entity;
        }
    }

    extension(StockTransfer entity)
    {
        public void Update(StockTransferRequest request)
        {
            entity.Update(request.FromWarehouseId, request.ToWarehouseId, request.TransferDate, request.ShipmentDate,
                request.ReceivedDate, request.Notes);
        }
    }
}
