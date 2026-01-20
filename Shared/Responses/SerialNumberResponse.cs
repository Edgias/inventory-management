using EdgIQ.Inventory.Shared.Enums;

namespace EdgIQ.Inventory.Shared.Responses;

public record SerialNumberResponse : BaseResponse
{
    public string SerialNo { get; init; } = default!;

    public ReferenceDataResponse<int> Product { get; init; } = default!;

    public ReferenceDataResponse<int>? Batch { get; init; }

    public ReferenceDataResponse<int>? Warehouse { get; init; }

    public ReferenceDataResponse<int>? Bin { get; init; }

    public SerialNumberStatus Status { get; init; }

    public DateTime? PurchaseDate { get; init; }

    public DateTime? SaleDate { get; init; }

    public decimal? PurchaseCost { get; init; }

    public string? WarrantyInformation { get; init; }

    public string? Notes { get; init; }
}
