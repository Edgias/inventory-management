
namespace EdgIQ.Inventory.WebAPI.Departments;

internal static class DepartmentsApi
{
    public static RouteGroupBuilder MapDepartments(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("/departments");
        group.WithTags("Departments");

        group.RequireAuthorization(pb => pb.RequireCurrentUser());

        // Rate limit all of the APIs
        group.RequirePerUserRateLimit();

        group.MapGet("/", async (InventoryDbContext dbContext, CurrentUser currentUser) =>
        {
            return await dbContext.Departments
            .AsNoTracking()
            .Where(d => d.TenantId == currentUser.User!.TenantId)
            .Select(d => d.AsApiResponse())            
            .ToListAsync();

        });

        group.MapGet("/{skip}/{take}", async (InventoryDbContext dbContext, 
            int skip, int take, CurrentUser currentUser) =>
        {
            List<DepartmentResponse> departments = await dbContext.Departments
            .AsNoTracking()
            .Where(d => d.TenantId == currentUser.User!.TenantId)
            .Select(d => d.AsApiResponse())
            .Skip(skip)
            .Take(take)
            .ToListAsync();

            int departmentsCount = await dbContext.Departments
            .AsNoTracking()
            .CountAsync(d => d.TenantId == currentUser.User!.TenantId);

            PaginatedResponse<DepartmentResponse> response = new(departments, departmentsCount, skip, take);
            
            return response;
        });

        group.MapGet("/{id}", async Task<Results<Ok<DepartmentResponse>, NotFound>>
            (InventoryDbContext dbContext, int id) =>
        {
            return await dbContext.Departments.FindAsync(id) switch
            {
                Department entity when entity is not null => TypedResults.Ok(entity.AsApiResponse()),
                _ => TypedResults.NotFound()
            };
        });

        group.MapPost("/", async Task<Created<DepartmentResponse>> (InventoryDbContext dbContext, 
            DepartmentRequest request, CurrentUser currentUser) =>
        {
            Department entity = request.ToEntity();
            //entity.OnCreate(currentUser.User!.TenantId, currentUser.User!.Id);

            dbContext.Add(entity);
            await dbContext.SaveChangesAsync();

            return TypedResults.Created($"/departments/{entity.Id}", entity.AsApiResponse());
        });

        group.MapPut("/{id}", async Task<Results<Ok, NotFound, BadRequest>> (InventoryDbContext dbContext, 
            int id, DepartmentRequest request, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            Department? entity = await dbContext.Departments.FindAsync(id);

            if (entity is null)
            {
                return TypedResults.NotFound();
            }

            entity.Update(request);
            //entity.OnUpdate(currentUser.User!.Id);

            dbContext.Update(entity);
            await dbContext.SaveChangesAsync();

            return TypedResults.Ok();
        });

        group.MapDelete("/{id}", async Task<Results<NotFound, Ok>> (InventoryDbContext dbContext, int id) =>
        {
            int rowsAffected = await dbContext.Departments
            .Where(d => d.Id == id)
            .ExecuteDeleteAsync();

            return rowsAffected == 0 ? TypedResults.NotFound() : TypedResults.Ok();
        });

        return group;
    }

}
