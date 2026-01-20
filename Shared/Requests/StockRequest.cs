namespace EdgIQ.Inventory.Shared.Requests;

public record StockRequest
{
    public int ProductVariantId { get; init; }

    public int WarehouseId { get; init; }

    public int? BatchId { get; init; }

    public int? BinId { get; init; }

    public int QuantityOnHand { get; init; }

    // Stock sold but not yet shipped
    public int QuantityReserved { get; init; }

    public DateTime? LastStockDate { get; init; }
}
