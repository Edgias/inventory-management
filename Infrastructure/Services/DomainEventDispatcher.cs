using Edgias.Inventory.Management.ApplicationCore.Events;
using Edgias.Inventory.Management.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Edgias.Inventory.Management.Infrastructure.Services
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IEnumerable<IDomainEventHandler> _domainEventHandlers;

        public DomainEventDispatcher(IEnumerable<IDomainEventHandler> domainEventHandlers)
        {
            _domainEventHandlers = domainEventHandlers;
        }

        public void Dispatch(BaseDomainEvent domainEvent)
        {
            IDomainEventHandler domainEventHandler = GetEventHandler(domainEvent);

            domainEventHandler.HandleAsync(domainEvent);
        }

        private IDomainEventHandler GetEventHandler<T>(T model) where T : BaseDomainEvent
        {
            IDomainEventHandler handler = _domainEventHandlers.FirstOrDefault(eh => eh.AppliesTo(model.GetType()));

            if (handler == null)
            {
                throw new InvalidOperationException($"Event handler for ({model.GetType()} not registered.)");
            }

            return handler;
        }
    }
}
