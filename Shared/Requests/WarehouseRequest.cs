using System.ComponentModel.DataAnnotations;

namespace EdgIQ.Inventory.Shared.Requests;

public record WarehouseRequest
{
    [Required]
    public string Name { get; init; } = default!;

    public string? Phone { get; init; } 

    public string? ManagerId { get; init; }

    public string? Street { get; init; }

    public string? City { get; init; }

    public string? State { get; init; }

    public string? ZipCode { get; init; }

    public string? Country { get; init; }
}
