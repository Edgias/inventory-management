using System;
using System.Collections.Generic;

namespace Edgias.Inventory.Management.ApplicationCore.Entities
{
    public class Warehouse : BaseEntity
    {
        public string Name { get; set; }

        public Guid LocationId { get; set; }

        public Location Location { get; set; }

        private readonly List<Bin> _bins = new List<Bin>();

        public IReadOnlyCollection<Bin> Bins => _bins.AsReadOnly();
    }
}
