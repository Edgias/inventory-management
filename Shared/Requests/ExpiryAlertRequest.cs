namespace EdgIQ.Inventory.Shared.Requests;

public record ExpiryAlertRequest
{
    public int BatchId { get; init; }

    public DateTime AlertDate { get; init; }

    public int DaysToExpiry { get; init; }

    public string? Action { get; init; }

}
