using Edgias.Inventory.Management.ApplicationCore.Entities;

namespace Edgias.Inventory.Management.ApplicationCore.Events
{
    public class BinMovedEvent : BaseDomainEvent
    {
        public Bin MovedBin { get; private set; }

        public BinMovedEvent(Bin movedBin)
        {
            MovedBin = movedBin;
        }
    }
}
