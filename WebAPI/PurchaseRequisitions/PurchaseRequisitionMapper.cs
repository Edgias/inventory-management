namespace EdgIQ.Inventory.WebAPI.PurchaseRequisitions;

public static class PurchaseRequisitionMapper
{
    public static PurchaseRequisitionResponse AsApiResponse(this PurchaseRequisition entity)
    {
        PurchaseRequisitionResponse response = new()
        {
            Id = entity.Id,
            RequisitionNumber = entity.RequisitionNumber,
            RequestDate = entity.RequestDate,
            RequiredDate = entity.RequestDate,
            Purpose = entity.Purpose,
            TotalAmount = entity.GetTotal(),
            Requester = !string.IsNullOrEmpty(entity.RequesterId) ?
            new ReferenceDataResponse<string>() { Id = entity.RequesterId } :
            default!,
            Department = entity.Department != null ?
            new ReferenceDataResponse<int>() { Id = entity.Department.Id, Name = entity.Department.Name } :
            default!,
            Warehouse = entity.Warehouse != null ?
            new ReferenceDataResponse<int>() { Id = entity.Warehouse.Id, Name = entity.Warehouse.Name } : 
            default!,
            PurchaseRequisitionItems = entity.PurchaseRequisitionItems.Select(ri => ri.AsApiResponse()),
        };

        return response;
    }

    public static PurchaseRequisition ToEntity(this PurchaseRequisitionRequest request, 
        string purchaseRequisitionNumber, string requesterId, List<ProductVariant> productVariants)
    {
        List<PurchaseRequisitionItem> purchaseRequisitionItems = [.. request.PurchaseRequisitionItems.Select(ri => ri.ToEntity(productVariants))];

        PurchaseRequisition entity = new(purchaseRequisitionNumber, request.RequestDate.ToUtc(), request.RequestDate.ToUtc(), request.Purpose, 
            requesterId, request.DepartmentId, request.WarehouseId, purchaseRequisitionItems);
        
        return entity;
    }

    public static void Update(this PurchaseRequisition entity, PurchaseRequisitionRequest request)
    {
        entity.Update(request.RequiredDate.ToUtc(), request.Purpose, request.DepartmentId,
            request.WarehouseId);
    }
}

