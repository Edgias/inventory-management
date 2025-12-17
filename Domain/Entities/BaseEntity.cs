namespace EdgIQ.Inventory.Domain.Entities;

public abstract class BaseEntity
{
    public int Id { get; private set; }

    public string CreatedBy { get; private set; } = default!;

    public DateTimeOffset CreatedDate { get; private set; } = DateTimeOffset.UtcNow;

    public string? ModifiedBy { get; private set; }

    public DateTimeOffset LastModifiedDate { get; protected set; } = DateTimeOffset.UtcNow;

    public bool IsActive { get; private set; } = true;  

    public int TenantId { get; protected set; }

    public List<BaseDomainEvent> Events { get; private set; } = [];

    public void AddDomainEvent(BaseDomainEvent domainEvent)
    {
        Events.Add(domainEvent);
    }

    public void RemoveDomainEvent(BaseDomainEvent domainEvent)
    {
        Events.Remove(domainEvent);
    }

    public void ChangeStatus()
    {
        IsActive = !IsActive;
    }
}
