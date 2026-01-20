namespace EdgIQ.Inventory.WebAPI.Products;

internal static class ProductsApi
{
    public static RouteGroupBuilder MapProducts(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("/products");
        group.WithTags("Products");

        group.RequireAuthorization(pb => pb.RequireCurrentUser());

        // Rate limit all of the APIs
        group.RequirePerUserRateLimit();

        group.MapGet("/", async (IHttpContextAccessor httpContext, InventoryDbContext dbContext, CurrentUser currentUser) =>
        {
            return await dbContext.Products
            .Where(p => p.TenantId == currentUser.User!.TenantId)
            .Select(p => p.AsApiResponse())
            .AsNoTracking()
            .ToListAsync();

        });

        group.MapGet("/{skip}/{take}", async (IHttpContextAccessor httpContext, 
            InventoryDbContext dbContext, int skip, int take, CurrentUser currentUser) =>
        {
            List<Product> products = await dbContext.Products
            .Where(p => p.TenantId == currentUser.User!.TenantId)
            .AsNoTracking()
            .Skip(skip)
            .Take(take)
            .ToListAsync();

            int productsCount = await dbContext.Products
            .AsNoTracking()
            .CountAsync(p => p.TenantId == currentUser.User!.TenantId);

            PaginatedResponse<ProductResponse> response = new([.. products.Select(p => p.AsApiResponse())], productsCount, skip, take);
            
            return response;
        });

        group.MapGet("/{id}", async Task<Results<Ok<ProductResponse>, NotFound>>
            (InventoryDbContext dbContext, int id) =>
        {
            return await dbContext.Products.FindAsync(id) switch
            {
                Product entity when entity is not null => TypedResults.Ok(entity.AsApiResponse()),
                _ => TypedResults.NotFound()
            };
        });

        group.MapPost("/", async Task<Created<ProductResponse>> (IHttpContextAccessor httpContext, 
            InventoryDbContext dbContext, ProductRequest request, CurrentUser currentUser) =>
        {
            Product entity = request.ToEntity();
            //entity.OnCreate(currentUser.User!.TenantId, currentUser.User!.Id);

            dbContext.Add(entity);
            await dbContext.SaveChangesAsync();

            return TypedResults.Created($"/products/{entity.Id}", entity.AsApiResponse());
        });

        group.MapPut("/{id}", async Task<Results<Ok, NotFound, BadRequest>> (IHttpContextAccessor httpContext, 
            InventoryDbContext dbContext, int id, ProductRequest request, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            Product? entity = await dbContext.Products.FindAsync(id);

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
            int rowsAffected = await dbContext.Products
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync();

            return rowsAffected == 0 ? TypedResults.NotFound() : TypedResults.Ok();
        });

        return group;
    }

}
