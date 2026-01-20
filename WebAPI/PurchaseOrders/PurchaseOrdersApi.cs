
namespace EdgIQ.Inventory.WebAPI.PurchaseOrders;

internal static class PurchaseOrdersApi
{
    public static RouteGroupBuilder MapPurchaseOrders(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("/purchase-orders");
        group.WithTags("Purchase Orders");

        group.RequireAuthorization(pb => pb.RequireCurrentUser());

        // Rate limit all of the APIs
        group.RequirePerUserRateLimit();

        group.MapGet("/", async (IHttpContextAccessor httpContext, InventoryDbContext dbContext, CurrentUser currentUser) =>
        {
            return await dbContext.PurchaseOrders
            .AsNoTracking()
            .Where(po => po.TenantId == currentUser.User!.TenantId)
            .Include(po => po.Quotation)
            .Include(po => po.Warehouse)
            .Include(po => po.Supplier)
            .Include(po => po.PurchaseOrderItems)
            .ThenInclude(poi => poi.OrderedProduct)
            .Select(po => po.AsApiResponse())
            .ToListAsync();

        });

        group.MapGet("/{skip}/{take}", async (IHttpContextAccessor httpContext, 
            InventoryDbContext dbContext, int skip, int take, CurrentUser currentUser) =>
        {
            List<PurchaseOrderResponse> purchaseOrders = await dbContext.PurchaseOrders
            .AsNoTracking()
            .Where(po => po.TenantId == currentUser.User!.TenantId)
            .Include(po => po.Quotation)
            .Include(po => po.Warehouse)
            .Include(po => po.Supplier)
            .Include(po => po.PurchaseOrderItems)
            .ThenInclude(poi => poi.OrderedProduct)
            .Select(po => po.AsApiResponse())
            .Skip(skip)
            .Take(take)
            .ToListAsync();

            int purchaseOrdersCount = await dbContext.PurchaseOrders
            .AsNoTracking()
            .CountAsync(po => po.TenantId == currentUser.User!.TenantId);

            PaginatedResponse<PurchaseOrderResponse> response = new(purchaseOrders, purchaseOrdersCount, skip, take);

            return response;
        });

        group.MapGet("/{id}", async Task<Results<Ok<PurchaseOrderResponse>, NotFound>>
            (InventoryDbContext dbContext, int id) =>
        {
            PurchaseOrder? purchaseOrder = await dbContext.PurchaseOrders
            .AsNoTracking()
            .Include(po => po.Quotation)
            .Include(po => po.Warehouse)
            .Include(po => po.Supplier)
            .Include(po => po.PurchaseOrderItems)
            .ThenInclude(poi => poi.OrderedProduct)
            .FirstOrDefaultAsync(po => po.Id == id);

            return purchaseOrder is not null ? TypedResults.Ok(purchaseOrder.AsApiResponse()) : TypedResults.NotFound();
        });

        group.MapGet("/{id}/purchase-order-item/{itemId}", async Task<Results<Ok<PurchaseOrderItemResponse>, NotFound>>
            (InventoryDbContext dbContext, int id, int itemId) =>
        {
            if(id == default)
            {
                return TypedResults.NotFound();
            }

            PurchaseOrder? purchaseOrder = await dbContext.PurchaseOrders
            .AsNoTracking()
            .Include(po => po.PurchaseOrderItems)
            .ThenInclude(poi => poi.OrderedProduct)
            .FirstOrDefaultAsync(po => po.Id == id);

            if (purchaseOrder is null)
            {
                return TypedResults.NotFound();
            }

            PurchaseOrderItem? purchaseOrderItem = purchaseOrder.PurchaseOrderItems.FirstOrDefault(q => q.Id == itemId);

            if (purchaseOrderItem is null)
            {
                return TypedResults.NotFound();
            }

            return purchaseOrderItem is not null ? TypedResults.Ok(purchaseOrderItem.AsApiResponse()) : TypedResults.NotFound();
        });

        group.MapPost("/", async Task<Results<Created<PurchaseOrderResponse>, BadRequest>> (IHttpContextAccessor httpContext, 
            InventoryDbContext dbContext, PurchaseOrderRequest request, INumberGeneratorService numberGeneratorService, CurrentUser currentUser) =>
        {
            IEnumerable<int> productIds = request.PurchaseOrderItems.Select(p => p.ProductVariantId);

            List<ProductVariant> products = await dbContext.ProductVariants
            .AsNoTracking()
            .Include(pv => pv.Product)
            .Where(p => p.TenantId == currentUser.User!.TenantId && productIds.Contains(p.Id))
            .ToListAsync();

            string purchaseOrderNumber = await numberGeneratorService.GenerateNextNumberAsync();

            PurchaseOrder purchaseOrder = request.ToEntity(purchaseOrderNumber, products);
            //PurchaseOrder.OnCreate(currentUser.User!.TenantId, currentUser.User!.Id);

            //foreach (PurchaseOrderItem PurchaseOrderItem in PurchaseOrder.PurchaseOrderItems)
            //{
            //    PurchaseOrderItem.OnCreate(currentUser.User!.TenantId, currentUser.User!.Id);
            //}

            dbContext.Add(purchaseOrder);
            await dbContext.SaveChangesAsync();

            return TypedResults.Created($"/purchase-orders/{purchaseOrder.Id}", purchaseOrder.AsApiResponse());
        });

        group.MapPut("/{id}", async Task<Results<Ok, NotFound, BadRequest>> (IHttpContextAccessor httpContext, 
            InventoryDbContext dbContext, int id, PurchaseOrderRequest request, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            PurchaseOrder? entity = await dbContext.PurchaseOrders.FindAsync(id);

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
            InventoryDbContext dbContext, int id, PurchaseOrderItemRequest request, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            PurchaseOrder? purchaseOrder = await dbContext.PurchaseOrders.FindAsync(id);

            if (purchaseOrder is null)
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

            PurchaseOrderItem purchaseOrderItem = request.ToEntity(productVariant);
            //PurchaseOrderItem.OnCreate(currentUser.User!.TenantId, currentUser.User!.Id);

            purchaseOrder.AddPurchaseOrderItem(purchaseOrderItem);

            dbContext.Update(purchaseOrder);
            await dbContext.SaveChangesAsync();

            return TypedResults.Ok();
        });

        group.MapPut("/{id}/edit-item/{itemId}", async Task<Results<Ok, NotFound, BadRequest>> (IHttpContextAccessor httpContext,
            InventoryDbContext dbContext, int id, int itemId, PurchaseOrderItemRequest request, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            if (itemId == default)
            {
                return TypedResults.BadRequest();
            }

            PurchaseOrder? purchaseOrder = await dbContext.PurchaseOrders
            .Include(po => po.PurchaseOrderItems)
            .FirstOrDefaultAsync(q => q.Id == id);

            if (purchaseOrder is null)
            {
                return TypedResults.NotFound();
            }

            PurchaseOrderItem? purchaseOrderItem = await dbContext.PurchaseOrderItems.FindAsync(itemId);

            if (purchaseOrderItem is null)
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

            purchaseOrderItem.Update(request, product);
            //PurchaseOrderItem.OnUpdate(currentUser.User!.Id);

            purchaseOrder.UpdatePurchaseOrderItem(purchaseOrderItem);

            dbContext.Update(purchaseOrder);
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

            PurchaseOrder? purchaseOrder = await dbContext.PurchaseOrders.FindAsync(id);

            if (purchaseOrder is null)
            {
                return TypedResults.NotFound();
            }

            PurchaseOrderItem? purchaseOrderItem = await dbContext.PurchaseOrderItems.FindAsync(itemId);

            if (purchaseOrderItem is null)
            {
                return TypedResults.NotFound();
            }

            purchaseOrder.RemovePurchaseOrderItem(purchaseOrderItem);
            //PurchaseOrder.OnUpdate(currentUser.User!.Id);

            dbContext.Update(purchaseOrder);
            await dbContext.SaveChangesAsync();

            return TypedResults.Ok();
        });

        group.MapDelete("/{id}", async Task<Results<NotFound, Ok>> (InventoryDbContext dbContext, int id) =>
        {
            int rowsAffected = await dbContext.PurchaseOrders
            .Where(po => po.Id == id)
            .ExecuteDeleteAsync();

            return rowsAffected == 0 ? TypedResults.NotFound() : TypedResults.Ok();
        });

        return group;
    }

}

