using System;

namespace Edgias.Inventory.Management.RESTAPI.Models.Responses
{
    public class BinResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string SerialNumber { get; set; }

        public string Color { get; set; }

        public int? Width { get; set; }

        public int? Depth { get; set; }

        public decimal? Height { get; set; }

        public int? DividerSlots { get; set; }

        public decimal? Weight { get; set; }

        public Guid WarehouseId { get; set; }
    }
}
