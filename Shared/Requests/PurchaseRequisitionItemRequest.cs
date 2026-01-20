using EdgIQ.Inventory.Shared.Enums;

namespace EdgIQ.Inventory.Shared.Requests;

public record PurchaseRequisitionItemRequest
{
    public decimal UnitPrice { get; init; }

    public int Units { get; init; }

    public int ProductVariantId { get; init; } 

    public ApprovalStatus Status { get; init; }

    public string? Notes { get; init; }

}
