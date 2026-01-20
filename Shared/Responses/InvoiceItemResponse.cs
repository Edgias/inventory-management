namespace EdgIQ.Inventory.Shared.Responses;

public record InvoiceItemResponse : BaseResponse
{
    public ReferenceDataResponse<int> Product { get; init; } = default!;
    
    public int Quantity { get; init; }

    public decimal UnitPrice { get; init; }
}
