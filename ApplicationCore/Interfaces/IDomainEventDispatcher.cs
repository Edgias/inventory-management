using Edgias.Inventory.Management.ApplicationCore.Events;

namespace Edgias.Inventory.Management.ApplicationCore.Interfaces
{
    public interface IDomainEventDispatcher
    {
        void Dispatch(BaseDomainEvent domainEvent);
    }
}
