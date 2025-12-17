namespace EdgIQ.Inventory.Domain.Events;

public abstract class BaseDomainEvent
{
    public DateTimeOffset DateOccurred { get; protected set; } = DateTimeOffset.Now;
}
