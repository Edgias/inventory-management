using System.ComponentModel.DataAnnotations;

namespace EdgIQ.Inventory.Shared.Requests;

public record PurchaseRequisitionRequest
{
    public DateTime RequestDate { get; init; }

    public DateTime RequiredDate { get; init; }

    public string? Purpose { get; init; }

    [Required]
    public string RequesterId { get; init; } = default!;

    public int? DepartmentId { get; init; }

    public int? WarehouseId { get; init; }

    public IEnumerable<PurchaseRequisitionItemRequest> PurchaseRequisitionItems { get; init; } = [];
}
