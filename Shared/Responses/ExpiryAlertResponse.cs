namespace EdgIQ.Inventory.Shared.Responses;

public record ExpiryAlertResponse : BaseResponse
{
    public ReferenceDataResponse<int> Batch { get; init; } = default!;

    public ReferenceDataResponse<int> Product { get; init; } = default!;

    public DateTime AlertDate { get; init; }

    public int DaysToExpiry { get; init; }

    public string? Action { get; init; }

}
