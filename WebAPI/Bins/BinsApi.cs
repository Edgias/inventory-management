
namespace EdgIQ.Inventory.WebAPI.Bins;

internal static class BinsApi
{
    public static RouteGroupBuilder MapBins(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("/bins");
        group.WithTags("Bins");

        group.RequireAuthorization(pb => pb.RequireCurrentUser());

        // Rate limit all of the APIs
        group.RequirePerUserRateLimit();

        group.MapGet("/", async (InventoryDbContext dbContext, CurrentUser currentUser) =>
        {
            return await dbContext.Bins
            .AsNoTracking()
            .Where(b => b.TenantId == currentUser.User!.TenantId)
            .Select(b => b.AsApiResponse())            
            .ToListAsync();

        });

        group.MapGet("/{skip}/{take}", async (InventoryDbContext dbContext, 
            int skip, int take, CurrentUser currentUser) =>
        {
            List<BinResponse> bins = await dbContext.Bins
            .AsNoTracking()
            .Where(b => b.TenantId == currentUser.User!.TenantId)
            .Select(b => b.AsApiResponse())
            .Skip(skip)
            .Take(take)
            .ToListAsync();

            int binsCount = await dbContext.Bins
            .AsNoTracking()
            .CountAsync(b => b.TenantId == currentUser.User!.TenantId);

            PaginatedResponse<BinResponse> response = new(bins, binsCount, skip, take);
            
            return response;
        });

        group.MapGet("/{id}", async Task<Results<Ok<BinResponse>, NotFound>>
            (InventoryDbContext dbContext, int id) =>
        {
            return await dbContext.Bins.FindAsync(id) switch
            {
                Bin entity when entity is not null => TypedResults.Ok(entity.AsApiResponse()),
                _ => TypedResults.NotFound()
            };
        });

        group.MapPost("/", async Task<Created<BinResponse>> (InventoryDbContext dbContext, 
            BinRequest request, CurrentUser currentUser) =>
        {
            Bin entity = request.ToEntity();
            //entity.OnCreate(currentUser.User!.TenantId, currentUser.User!.Id);

            dbContext.Add(entity);
            await dbContext.SaveChangesAsync();

            return TypedResults.Created($"/bins/{entity.Id}", entity.AsApiResponse());
        });

        group.MapPut("/{id}", async Task<Results<Ok, NotFound, BadRequest>> (InventoryDbContext dbContext, 
            int id, BinRequest request, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            Bin? entity = await dbContext.Bins.FindAsync(id);

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
            int rowsAffected = await dbContext.Bins
            .Where(b => b.Id == id)
            .ExecuteDeleteAsync();

            return rowsAffected == 0 ? TypedResults.NotFound() : TypedResults.Ok();
        });

        return group;
    }

}
