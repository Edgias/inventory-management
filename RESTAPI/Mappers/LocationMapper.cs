using Edgias.Inventory.Management.ApplicationCore.Entities;
using Edgias.Inventory.Management.RESTAPI.Interfaces;
using Edgias.Inventory.Management.RESTAPI.Models.Requests;
using Edgias.Inventory.Management.RESTAPI.Models.Responses;

namespace Edgias.Inventory.Management.RESTAPI.Mappers
{
    public class LocationMapper : IMapper<Location, LocationRequest, LocationResponse>
    {
        public Location Map(LocationRequest request)
        {
            Location location = new(request.Name, GetAddress(request));

            return location;
        }

        public LocationResponse Map(Location entity)
        {
            LocationResponse response = new()
            {
                Id = entity.Id,
                Name = entity.Name,
                State = entity.LocationAddress?.State,
                Street = entity.LocationAddress?.Street,
                City = entity.LocationAddress?.City,
                Country = entity.LocationAddress?.Country,
                ZipCode = entity.LocationAddress?.ZipCode
            };

            return response;
        }

        public void Map(Location entity, LocationRequest request)
        {
            
            entity.UpdateDetails(request.Name, GetAddress(request));
        }

        private Address GetAddress(LocationRequest request)
        {
            Address address = new(request.Street, request.City, request.State, request.ZipCode, request.Country);

            return address;
        }
    }
}
