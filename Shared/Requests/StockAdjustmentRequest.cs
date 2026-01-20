using EdgIQ.Inventory.Shared.Enums;

namespace EdgIQ.Inventory.Shared.Requests;

public record StockAdjustmentRequest
{
    public int WarehouseId { get; init; }

    public DateTime AdjustmentDate { get; init; }

    public StockAdjustmentReason Reason { get; init; }

    public string? ReasonDescription { get; init; }

    public string? ApproverId { get; init; }

    public DateTime? ApprovedDate { get; init; }

    public string? ReferenceDocument { get; init; }

    public string? Notes { get; init; }

    public IEnumerable<StockAdjustmentItemRequest> StockAdjustmentItems { get; init; } = [];
}
