using System;

namespace Edgias.Inventory.Management.ApplicationCore.Entities
{
    public class Location : BaseEntity
    {
        public string Name { get; private set; }

        public Address LocationAddress { get; private set; }

        private Location()
        {
        }

        public Location(string name, Address locationAddress)
        {
            Name = name;
            LocationAddress = locationAddress;
        }

        public void UpdateDetails(string name, Address locationAddress)
        {
            Name = name;
            LocationAddress = locationAddress;
            LastModifiedDate = DateTimeOffset.UtcNow;
        }
    }
}
