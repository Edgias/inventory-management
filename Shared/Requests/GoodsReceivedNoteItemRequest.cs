namespace EdgIQ.Inventory.Shared.Requests;

public record GoodsReceivedNoteItemRequest
{
    public int ProductVariantId { get; init; }

    public int PurchaseOrderItemId { get; init; }

    public int? BinId { get; init; }

    public int QuantityOrdered { get; init; }

    public int QuantityReceived { get; init; }

    public string? BatchNumber { get; init; }

    public DateTime? ExpiryDate { get; init; }

    public string? Notes { get; init; }
}
