namespace EdgIQ.Inventory.WebAPI.StockTransactions;

public static class StockTransactionMapper
{
    extension(StockTransaction entity)
    {
        public StockTransactionResponse AsApiResponse()
        {
            StockTransactionResponse response = new()
            {
                Id = entity.Id,
                TransactionNumber = entity.TransactionNumber,
                TransactionDate = entity.TransactionDate,
                Product = entity.ProductVariant != null ?
                new ReferenceDataResponse<int>() { Id = entity.ProductVariant.Id, Name = entity.ProductVariant.Product.Name } :
                default!,
                Batch = entity.Batch != null ?
                new ReferenceDataResponse<int>() { Id = entity.Batch.Id, Name = entity.Batch.BatchNumber } :
                default!,
                SerialNumber = entity.SerialNumber != null ?
                new ReferenceDataResponse<int>() { Id = entity.SerialNumber.Id, Name = entity.SerialNumber.SerialNo } :
                default!,
                Warehouse = entity.Warehouse != null ?
                new ReferenceDataResponse<int>() { Id = entity.Warehouse.Id, Name = entity.Warehouse.Name } :
                default!,
                Bin = entity.Bin != null ?
                new ReferenceDataResponse<int>() { Id = entity.Bin.Id, Name = entity.Bin.Name } :
                default!,
                Quantity = entity.Quantity,
                UnitPrice = entity.UnitPrice,
                TransactionType = entity.Type,
                DocumentType = entity.DocumentType,
                DocumentId = entity.DocumentId,
                Notes = entity.Notes
            };

            return response;
        }
    }
}
