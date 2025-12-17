namespace EdgIQ.Inventory.Domain.Events;

public class BinMovedEvent : BaseDomainEvent
{
    public Bin MovedBin { get; private set; }

    public BinMovedEvent(Bin movedBin)
    {
        MovedBin = movedBin;
    }
}
