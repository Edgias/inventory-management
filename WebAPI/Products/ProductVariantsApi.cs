namespace EdgIQ.Inventory.WebAPI.Products;

internal static class ProductVariantsApi
{
    public static RouteGroupBuilder MapProductVariants(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("/product-variants");
        group.WithTags("Product Variants");

        group.RequireAuthorization(pv => pv.RequireCurrentUser());

        // Rate limit all of the APIs
        group.RequirePerUserRateLimit();

        group.MapGet("/", async (IHttpContextAccessor httpContext, InventoryDbContext dbContext, CurrentUser currentUser) =>
        {
            return await dbContext.ProductVariants
            .Where(pv => pv.TenantId == currentUser.User!.TenantId)
            .Select(pv => pv.AsApiResponse())
            .AsNoTracking()
            .ToListAsync();

        });

        group.MapGet("/{skip}/{take}", async (IHttpContextAccessor httpContext,
            InventoryDbContext dbContext, int skip, int take, CurrentUser currentUser) =>
        {
            List<ProductVariant> productVariants = await dbContext.ProductVariants
            .Where(pv => pv.TenantId == currentUser.User!.TenantId)
            .AsNoTracking()
            .Skip(skip)
            .Take(take)
            .ToListAsync();

            int productVariantsCount = await dbContext.ProductVariants
            .AsNoTracking()
            .CountAsync(pv => pv.TenantId == currentUser.User!.TenantId);

            PaginatedResponse<ProductVariantResponse> response = new([.. productVariants.Select(pv => pv.AsApiResponse())], productVariantsCount, skip, take);

            return response;
        });

        group.MapGet("/{id}", async Task<Results<Ok<ProductVariantResponse>, NotFound>>
            (InventoryDbContext dbContext, int id) =>
        {
            return await dbContext.ProductVariants.FindAsync(id) switch
            {
                ProductVariant entity when entity is not null => TypedResults.Ok(entity.AsApiResponse()),
                _ => TypedResults.NotFound()
            };
        });

        group.MapPost("/", async Task<Created<ProductVariantResponse>> (IHttpContextAccessor httpContext,
            InventoryDbContext dbContext, ProductVariantRequest request, CurrentUser currentUser) =>
        {
            ProductVariant entity = request.ToEntity();
            //entity.OnCreate(currentUser.User!.TenantId, currentUser.User!.Id);

            dbContext.Add(entity);
            await dbContext.SaveChangesAsync();

            return TypedResults.Created($"/product-variants/{entity.Id}", entity.AsApiResponse());
        });

        group.MapPut("/{id}", async Task<Results<Ok, NotFound, BadRequest>> (IHttpContextAccessor httpContext,
            InventoryDbContext dbContext, int id, ProductVariantRequest request, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            ProductVariant? entity = await dbContext.ProductVariants.FindAsync(id);

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
            int rowsAffected = await dbContext.ProductVariants
            .Where(pv => pv.Id == id)
            .ExecuteDeleteAsync();

            return rowsAffected == 0 ? TypedResults.NotFound() : TypedResults.Ok();
        });

        return group;
    }

}
