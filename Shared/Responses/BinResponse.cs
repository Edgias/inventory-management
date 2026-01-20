namespace EdgIQ.Inventory.Shared.Responses;

public record BinResponse : BaseResponse
{
    public string Name { get; init; } = default!;

    public string? SerialNumber { get; init; }

    public string? Color { get; init; }

    public int? Width { get; init; }

    public int? Depth { get; init; }

    public decimal? Height { get; init; }

    public int? DividerSlots { get; init; }

    public decimal? Weight { get; init; }

    public ReferenceDataResponse<int> Warehouse { get; init; } = default!;
}
