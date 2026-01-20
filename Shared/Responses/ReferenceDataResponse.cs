namespace EdgIQ.Inventory.Shared.Responses;

public record ReferenceDataResponse<TId>
{
    public TId Id { get; init; } = default!;

    public string Name { get; init; } = default!;
}
