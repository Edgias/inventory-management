using System;
using System.Collections.Generic;

namespace Edgias.Inventory.Management.ApplicationCore.Entities
{
    public class Warehouse : BaseEntity
    {
        public string Name { get; private set; }

        public Guid LocationId { get; private set; }

        public Location Location { get; private set; }

        private readonly List<Bin> _bins = new();

        public IReadOnlyCollection<Bin> Bins => _bins.AsReadOnly();

        private Warehouse()
        {

        }

        public Warehouse(string name, Guid locationId)
        {
            Name = name;
            LocationId = locationId;
        }

        public void UpdateDetails(string name)
        {
            Name = name;
        }

        public void ChangeLocation(Guid locationId)
        {
            LocationId = locationId;
        }
    }
}
