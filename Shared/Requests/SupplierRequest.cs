using System.ComponentModel.DataAnnotations;

namespace EdgIQ.Inventory.Shared.Requests;

public record SupplierRequest
{
    [Required]
    public string Name { get; init; } = default!;

    public string? ContactName { get; init; }

    public string? Phone { get; init; }

    [EmailAddress]
    public string? Email { get; init; }

    public string? Street { get; init; } 

    public string? City { get; init; } 

    public string? State { get; init; } 

    public string? ZipCode { get; init; } 

    public string? Country { get; init; } 
}
