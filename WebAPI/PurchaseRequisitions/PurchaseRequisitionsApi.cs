
namespace EdgIQ.Inventory.WebAPI.PurchaseRequisitions;

internal static class PurchaseRequisitionsApi
{
    public static RouteGroupBuilder MapPurchaseRequisitions(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("/purchase-requisitions");
        group.WithTags("Purchase Requisitions");

        group.RequireAuthorization(pb => pb.RequireCurrentUser());

        // Rate limit all of the APIs
        group.RequirePerUserRateLimit();

        group.MapGet("/", async (IHttpContextAccessor httpContext, InventoryDbContext dbContext, CurrentUser currentUser) =>
        {
            return await dbContext.PurchaseRequisitions
            .AsNoTracking()
            .Where(pr => pr.TenantId == currentUser.User!.TenantId)
            .Include(pr => pr.Department)
            .Include(pr => pr.Warehouse)
            .Include(pr => pr.PurchaseRequisitionItems)
            .ThenInclude(ri => ri.RequisitionedProduct)
            .Select(pr => pr.AsApiResponse())
            .ToListAsync();

        });

        group.MapGet("/{skip}/{take}", async (IHttpContextAccessor httpContext, 
            InventoryDbContext dbContext, int skip, int take, CurrentUser currentUser) =>
        {
            List<PurchaseRequisitionResponse> purchaseRequisitions = await dbContext.PurchaseRequisitions
            .AsNoTracking()
            .Where(pr => pr.TenantId == currentUser.User!.TenantId)
            .Include(pr => pr.Department)
            .Include(pr => pr.Warehouse)
            .Include(pr => pr.PurchaseRequisitionItems)
            .ThenInclude(ri => ri.RequisitionedProduct)
            .Select(pr => pr.AsApiResponse())
            .Skip(skip)
            .Take(take)
            .ToListAsync();

            int purchaseRequisitionsCount = await dbContext.PurchaseRequisitions
            .AsNoTracking()
            .CountAsync(q => q.TenantId == currentUser.User!.TenantId);

            PaginatedResponse<PurchaseRequisitionResponse> response = new(purchaseRequisitions, purchaseRequisitionsCount, skip, take);

            return response;
        });

        group.MapGet("/{id}", async Task<Results<Ok<PurchaseRequisitionResponse>, NotFound>>
            (InventoryDbContext dbContext, int id) =>
        {
            PurchaseRequisition? quote = await dbContext.PurchaseRequisitions
            .AsNoTracking()
            .Include(pr => pr.Department)
            .Include(pr => pr.Warehouse)
            .Include(pr => pr.PurchaseRequisitionItems)
            .ThenInclude(ri => ri.RequisitionedProduct)
            .FirstOrDefaultAsync(q => q.Id == id);

            return quote is not null ? TypedResults.Ok(quote.AsApiResponse()) : TypedResults.NotFound();
        });

        group.MapGet("/{id}/purchase-requisition-item/{itemId}", async Task<Results<Ok<PurchaseRequisitionItemResponse>, NotFound>>
            (InventoryDbContext dbContext, int id, int itemId) =>
        {
            if(id == default)
            {
                return TypedResults.NotFound();
            }

            PurchaseRequisition? purchaseRequisition = await dbContext.PurchaseRequisitions
            .AsNoTracking()
            .Include(pr => pr.PurchaseRequisitionItems)
            .ThenInclude(ri => ri.RequisitionedProduct)
            .FirstOrDefaultAsync(pr => pr.Id == id);

            if (purchaseRequisition is null)
            {
                return TypedResults.NotFound();
            }

            PurchaseRequisitionItem? purchaseRequisitionItem = 
            purchaseRequisition.PurchaseRequisitionItems.FirstOrDefault(pr => pr.Id == itemId);

            if (purchaseRequisitionItem is null)
            {
                return TypedResults.NotFound();
            }

            return purchaseRequisitionItem is not null ? TypedResults.Ok(purchaseRequisitionItem.AsApiResponse()) : TypedResults.NotFound();
        });

        group.MapPost("/", async Task<Results<Created<PurchaseRequisitionResponse>, BadRequest>> (IHttpContextAccessor httpContext, 
            InventoryDbContext dbContext, PurchaseRequisitionRequest request, INumberGeneratorService numberGeneratorService, CurrentUser currentUser) =>
        {
            IEnumerable<int> productIds = request.PurchaseRequisitionItems.Select(p => p.ProductVariantId);

            List<ProductVariant> products = await dbContext.ProductVariants
            .AsNoTracking()
            .Include(pv => pv.Product)
            .Where(p => p.TenantId == currentUser.User!.TenantId && productIds.Contains(p.Id))
            .ToListAsync();

            string purchaseRequisitionNumber = await numberGeneratorService.GenerateNextNumberAsync();

            PurchaseRequisition purchaseRequisition = request.ToEntity(purchaseRequisitionNumber, currentUser.Id, products);
            //PurchaseRequisition.OnCreate(currentUser.User!.TenantId, currentUser.User!.Id);

            //foreach (PurchaseRequisitionItem PurchaseRequisitionItem in PurchaseRequisition.PurchaseRequisitionItems)
            //{
            //    PurchaseRequisitionItem.OnCreate(currentUser.User!.TenantId, currentUser.User!.Id);
            //}

            dbContext.Add(purchaseRequisition);
            await dbContext.SaveChangesAsync();

            return TypedResults.Created($"/purchase-requisitions/{purchaseRequisition.Id}", purchaseRequisition.AsApiResponse());
        });

        group.MapPut("/{id}", async Task<Results<Ok, NotFound, BadRequest>> (IHttpContextAccessor httpContext, 
            InventoryDbContext dbContext, int id, PurchaseRequisitionRequest request, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            PurchaseRequisition? entity = await dbContext.PurchaseRequisitions.FindAsync(id);

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

        group.MapPut("/{id}/add-item", async Task<Results<Ok, NotFound, BadRequest>> (IHttpContextAccessor httpContext,
            InventoryDbContext dbContext, int id, PurchaseRequisitionItemRequest request, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            PurchaseRequisition? purchaseRequisition = await dbContext.PurchaseRequisitions.FindAsync(id);

            if (purchaseRequisition is null)
            {
                return TypedResults.NotFound();
            }

            ProductVariant? productVariant = await dbContext.ProductVariants
            .AsNoTracking()
            .Include(pv => pv.Product)
            .FirstOrDefaultAsync(p => p.Id == request.ProductVariantId);

            if (productVariant is null)
            {
                return TypedResults.NotFound();
            }

            PurchaseRequisitionItem purchaseRequisitionItem = request.ToEntity(productVariant);
            //PurchaseRequisitionItem.OnCreate(currentUser.User!.TenantId, currentUser.User!.Id);

            purchaseRequisition.AddPurchaseRequisitionItem(purchaseRequisitionItem);

            dbContext.Update(purchaseRequisition);
            await dbContext.SaveChangesAsync();

            return TypedResults.Ok();
        });

        group.MapPut("/{id}/edit-item/{itemId}", async Task<Results<Ok, NotFound, BadRequest>> (IHttpContextAccessor httpContext,
            InventoryDbContext dbContext, int id, int itemId, PurchaseRequisitionItemRequest request, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            if (itemId == default)
            {
                return TypedResults.BadRequest();
            }

            PurchaseRequisition? purchaseRequisition = await dbContext.PurchaseRequisitions
            .Include(pr => pr.PurchaseRequisitionItems)
            .FirstOrDefaultAsync(pr => pr.Id == id);

            if (purchaseRequisition is null)
            {
                return TypedResults.NotFound();
            }

            PurchaseRequisitionItem? purchaseRequisitionItem = await dbContext.PurchaseRequisitionItems.FindAsync(itemId);

            if (purchaseRequisitionItem is null)
            {
                return TypedResults.NotFound();
            }

            ProductVariant? product = await dbContext.ProductVariants
            .AsNoTracking()
            .Include(pv => pv.Product)
            .FirstOrDefaultAsync(p => p.Id == request.ProductVariantId);

            if (product is null)
            {
                return TypedResults.NotFound();
            }

            purchaseRequisitionItem.Update(request, product);
            //PurchaseRequisitionItem.OnUpdate(currentUser.User!.Id);

            //purchaseRequisition.UpdatePurchaseRequisitionItem(purchaseRequisitionItem);

            dbContext.Update(purchaseRequisition);
            await dbContext.SaveChangesAsync();

            return TypedResults.Ok();
        });

        group.MapPut("/{id}/delete-item/{itemId}", async Task<Results<Ok, NotFound, BadRequest>> (IHttpContextAccessor httpContext,
            InventoryDbContext dbContext, int id, int itemId, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            if (itemId == default)
            {
                return TypedResults.BadRequest();
            }

            PurchaseRequisition? purchaseRequisition = await dbContext.PurchaseRequisitions.FindAsync(id);

            if (purchaseRequisition is null)
            {
                return TypedResults.NotFound();
            }

            PurchaseRequisitionItem? purchaseRequisitionItem = await dbContext.PurchaseRequisitionItems.FindAsync(itemId);

            if (purchaseRequisitionItem is null)
            {
                return TypedResults.NotFound();
            }

            purchaseRequisition.RemovePurchaseRequisitionItem(purchaseRequisitionItem);
            //PurchaseRequisition.OnUpdate(currentUser.User!.Id);

            dbContext.Update(purchaseRequisition);
            await dbContext.SaveChangesAsync();

            return TypedResults.Ok();
        });

        group.MapDelete("/{id}", async Task<Results<NotFound, Ok>> (InventoryDbContext dbContext, int id) =>
        {
            int rowsAffected = await dbContext.PurchaseRequisitions
            .Where(pr => pr.Id == id)
            .ExecuteDeleteAsync();

            return rowsAffected == 0 ? TypedResults.NotFound() : TypedResults.Ok();
        });

        return group;
    }

}

