namespace EdgIQ.Inventory.Shared.Responses;

public record MeasurementUnitResponse : BaseResponse
{
    public string Name { get; init; } = default!;
}
