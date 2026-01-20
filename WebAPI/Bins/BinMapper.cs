namespace EdgIQ.Inventory.WebAPI.Bins;

public static class BinMapper
{
    extension(Bin entity)
    {
        public BinResponse AsApiResponse()
        {
            BinResponse response = new()
            {
                Id = entity.Id,
                Name = entity.Name,
                Color = entity.Color,
                Depth = entity.Depth,
                DividerSlots = entity.DividerSlots,
                Height = entity.Height,
                SerialNumber = entity.SerialNumber,
                Weight = entity.Weight,
                Width = entity.Width,
                Warehouse = new ReferenceDataResponse<int>
                {
                    Id = entity.Warehouse.Id,
                    Name = entity.Warehouse.Name
                }
            };

            return response;
        }
    }

    extension(BinRequest request)
    {
        public Bin ToEntity()
        {
            Bin entity = new(request.Name, request.SerialNumber, request.Color, request.Width,
                request.Depth, request.Height, request.DividerSlots, request.Weight,
                request.WarehouseId);

            return entity;
        }
    }

    extension(Bin entity)
    {
        public void Update(BinRequest request)
        {
            entity.Update(request.Name, request.SerialNumber, request.Color, request.Width,
                request.Depth, request.Height, request.DividerSlots, request.Weight);
        }
    }
}


