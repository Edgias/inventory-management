using EdgIQ.Inventory.Shared.Enums;

namespace EdgIQ.Inventory.Shared.Responses;

public record StockTransactionResponse : BaseResponse
{
    public string TransactionNumber { get; init; } = default!;

    public DateTime TransactionDate { get; init; }

    public ReferenceDataResponse<int> Product { get; init; } = default!;

    public ReferenceDataResponse<int>? Batch { get; init; }

    public ReferenceDataResponse<int>? SerialNumber { get; init; }

    public ReferenceDataResponse<int>? Warehouse { get; init; }

    public ReferenceDataResponse<int>? Bin { get; init; }

    // Positive for restock, negative for sales
    public int Quantity { get; init; }

    public decimal UnitPrice { get; init; }

    public TransactionType TransactionType { get; init; }

    public DocumentType DocumentType { get; init; }

    public int? DocumentId { get; init; }

    public string? Notes { get; init; }
}
