using System.ComponentModel.DataAnnotations;

namespace EdgIQ.Inventory.Shared.Requests;

public record ProductRequest
{
    [Required]
    public string Name { get; init; } = default!;

    public string? Description { get; init; }

    public int? ProductCategoryId { get; init; }
}
