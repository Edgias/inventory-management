namespace EdgIQ.Inventory.WebAPI.PurchaseOrders;

public static class PurchaseOrderMapper
{
    public static PurchaseOrderResponse AsApiResponse(this PurchaseOrder entity)
    {
        PurchaseOrderResponse response = new()
        {
            Id = entity.Id,
            PONumber = entity.PONumber,
            ExpectedDeliveryDate = entity.ExpectedDeliveryDate,
            OrderDate = entity.OrderDate,
            Terms = entity.Terms,
            Status = entity.Status,
            Street = entity.ShippingAddress?.Street,
            City = entity.ShippingAddress?.City,
            State = entity.ShippingAddress?.State,
            ZipCode = entity.ShippingAddress?.ZipCode,
            Country = entity.ShippingAddress?.Country,
            TotalAmount = entity.GetTotal(),
            Supplier = entity.Supplier != null ?
            new ReferenceDataResponse<int>() { Id = entity.Supplier.Id, Name = entity.Supplier.Name } :
            default!,
            Warehouse = entity.Warehouse != null ?
            new ReferenceDataResponse<int>() { Id = entity.Warehouse.Id, Name = entity.Warehouse.Name } :
            default!,
            Quotation = entity.Quotation != null ?
            new ReferenceDataResponse<int>() { Id = entity.Quotation.Id, Name = entity.Quotation.QuotationNumber } : 
            default!,
            PurchaseOrderItems = entity.PurchaseOrderItems.Select(qi => qi.AsApiResponse())
        };

        return response;
    }

    public static PurchaseOrder ToEntity(this PurchaseOrderRequest request, string purchaseOrderNumber, List<ProductVariant> productVariants)
    {
        Address? address = null;
        if (!string.IsNullOrEmpty(request.Street))
        {
            address = new Address(request.Street, request.City, request.State, request.ZipCode, request.Country);
        }

        List<PurchaseOrderItem> purchaseOrderItems = [.. request.PurchaseOrderItems.Select(poi => poi.ToEntity(productVariants))];

        PurchaseOrder entity = new(purchaseOrderNumber, request.OrderDate.ToUtc(), request.ExpectedDeliveryDate.ToUtc(), 
            request.QuotationId, request.SupplierId, request.WarehouseId, address, request.Terms, purchaseOrderItems);
        
        return entity;
    }

    public static void Update(this PurchaseOrder entity, PurchaseOrderRequest request)
    {
        Address? address = null;
        if (!string.IsNullOrEmpty(request.Street))
        {
            address = new Address(request.Street, request.City, request.State, request.ZipCode, request.Country);
        }

        entity.Update(request.ExpectedDeliveryDate.ToUtc(), request.QuotationId, 
            request.SupplierId, request.WarehouseId, address, request.Terms);
    }
}

