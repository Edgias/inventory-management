using EdgIQ.Inventory.Shared.Enums;

namespace EdgIQ.Inventory.Shared.Responses;

public record PurchaseOrderResponse : BaseResponse
{
    public string PONumber { get; init; } = default!;

    public DateTime OrderDate { get; init; }

    public DateTime ExpectedDeliveryDate { get; init; }

    public ApprovalStatus Status { get; init; }

    public decimal TotalAmount { get; init; }

    public string? Terms { get; init; }

    public ReferenceDataResponse<int> Quotation { get; init; } = default!;

    public ReferenceDataResponse<int> Supplier { get; init; } = default!;

    public ReferenceDataResponse<int>? Warehouse { get; init; }

    public string? Street { get; init; }

    public string? City { get; init; }

    public string? State { get; init; }

    public string? ZipCode { get; init; }

    public string? Country { get; init; }

    public IEnumerable<PurchaseOrderItemResponse> PurchaseOrderItems { get; init; } = [];
}
