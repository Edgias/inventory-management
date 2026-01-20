namespace EdgIQ.Inventory.Shared.Responses;

public record ProductCategoryResponse : BaseResponse
{
    public string Name { get; init; } = default!;

    public string? Description { get; init; }
}
