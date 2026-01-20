using System.ComponentModel.DataAnnotations;

namespace EdgIQ.Inventory.Shared.Requests;

public record TaxRequest
{
    [Required]
    public string Name { get; init; } = default!;

    public decimal Percentage { get; init; }
}
