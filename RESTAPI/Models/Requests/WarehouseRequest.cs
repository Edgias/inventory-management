using System;

namespace Edgias.Inventory.Management.RESTAPI.Models.Requests
{
    public class WarehouseRequest
    {
        public string Name { get; set; }

        public Guid LocationId { get; set; }
    }
}
