namespace EdgIQ.Inventory.Shared.Requests;

public record StockAdjustmentItemRequest
{
    public int ProductVariantId { get; init; }

    public int? BatchId { get; init; }

    public int? SerialNumberId { get; init; }

    public int? BinId { get; init; }

    public int SystemQuantity { get; init; }

    public int ActualQuantity { get; init; }

    public decimal UnitPrice { get; init; }

    public string? Notes { get; init; }
}
