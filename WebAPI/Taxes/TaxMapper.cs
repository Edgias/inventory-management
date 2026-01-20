namespace EdgIQ.Inventory.WebAPI.Taxes;

public static class TaxMapper
{
    extension(Tax entity)
    {
        public TaxResponse AsApiResponse()
        {
            TaxResponse response = new()
            {
                Id = entity.Id,
                Name = entity.Name,
                Percentage = entity.Percentage
            };

            return response;
        }
    }

    extension(TaxRequest request)
    {
        public Tax ToEntity()
        {
            Tax entity = new(request.Name, request.Percentage);
            return entity;
        }
    }

    extension(Tax entity)
    {
        public void Update(TaxRequest request)
        {
            entity.Update(request.Name, request.Percentage);
        }
    }
}


