namespace EdgIQ.Inventory.Shared.Requests;

public record StockTransferItemRequest
{
    public int Id { get; init; }

    public int ProductVariantId { get; init; }

    public int QuantityRequested { get; init; }

    public int QuantityShipped { get; init; }

    public int QuantityReceived { get; init; }

    public int? FromBinId { get; init; }

    public int? ToBinId { get; init; }

    public string? Notes { get; init; }

}
