namespace EdgIQ.Inventory.Shared.Responses;

public record GoodsReceivedNoteItemResponse : BaseResponse
{
    public ReferenceDataResponse<int> Product { get; init; } = default!;

    public int PurchaseOrderItemId { get; init; }

    public ReferenceDataResponse<int>? Bin { get; init; }

    public int QuantityOrdered { get; init; }

    public int QuantityReceived { get; init; }

    public string? BatchNumber { get; init; }

    public DateTime? ExpiryDate { get; init; }

    public string? Notes { get; init; }
}
