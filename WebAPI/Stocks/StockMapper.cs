namespace EdgIQ.Inventory.WebAPI.Stocks;

public static class StockMapper
{
    extension(WarehouseStock entity)
    {
        public StockResponse AsApiResponse()
        {
            StockResponse response = new()
            {
                Id = entity.Id,
                QuantityOnHand = entity.QuantityOnHand,
                QuantityReserved = entity.QuantityReserved,
                LastStockDate = entity.LastStockDate,
                Product = entity.ProductVariant != null ?
                new ReferenceDataResponse<int>() { Id = entity.ProductVariant.Id, Name = entity.ProductVariant.Product.Name } :
                default!,
                Batch = entity.Batch != null ?
                new ReferenceDataResponse<int>() { Id = entity.Batch.Id, Name = entity.Batch.BatchNumber } :
                default!,
                Warehouse = entity.Warehouse != null ?
                new ReferenceDataResponse<int>() { Id = entity.Warehouse.Id, Name = entity.Warehouse.Name } :
                default!
            };

            return response;
        }
    }

    extension(BinStock entity)
    {
        public StockResponse AsApiResponse()
        {
            StockResponse response = new()
            {
                Id = entity.Id,
                QuantityOnHand = entity.Quantity,
                LastStockDate = entity.LastStockDate,
                Product = entity.ProductVariant != null ?
                new ReferenceDataResponse<int>() { Id = entity.ProductVariant.Id, Name = entity.ProductVariant.Product.Name } :
                default!,
                Bin = entity.Bin != null ?
                new ReferenceDataResponse<int>() { Id = entity.Bin.Id, Name = entity.Bin.Name } :
                default!
            };

            return response;
        }
    }

}


