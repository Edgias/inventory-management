using EdgIQ.Inventory.Shared.Enums;

namespace EdgIQ.Inventory.Shared.Responses;

public record RequestForQuotationResponse : BaseResponse
{
    public string RFQNumber { get; init; } = default!;

    public ReferenceDataResponse<int>? PurchaseRequisition { get; init; }

    public DateTime RFQDate { get; init; }

    public DateTime ResponseDeadline { get; init; }

    public string? Description { get; init; }

    public string? Terms { get; init; }

    public RFQStatus Status { get; init; }

    public string? Street { get; init; }

    public string? City { get; init; }

    public string? State { get; init; }

    public string? ZipCode { get; init; }

    public string? Country { get; init; }

    public IEnumerable<RequestForQuotationItemResponse> RequestForQuotationItems { get; init; } = [];
}
