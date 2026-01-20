namespace EdgIQ.Inventory.Shared.Responses;

public record DepartmentResponse : BaseResponse
{
    public string Name { get; init; } = default!;
}
