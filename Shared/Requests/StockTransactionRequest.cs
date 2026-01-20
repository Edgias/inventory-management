using EdgIQ.Inventory.Shared.Enums;

namespace EdgIQ.Inventory.Shared.Requests;

public record StockTransactionRequest
{
    public DateTime TransactionDate { get; init; }

    public int ProductVariantId { get; init; }

    public int? BatchId { get; init; }

    public int? SerialNumberId { get; init; }

    public int? WarehouseId { get; init; }

    public int? BinId { get; init; }

    // Positive for restock, negative for sales
    public int Quantity { get; init; }

    public decimal UnitPrice { get; init; }

    public TransactionType Type { get; init; }

    public DocumentType DocumentType { get; init; }

    public int? DocumentId { get; init; }

    public string? Notes { get; init; }
}
