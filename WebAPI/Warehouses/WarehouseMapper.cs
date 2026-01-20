namespace EdgIQ.Inventory.WebAPI.Warehouses;

public static class WarehouseMapper
{
    extension(Warehouse entity)
    {
        public WarehouseResponse AsApiResponse()
        {
            WarehouseResponse response = new()
            {
                Id = entity.Id,
                Name = entity.Name,
                Phone = entity.Phone,
                City = entity.LocationAddress?.City,
                Country = entity.LocationAddress?.Country,
                ZipCode = entity.LocationAddress?.ZipCode,
                ManagerId = entity.ManagerId,
                State = entity.LocationAddress?.State,
                Street = entity.LocationAddress?.Street
            };

            return response;
        }
    }

    extension(WarehouseRequest request)
    {
        public Warehouse ToEntity()
        {
            Address? address = null;
            if (!string.IsNullOrEmpty(request.Street))
            {
                address = new Address(request.Street, request.City, request.State, request.ZipCode, request.Country);
            }

            Warehouse entity = new(request.Name, request.Phone, request.ManagerId, address);
            return entity;
        }
    }

    extension(Warehouse entity)
    {
        public void Update(WarehouseRequest request)
        {
            Address? address = null;
            if (!string.IsNullOrEmpty(request.Street))
            {
                address = new Address(request.Street, request.City, request.State, request.ZipCode, request.Country);
            }

            entity.Update(request.Name, request.Phone, request.ManagerId, address);
        }
    }
}


