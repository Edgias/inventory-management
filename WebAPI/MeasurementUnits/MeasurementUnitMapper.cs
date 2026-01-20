namespace EdgIQ.Inventory.WebAPI.MeasurementUnits;

public static class MeasurementUnitMapper
{
    extension(MeasurementUnit entity)
    {
        public MeasurementUnitResponse AsApiResponse()
        {
            MeasurementUnitResponse response = new()
            {
                Id = entity.Id,
                Name = entity.Name
            };

            return response;
        }
    }

    extension(MeasurementUnitRequest request)
    {
        public MeasurementUnit ToEntity()
        {
            MeasurementUnit entity = new(request.Name);
            return entity;
        }
    }

    extension(MeasurementUnit entity)
    {
        public void Update(MeasurementUnitRequest request)
        {
            entity.Update(request.Name);
        }
    }
}


