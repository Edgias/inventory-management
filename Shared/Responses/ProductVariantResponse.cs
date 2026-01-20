using EdgIQ.Inventory.Shared.Enums;

namespace EdgIQ.Inventory.Shared.Responses;

public record ProductVariantResponse : BaseResponse
{
    public ReferenceDataResponse<int> Product { get; init; } = default!;

    public string SKU { get; init; } = default!;

    public string? Barcode { get; init; }

    public decimal UnitPrice { get; init; }

    public string? Color { get; init; }

    public string? Size { get; init; }

    public int ReorderPoint { get; init; }

    public bool IsBatchTracked { get; init; }

    public bool IsSerialTracked { get; init; }

    public bool HasExpiryDate { get; init; }

    public int? ShelfLifeDays { get; init; }

    public CostingMethod CostingMethod { get; init; }

    public int? ExpiryAlertDays { get; init; }

    public ReferenceDataResponse<int> MeasurementUnit { get; init; } = default!;
}
