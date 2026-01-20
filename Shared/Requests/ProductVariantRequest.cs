using EdgIQ.Inventory.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace EdgIQ.Inventory.Shared.Requests;

public record ProductVariantRequest
{
    public int ProductId { get; init; }

    [Required]
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

    public int MeasurementUnitId { get; init; }
}
