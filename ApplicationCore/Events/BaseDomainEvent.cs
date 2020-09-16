using System;

namespace Edgias.Inventory.Management.ApplicationCore.Events
{
    public abstract class BaseDomainEvent
    {
        public DateTimeOffset DateOccurred { get; protected set; } = DateTimeOffset.Now;
    }
}
