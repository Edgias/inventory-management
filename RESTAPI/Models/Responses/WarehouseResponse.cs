using System;

namespace Edgias.Inventory.Management.RESTAPI.Models.Responses
{
    public class WarehouseResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public LocationResponse Location { get; set; }
    }
}
