using EdgIQ.Inventory.Shared.Enums;

namespace EdgIQ.Inventory.Shared.Responses;

public record PurchaseRequisitionItemResponse : BaseResponse
{
    public decimal UnitPrice { get; init; }

    public int Units { get; init; }

    public ReferenceDataResponse<int> Product { get; init; } = default!;

    public ApprovalStatus Status { get; init; }

    public string? Notes { get; init; }

}
