using EdgIQ.Inventory.Shared.Enums;

namespace EdgIQ.Inventory.Shared.Requests;

public record PurchaseOrderItemRequest
{
    public decimal UnitPrice { get; init; }

    public int Units { get; init; }

    public int ProductVariantId { get; init; } 

    public ApprovalStatus Status { get; init; }
}
