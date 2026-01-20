using EdgIQ.Inventory.Shared.Enums;

namespace EdgIQ.Inventory.Shared.Responses;

public record StockAdjustmentResponse : BaseResponse
{
    public string AdjustmentNumber { get; init; } = default!;

    public ReferenceDataResponse<int> Warehouse { get; init; } = default!;

    public DateTime AdjustmentDate { get; init; }

    public StockAdjustmentReason Reason { get; init; }

    public string? ReasonDescription { get; init; }

    public ReferenceDataResponse<string>? Approver { get; init; }

    public DateTime? ApprovedDate { get; init; }

    public string? ReferenceDocument { get; init; }

    public string? Notes { get; init; }

    public IEnumerable<StockAdjustmentItemResponse> StockAdjustmentItems { get; init; } = [];
}
