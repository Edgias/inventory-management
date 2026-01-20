using EdgIQ.Inventory.Shared.Enums;

namespace EdgIQ.Inventory.Shared.Requests;

public record BatchRequest
{
    public int ProductVariantId { get; init; }
    
    public int SupplierId { get; init; }

    public string? SerialNumber { get; init; }

    public BatchStatus Status { get; init; }

    public DateTime ManufactureDate { get; init; }

    public DateTime? ExpiryDate { get; init; }

    public DateTime ReceivedDate { get; init; }

    public int InitialQuantity { get; init; }

    public decimal UnitPrice { get; init; }

    public string? QualityCertificate { get; init; }

    public string? Notes { get; init; }
}
