using EdgIQ.Inventory.Shared.Enums;

namespace EdgIQ.Inventory.Shared.Requests;

public record PurchaseOrderRequest
{
    public DateTime OrderDate { get; init; }

    public DateTime ExpectedDeliveryDate { get; init; }

    public ApprovalStatus Status { get; init; }

    public string? Terms { get; init; }

    public int QuotationId { get; init; }

    public int SupplierId { get; init; }

    public int? WarehouseId { get; init; }

    public string? Street { get; init; }

    public string? City { get; init; }

    public string? State { get; init; }

    public string? ZipCode { get; init; }

    public string? Country { get; init; }

    public IEnumerable<PurchaseOrderItemRequest> PurchaseOrderItems { get; init; } = [];
}
