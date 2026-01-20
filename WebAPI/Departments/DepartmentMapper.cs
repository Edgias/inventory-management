namespace EdgIQ.Inventory.WebAPI.Departments;

public static class DepartmentMapper
{
    extension(Department entity)
    {
        public DepartmentResponse AsApiResponse()
        {
            DepartmentResponse response = new()
            {
                Id = entity.Id,
                Name = entity.Name
            };

            return response;
        }
    }

    extension(DepartmentRequest request)
    {
        public Department ToEntity()
        {
            Department entity = new(request.Name);
            return entity;
        }
    }

    extension(Department entity)
    {
        public void Update(DepartmentRequest request)
        {
            entity.Update(request.Name);
        }
    }
}


