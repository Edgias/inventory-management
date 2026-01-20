namespace EdgIQ.Inventory.WebAPI.ApprovalLevels;

public static class ApprovalLevelMapper
{
    extension(ApprovalLevel entity)
    {
        public ApprovalLevelResponse AsApiResponse()
        {
            ApprovalLevelResponse response = new()
            {
                Id = entity.Id,
                Name = entity.Name,
                DocumentType = entity.DocumentType,
                Level = entity.Level,
                MinAmount = entity.MinAmount,
                MaxAmount = entity.MaxAmount,
                Role = new ReferenceDataResponse<string>
                {
                    Id = entity.RoleId
                },
                Department = entity.Department != null
                    ? new ReferenceDataResponse<int>
                    {
                        Id = entity.Department.Id,
                        Name = entity.Department.Name
                    }
                    : null
            };

            return response;
        }
    }

    extension(ApprovalLevelRequest request)
    {
        public ApprovalLevel ToEntity()
        {
            ApprovalLevel entity = new(request.Name, request.Level, request.MinAmount, request.MaxAmount, 
                request.RoleId, request.DepartmentId, request.DocumentType);

            return entity;
        }
    }

    extension(ApprovalLevel entity)
    {
        public void Update(ApprovalLevelRequest request)
        {
            entity.Update(request.Name, request.Level, request.MinAmount, request.MaxAmount,
                request.RoleId, request.DepartmentId, request.DocumentType);
        }
    }
}


