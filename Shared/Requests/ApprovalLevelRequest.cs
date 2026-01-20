using EdgIQ.Inventory.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace EdgIQ.Inventory.Shared.Requests;

public record ApprovalLevelRequest
{
    [Required]
    public string Name { get; init; } = default!;

    public int Level { get; init; }

    public decimal MinAmount { get; init; }

    public decimal MaxAmount { get; init; }

    [Required]
    public string RoleId { get; init; } = default!;

    public int? DepartmentId { get; init; }

    public DocumentType DocumentType { get; init; }
}
