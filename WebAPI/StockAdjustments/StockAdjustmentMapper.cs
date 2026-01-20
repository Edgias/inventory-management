namespace EdgIQ.Inventory.WebAPI.StockAdjustments;

public static class StockAdjustmentMapper
{
    extension(StockAdjustment entity)
    {
        public StockAdjustmentResponse AsApiResponse()
        {
            StockAdjustmentResponse response = new()
            {
                Id = entity.Id,
                AdjustmentNumber = entity.AdjustmentNumber,
                AdjustmentDate = entity.AdjustmentDate,
                ApprovedDate = entity.ApprovedDate,
                Approver = !string.IsNullOrEmpty(entity.ApproverId) ? 
                new ReferenceDataResponse<string>() { Id = entity.ApproverId } :
                default!,
                Notes = entity.Notes,
                Reason = entity.Reason,
                ReasonDescription = entity.ReasonDescription,
                ReferenceDocument = entity.ReferenceDocument,
                Warehouse = entity.Warehouse != null ?
                new ReferenceDataResponse<int>() { Id = entity.Warehouse.Id, Name = entity.Warehouse.Name } :
                default!,
                StockAdjustmentItems = entity.StockAdjustmentItems.Select(sai => sai.AsApiResponse())
            };

            return response;
        }
    }

    extension(StockAdjustmentRequest request)
    {
        public StockAdjustment ToEntity(string adjustmentNumber, string userId, List<ProductVariant> productVariants)
        {
            List<StockAdjustmentItem> stockAdjustmentItems = [.. request.StockAdjustmentItems.Select(sai => sai.ToEntity(productVariants))];

            StockAdjustment entity = new(adjustmentNumber, request.WarehouseId, request.Reason, request.ReasonDescription,
                userId, request.ReferenceDocument, request.Notes, stockAdjustmentItems);

           return entity;
        }
    }

    extension(StockAdjustment entity)
    {
        public void Update(StockAdjustmentRequest request)
        {
            entity.Update(request.WarehouseId, request.Reason, request.ReasonDescription,
                request.ReferenceDocument, request.Notes);
        }
    }
}
