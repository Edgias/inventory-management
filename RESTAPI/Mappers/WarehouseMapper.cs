using Edgias.Inventory.Management.ApplicationCore.Entities;
using Edgias.Inventory.Management.RESTAPI.Interfaces;
using Edgias.Inventory.Management.RESTAPI.Models.Requests;
using Edgias.Inventory.Management.RESTAPI.Models.Responses;

namespace Edgias.Inventory.Management.RESTAPI.Mappers
{
    public class WarehouseMapper : IMapper<Warehouse, WarehouseRequest, WarehouseResponse>
    {
        public Warehouse Map(WarehouseRequest request)
        {
            Warehouse warehouse = new(request.Name, request.LocationId);

            return warehouse;
        }

        public WarehouseResponse Map(Warehouse entity)
        {
            WarehouseResponse response = new()
            {
                Id = entity.Id,
                Name = entity.Name,
                Location = new LocationResponse
                {
                    Name = entity.Location?.Name,
                    State = entity.Location?.LocationAddress?.State,
                    Street = entity.Location?.LocationAddress?.Street,
                    City = entity.Location?.LocationAddress?.City,
                    Country = entity.Location?.LocationAddress?.Country,
                    ZipCode = entity.Location?.LocationAddress?.ZipCode
                }
            };

            return response;
        }

        public void Map(Warehouse entity, WarehouseRequest request)
        {
            entity.UpdateDetails(request.Name);
        }
    }
}
