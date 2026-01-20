namespace EdgIQ.Inventory.Shared.Responses;

public record ProductResponse : BaseResponse
{
    public string Name { get; init; } = default!;

    public string? Description { get; init; }

    public ReferenceDataResponse<int>? ProductCategory { get; init; }
}
