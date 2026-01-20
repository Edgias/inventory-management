namespace EdgIQ.Inventory.WebAPI.Suppliers;

public static class SupplierMapper
{
    extension(Supplier entity)
    {
        public SupplierResponse AsApiResponse()
        {
            SupplierResponse response = new()
            {
                Id = entity.Id,
                Name = entity.Name,
                ContactName = entity.ContactName,
                Email = entity.Email,
                Phone = entity.Phone,
                City = entity.Address?.City,
                Country = entity.Address?.Country,
                State = entity.Address?.State,
                Street = entity.Address?.Street,
                ZipCode = entity.Address?.ZipCode

            };

            return response;
        }
    }

    extension(SupplierRequest request)
    {
        public Supplier ToEntity()
        {
            Address? address = null;
            if (!string.IsNullOrEmpty(request.Street))
            {
                address = new Address(request.Street, request.City, request.State, request.ZipCode, request.Country);
            }

            Supplier entity = new(request.Name, request.ContactName, request.Phone, request.Email, address);

            return entity;
        }
    }

    extension(Supplier entity)
    {
        public void Update(SupplierRequest request)
        {
            Address? address = null;
            if (!string.IsNullOrEmpty(request.Street))
            {
                address = new Address(request.Street, request.City, request.State, request.ZipCode, request.Country);
            }

            entity.Update(request.Name, request.ContactName, request.Phone, request.Email, address);
        }
    }
}


