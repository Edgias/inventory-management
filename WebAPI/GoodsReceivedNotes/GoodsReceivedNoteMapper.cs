namespace EdgIQ.Inventory.WebAPI.GoodsReceivedNotes;

public static class GoodsReceivedNoteMapper
{
    public static GoodsReceivedNoteResponse AsApiResponse(this GoodsReceivedNote entity)
    {
        GoodsReceivedNoteResponse response = new()
        {
            Id = entity.Id,
            GRNNumber = entity.GRNNumber,
            ReceivedDate = entity.ReceivedDate,
            Status = entity.Status,
            ExternalReference = entity.ExternalReference,
            Notes = entity.Notes,
            Receiver = !string.IsNullOrEmpty(entity.ReceiverId) ?
            new ReferenceDataResponse<string>() { Id = entity.ReceiverId } :
            default!,
            Supplier = entity.Supplier != null ?
            new ReferenceDataResponse<int>() { Id = entity.Supplier.Id, Name = entity.Supplier.Name } :
            default!,
            Warehouse = entity.Warehouse != null ?
            new ReferenceDataResponse<int>() { Id = entity.Warehouse.Id, Name = entity.Warehouse.Name } : 
            default!,
            GoodsReceivedNoteItems = entity.GoodsReceivedNoteItems.Select(grni => grni.AsApiResponse()),
        };

        return response;
    }

    public static GoodsReceivedNote ToEntity(this GoodsReceivedNoteRequest request, string goodsReceivedNoteNumber, 
        string receiverId, List<ProductVariant> productVariants)
    {
        List<GoodsReceivedNoteItem> goodsReceivedNoteItems = [.. request.GoodsReceivedNoteItems.Select(grni => grni.ToEntity(productVariants))];

        GoodsReceivedNote entity = new(goodsReceivedNoteNumber, request.ReceivedDate.ToUtc(), request.SupplierId, request.WarehouseId, 
            request.PurchaseOrderId, receiverId, request.ExternalReference, goodsReceivedNoteItems, request.Notes);
        
        return entity;
    }

    public static void Update(this GoodsReceivedNote entity, GoodsReceivedNoteRequest request)
    {
        entity.Update(request.ReceivedDate.ToUtc(), request.SupplierId, request.WarehouseId,
            request.PurchaseOrderId, request.ExternalReference, request.Notes);
    }
}

