using System;

namespace Edgias.Inventory.Management.RESTAPI.Models.Requests
{
    public class BinRequest
    {
        public string Name { get; private set; }

        public string SerialNumber { get; private set; }

        public string Color { get; private set; }

        public int? Width { get; private set; }

        public int? Depth { get; private set; }

        public decimal? Height { get; private set; }

        public int? DividerSlots { get; private set; }

        public decimal? Weight { get; private set; }

        public Guid WarehouseId { get; private set; }
    }
}
