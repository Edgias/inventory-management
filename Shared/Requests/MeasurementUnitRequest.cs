using System.ComponentModel.DataAnnotations;

namespace EdgIQ.Inventory.Shared.Requests;

public record MeasurementUnitRequest
{
    [Required]
    public string Name { get; init; } = default!;
}
