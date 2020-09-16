using System;

namespace Edgias.Inventory.Management.ApplicationCore.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }

        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;

        public DateTimeOffset LastModifiedDate { get; set; } = DateTimeOffset.Now;
    }
}
