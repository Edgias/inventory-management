using EdgIQ.Inventory.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace EdgIQ.Inventory.Shared.Requests;

public record SerialNumberRequest
{
    [Required]
    public string SerialNo { get; init; } = default!;

    public int ProductVariantId { get; init; }

    public int? BatchId { get; init; }

    public int? WarehouseId { get; init; }

    public int? BinId { get; init; }

    public SerialNumberStatus Status { get; init; }

    public DateTime? PurchaseDate { get; init; }

    public DateTime? SaleDate { get; init; }

    public decimal? PurchaseCost { get; init; }

    public string? WarrantyInformation { get; init; }

    public string? Notes { get; init; }
}
