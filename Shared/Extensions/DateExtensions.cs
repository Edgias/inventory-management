namespace EdgIQ.Inventory.Shared.Extensions;

public static class DateExtensions
{
    public static DateTime? ToUtc(this DateTime? date)
    {
        return date.HasValue ? date.Value.ToUniversalTime() : null;
    }

    public static DateTime ToUtc(this DateTime date)
    {
        return date.ToUniversalTime();
    }
}
