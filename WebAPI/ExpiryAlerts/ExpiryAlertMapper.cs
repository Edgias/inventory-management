namespace EdgIQ.Inventory.WebAPI.ExpiryAlerts;

public static class ExpiryAlertMapper
{
    extension(ExpiryAlert entity)
    {
        public ExpiryAlertResponse AsApiResponse()
        {
            ExpiryAlertResponse response = new()
            {
                Id = entity.Id,
                AlertDate = entity.AlertDate,
                Action = entity.Action,
                DaysToExpiry = entity.DaysToExpiry,
                Batch = entity.Batch != null ?
                new ReferenceDataResponse<int>() { Id = entity.Batch.Id, Name = entity.Batch.BatchNumber } :
                default!,
                Product = entity.Batch != null && entity.Batch.ProductVariant != null ?
                new ReferenceDataResponse<int>() { Id = entity.Batch.ProductVariant.Id, Name = entity.Batch.ProductVariant.Product.Name } :
                default!,
            };

            return response;
        }
    }

    extension(ExpiryAlertRequest request)
    {
        public ExpiryAlert ToEntity()
        {
            ExpiryAlert entity = new(request.BatchId, request.AlertDate, request.DaysToExpiry, AlertLevel.Info);
            return entity;
        }
    }

    extension(ExpiryAlert entity)
    {
        public void Update(ExpiryAlertRequest request)
        {
            entity.Update(request.AlertDate, request.DaysToExpiry, request.Action);
        }
    }
}


