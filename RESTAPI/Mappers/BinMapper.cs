using Edgias.Inventory.Management.ApplicationCore.Entities;
using Edgias.Inventory.Management.RESTAPI.Interfaces;
using Edgias.Inventory.Management.RESTAPI.Models.Requests;
using Edgias.Inventory.Management.RESTAPI.Models.Responses;

namespace Edgias.Inventory.Management.RESTAPI.Mappers
{
    public class BinMapper : IMapper<Bin, BinRequest, BinResponse>
    {
        public Bin Map(BinRequest request)
        {
            Bin bin = new(request.Name, request.SerialNumber, request.Color, request.Width, request.Depth, request.Height,
                request.DividerSlots, request.Weight, request.WarehouseId);

            return bin;
        }

        public BinResponse Map(Bin entity)
        {
            BinResponse response = new()
            {
                Id = entity.Id,
                WarehouseId = entity.WarehouseId,
                Color = entity.Color,
                Depth = entity.Depth,
                DividerSlots = entity.DividerSlots,
                Height = entity.Height,
                Name = entity.Name,
                SerialNumber = entity.SerialNumber,
                Weight = entity.Weight,
                Width = entity.Width
            };

            return response;
        }

        public void Map(Bin entity, BinRequest request)
        {
            entity.UpdateDetails(request.Name, request.SerialNumber, request.Color, request.Width, request.Depth, request.Height, 
                request.DividerSlots, request.Weight);
        }
    }
}
