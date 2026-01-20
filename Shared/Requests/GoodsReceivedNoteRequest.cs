namespace EdgIQ.Inventory.Shared.Requests;

public record GoodsReceivedNoteRequest
{
    public DateTime ReceivedDate { get; init; } 

    public int SupplierId { get; init; }

    public int WarehouseId { get; init; }

    public int PurchaseOrderId { get; init; }

    public string? ExternalReference { get; init; }

    public string? Notes { get; init; }

    public IEnumerable<GoodsReceivedNoteItemRequest> GoodsReceivedNoteItems { get; init; } = [];
}
