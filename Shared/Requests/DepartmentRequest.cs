using System.ComponentModel.DataAnnotations;

namespace EdgIQ.Inventory.Shared.Requests;

public record DepartmentRequest
{
    [Required]
    public string Name { get; init; } = default!;
}
