using System;

namespace Edgias.Inventory.Management.ApplicationCore.Entities
{
    public class Bin : BaseEntity
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

        public Warehouse Warehouse { get; private set; }

        private Bin()
        {
        }

        public Bin(string name, string serialNumber, string color, int? width, int? depth, decimal? height, int? dividerSlots, decimal? weight, Guid warehouseId)
        {
            Name = name;
            SerialNumber = serialNumber;
            Color = color;
            Width = width;
            Depth = depth;
            Height = height;
            DividerSlots = dividerSlots;
            Weight = weight;
            WarehouseId = warehouseId;
        }

        public void UpdateDetails(string name, string serialNumber, string color, int? width, int? depth, decimal? height, int? dividerSlots, decimal? weight)
        {
            Name = name;
            SerialNumber = serialNumber;
            Color = color;
            Width = width;
            Depth = depth;
            Height = height;
            DividerSlots = dividerSlots;
            Weight = weight;
            LastModifiedDate = DateTimeOffset.UtcNow;
        }

        public void ChangeWarehouse(Guid warehouseId)
        {
            WarehouseId = warehouseId;
            LastModifiedDate = DateTimeOffset.UtcNow;
        }
    }
}
