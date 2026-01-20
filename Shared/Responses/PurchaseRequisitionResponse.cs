namespace EdgIQ.Inventory.Shared.Responses;

public record PurchaseRequisitionResponse : BaseResponse
{
    public string RequisitionNumber { get; init; } = default!;

    public DateTime RequestDate { get; init; }

    public DateTime RequiredDate { get; init; }

    public string? Purpose { get; init; }

    public decimal TotalAmount { get; init; }

    public ReferenceDataResponse<string> Requester { get; init; } = default!;

    public ReferenceDataResponse<int>? Department { get; init; }

    public ReferenceDataResponse<int>? Warehouse { get; init; }

    public IEnumerable<PurchaseRequisitionItemResponse> PurchaseRequisitionItems { get; init; } = [];
}
