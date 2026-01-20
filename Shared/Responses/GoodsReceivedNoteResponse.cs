using EdgIQ.Inventory.Shared.Enums;

namespace EdgIQ.Inventory.Shared.Responses;

public record GoodsReceivedNoteResponse : BaseResponse
{
    public string GRNNumber { get; init; } = default!;

    public DateTime ReceivedDate { get; init; }

    public ReferenceDataResponse<int> Supplier { get; init; } = default!;

    public ReferenceDataResponse<int> Warehouse { get; init; } = default!;

    public int PurchaseOrderId { get; init; }

    public ReferenceDataResponse<string> Receiver { get; init; } = default!;

    public string? ExternalReference { get; init; }

    public GrnStatus Status { get; init; } 

    public string? Notes { get; init; }

    public IEnumerable<GoodsReceivedNoteItemResponse> GoodsReceivedNoteItems { get; init; } = [];
}
