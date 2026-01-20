namespace EdgIQ.Inventory.WebAPI.Products;

internal static class ProductCategoriesApi
{
    public static RouteGroupBuilder MapLeadSources(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("/product-categories");
        group.WithTags("Product Categories");

        group.RequireAuthorization(pb => pb.RequireCurrentUser());

        // Rate limit all of the APIs
        group.RequirePerUserRateLimit();

        group.MapGet("/", async (IHttpContextAccessor httpContext, InventoryDbContext dbContext, CurrentUser currentUser) =>
        {
            return await dbContext.ProductCategories
            .Where(pc => pc.TenantId == currentUser.User!.TenantId)
            .Select(pc => pc.AsApiResponse())
            .AsNoTracking()
            .ToListAsync();

        });

        group.MapGet("/{skip}/{take}", async (IHttpContextAccessor httpContext,
            InventoryDbContext dbContext, int skip, int take, CurrentUser currentUser) =>
        {
            List<ProductCategory> productCategories = await dbContext.ProductCategories
            .Where(pc => pc.TenantId == currentUser.User!.TenantId)
            .AsNoTracking()
            .Skip(skip)
            .Take(take)
            .ToListAsync();

            int productCategoriesCount = await dbContext.ProductCategories
            .AsNoTracking()
            .CountAsync(pc => pc.TenantId == currentUser.User!.TenantId);

            PaginatedResponse<ProductCategoryResponse> response = new([.. productCategories.Select(pc => pc.AsApiResponse())], productCategoriesCount, skip, take);

            return response;
        });

        group.MapGet("/{id}", async Task<Results<Ok<ProductCategoryResponse>, NotFound>>
            (InventoryDbContext dbContext, int id) =>
        {
            return await dbContext.ProductCategories.FindAsync(id) switch
            {
                ProductCategory entity when entity is not null => TypedResults.Ok(entity.AsApiResponse()),
                _ => TypedResults.NotFound()
            };
        });

        group.MapPost("/", async Task<Created<ProductCategoryResponse>> (IHttpContextAccessor httpContext,
            InventoryDbContext dbContext, ProductCategoryRequest request, CurrentUser currentUser) =>
        {
            ProductCategory entity = request.ToEntity();
            //entity.OnCreate(currentUser.User!.TenantId, currentUser.User!.Id);

            dbContext.Add(entity);
            await dbContext.SaveChangesAsync();

            return TypedResults.Created($"/product-categories/{entity.Id}", entity.AsApiResponse());
        });

        group.MapPut("/{id}", async Task<Results<Ok, NotFound, BadRequest>> (IHttpContextAccessor httpContext,
            InventoryDbContext dbContext, int id, ProductCategoryRequest request, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            ProductCategory? entity = await dbContext.ProductCategories.FindAsync(id);

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
            int rowsAffected = await dbContext.ProductCategories
            .Where(pc => pc.Id == id)
            .ExecuteDeleteAsync();

            return rowsAffected == 0 ? TypedResults.NotFound() : TypedResults.Ok();
        });

        return group;
    }

}
