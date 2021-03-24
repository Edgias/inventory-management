using Edgias.Inventory.Management.ApplicationCore.Events;
using System;
using System.Collections.Generic;

namespace Edgias.Inventory.Management.ApplicationCore.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }

        public DateTimeOffset CreatedDate { get; protected set; } = DateTimeOffset.Now;

        public DateTimeOffset LastModifiedDate { get; protected set; } = DateTimeOffset.Now;

        public string CreatedBy { get; protected set; }

        public string LastModifiedBy { get; protected set; }

        public bool IsActive { get; private set; } = true;

        public bool IsDeleted { get; private set; }

        public List<BaseDomainEvent> Events { get; private set; } = new List<BaseDomainEvent>();

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

        public void Delete()
        {
            IsDeleted = true;
        }
    }
}
