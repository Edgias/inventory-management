
namespace EdgIQ.Inventory.WebAPI.ApprovalLevels;

internal static class ApprovalLevelsApi
{
    public static RouteGroupBuilder MapApprovalLevels(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("/approval-levels");
        group.WithTags("Approval Levels");

        group.RequireAuthorization(pb => pb.RequireCurrentUser());

        // Rate limit all of the APIs
        group.RequirePerUserRateLimit();

        group.MapGet("/", async (InventoryDbContext dbContext, CurrentUser currentUser) =>
        {
            return await dbContext.ApprovalLevels
            .AsNoTracking()
            .Where(al => al.TenantId == currentUser.User!.TenantId)
            .Select(al => al.AsApiResponse())            
            .ToListAsync();

        });

        group.MapGet("/document-type/{type}", async (InventoryDbContext dbContext, DocumentType type, CurrentUser currentUser) =>
        {
            return await dbContext.ApprovalLevels
            .AsNoTracking()
            .Where(al => al.TenantId == currentUser.User!.TenantId && al.DocumentType == type)
            .Select(al => al.AsApiResponse())
            .ToListAsync();

        });

        group.MapGet("/{skip}/{take}", async (InventoryDbContext dbContext, 
            int skip, int take, CurrentUser currentUser) =>
        {
            List<ApprovalLevelResponse> ApprovalLevels = await dbContext.ApprovalLevels
            .AsNoTracking()
            .Where(al => al.TenantId == currentUser.User!.TenantId)
            .Select(al => al.AsApiResponse())
            .Skip(skip)
            .Take(take)
            .ToListAsync();

            int ApprovalLevelsCount = await dbContext.ApprovalLevels
            .AsNoTracking()
            .CountAsync(al => al.TenantId == currentUser.User!.TenantId);

            PaginatedResponse<ApprovalLevelResponse> response = new(ApprovalLevels, ApprovalLevelsCount, skip, take);
            
            return response;
        });

        group.MapGet("/{id}", async Task<Results<Ok<ApprovalLevelResponse>, NotFound>>
            (InventoryDbContext dbContext, int id) =>
        {
            return await dbContext.ApprovalLevels.FindAsync(id) switch
            {
                ApprovalLevel entity when entity is not null => TypedResults.Ok(entity.AsApiResponse()),
                _ => TypedResults.NotFound()
            };
        });

        group.MapPost("/", async Task<Created<ApprovalLevelResponse>> (InventoryDbContext dbContext, 
            ApprovalLevelRequest request, CurrentUser currentUser) =>
        {
            ApprovalLevel entity = request.ToEntity();
            //entity.OnCreate(currentUser.User!.TenantId, currentUser.User!.Id);

            dbContext.Add(entity);
            await dbContext.SaveChangesAsync();

            return TypedResults.Created($"/approval-levels/{entity.Id}", entity.AsApiResponse());
        });

        group.MapPut("/{id}", async Task<Results<Ok, NotFound, BadRequest>> (InventoryDbContext dbContext, 
            int id, ApprovalLevelRequest request, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            ApprovalLevel? entity = await dbContext.ApprovalLevels.FindAsync(id);

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
            int rowsAffected = await dbContext.ApprovalLevels
            .Where(al => al.Id == id)
            .ExecuteDeleteAsync();

            return rowsAffected == 0 ? TypedResults.NotFound() : TypedResults.Ok();
        });

        return group;
    }

}
