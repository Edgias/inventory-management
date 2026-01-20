
namespace EdgIQ.Inventory.WebAPI.RequestForQuotations;

internal static class RequestForQuotationsApi
{
    public static RouteGroupBuilder MapRequestForQuotations(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("/request-for-quotations");
        group.WithTags("Request for Quotations");

        group.RequireAuthorization(pb => pb.RequireCurrentUser());

        // Rate limit all of the APIs
        group.RequirePerUserRateLimit();

        group.MapGet("/", async (IHttpContextAccessor httpContext, InventoryDbContext dbContext, CurrentUser currentUser) =>
        {
            return await dbContext.RequestForQuotations
            .AsNoTracking()
            .Where(rfq => rfq.TenantId == currentUser.User!.TenantId)
            .Include(rfq => rfq.PurchaseRequisition)
            .Include(rfq => rfq.RequestForQuotationItems)
            .ThenInclude(rfqi => rfqi.RequestedProduct)
            .Include(rfq => rfq.RequestForQuotationItems)
            .Select(rfq => rfq.AsApiResponse())
            .ToListAsync();

        });

        group.MapGet("/{skip}/{take}", async (IHttpContextAccessor httpContext, 
            InventoryDbContext dbContext, int skip, int take, CurrentUser currentUser) =>
        {
            List<RequestForQuotationResponse> requestForQuotations = await dbContext.RequestForQuotations
            .AsNoTracking()
            .Where(rfq => rfq.TenantId == currentUser.User!.TenantId)
            .Include(rfq => rfq.PurchaseRequisition)
            .Include(rfq => rfq.RequestForQuotationItems)
            .ThenInclude(rfqi => rfqi.RequestedProduct)
            .Include(rfq => rfq.RequestForQuotationItems)
            .Select(rfq => rfq.AsApiResponse())
            .Skip(skip)
            .Take(take)
            .ToListAsync();

            int requestForQuotationsCount = await dbContext.RequestForQuotations
            .AsNoTracking()
            .CountAsync(rfq => rfq.TenantId == currentUser.User!.TenantId);

            PaginatedResponse<RequestForQuotationResponse> response = new(requestForQuotations, requestForQuotationsCount, skip, take);

            return response;
        });

        group.MapGet("/{id}", async Task<Results<Ok<RequestForQuotationResponse>, NotFound>>
            (InventoryDbContext dbContext, int id) =>
        {
            RequestForQuotation? requestForQuotation = await dbContext.RequestForQuotations
            .AsNoTracking()
            .Include(rfq => rfq.PurchaseRequisition)
            .Include(rfq => rfq.RequestForQuotationItems)
            .ThenInclude(rfqi => rfqi.RequestedProduct)
            .Include(rfq => rfq.RequestForQuotationItems)
            .Include(rfq => rfq.RequestForQuotationSuppliers)
            .ThenInclude(rs => rs.Supplier)
            .FirstOrDefaultAsync(rfq => rfq.Id == id);

            return requestForQuotation is not null ? TypedResults.Ok(requestForQuotation.AsApiResponse()) : TypedResults.NotFound();
        });

        group.MapGet("/{id}/request-for-quotation-item/{itemId}", async Task<Results<Ok<RequestForQuotationItemResponse>, NotFound>>
            (InventoryDbContext dbContext, int id, int itemId) =>
        {
            if(id == default)
            {
                return TypedResults.NotFound();
            }

            RequestForQuotation? requestForQuotation = await dbContext.RequestForQuotations
            .AsNoTracking()
            .Include(rfq => rfq.RequestForQuotationItems)
            .ThenInclude(rfqi => rfqi.RequestedProduct)
            .FirstOrDefaultAsync(rfq => rfq.Id == id);

            if (requestForQuotation is null)
            {
                return TypedResults.NotFound();
            }

            RequestForQuotationItem? requestForQuotationItem = requestForQuotation.RequestForQuotationItems.FirstOrDefault(q => q.Id == itemId);

            if (requestForQuotationItem is null)
            {
                return TypedResults.NotFound();
            }

            return requestForQuotationItem is not null ? TypedResults.Ok(requestForQuotationItem.AsApiResponse()) : TypedResults.NotFound();
        });

        group.MapPost("/", async Task<Results<Created<RequestForQuotationResponse>, BadRequest>> (IHttpContextAccessor httpContext, 
            InventoryDbContext dbContext, RequestForQuotationRequest request, INumberGeneratorService numberGeneratorService, CurrentUser currentUser) =>
        {
            IEnumerable<int> productIds = request.RequestForQuotationItems.Select(p => p.ProductVariantId);

            List<ProductVariant> products = await dbContext.ProductVariants
            .AsNoTracking()
            .Include(pv => pv.Product)
            .Where(p => p.TenantId == currentUser.User!.TenantId && productIds.Contains(p.Id))
            .ToListAsync();

            string rfqNumber = await numberGeneratorService.GenerateNextNumberAsync();

            RequestForQuotation requestForQuotation = request.ToEntity(rfqNumber, currentUser.Id, products);
            //RequestForQuotation.OnCreate(currentUser.User!.TenantId, currentUser.User!.Id);

            //foreach (RequestForQuotationItem RequestForQuotationItem in RequestForQuotation.RequestForQuotationItems)
            //{
            //    RequestForQuotationItem.OnCreate(currentUser.User!.TenantId, currentUser.User!.Id);
            //}

            dbContext.Add(requestForQuotation);
            await dbContext.SaveChangesAsync();

            return TypedResults.Created($"/request-for-quotations/{requestForQuotation.Id}", requestForQuotation.AsApiResponse());
        });

        group.MapPost("/{id}/add-supplier", async Task<Results<Ok, NotFound, BadRequest>> (IHttpContextAccessor httpContext,
            InventoryDbContext dbContext, int id, RequestForQuotationSupplierRequest request, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            RequestForQuotation? requestForQuotation = await dbContext.RequestForQuotations.FindAsync(id);

            if (requestForQuotation is null)
            {
                return TypedResults.NotFound();
            }

            RequestForQuotationSupplier requestForQuotationSupplier = request.ToEntity();
            //RequestForQuotationItem.OnCreate(currentUser.User!.TenantId, currentUser.User!.Id);

            requestForQuotation.AddRequestForQuotationSupplier(requestForQuotationSupplier);

            dbContext.Update(requestForQuotation);
            await dbContext.SaveChangesAsync();

            return TypedResults.Ok();
        });

        group.MapPut("/{id}", async Task<Results<Ok, NotFound, BadRequest>> (IHttpContextAccessor httpContext, 
            InventoryDbContext dbContext, int id, RequestForQuotationRequest request, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            RequestForQuotation? entity = await dbContext.RequestForQuotations.FindAsync(id);

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

        group.MapPatch("/{id}/close", async Task<Results<Ok, NotFound, BadRequest>> (IHttpContextAccessor httpContext,
            InventoryDbContext dbContext, int id, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            RequestForQuotation? entity = await dbContext.RequestForQuotations.FindAsync(id);

            if (entity is null)
            {
                return TypedResults.NotFound();
            }

            entity.UpdateStatus(RFQStatus.Closed);
            //entity.OnUpdate(currentUser.User!.Id);

            dbContext.Update(entity);
            await dbContext.SaveChangesAsync();

            return TypedResults.Ok();
        });

        group.MapPut("/{id}/suppliers/{supplierId}/respond/{status}", async Task<Results<Ok, NotFound, BadRequest>> (IHttpContextAccessor httpContext,
            InventoryDbContext dbContext, int id, int supplierId, RFQStatus status, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            RequestForQuotationSupplier? entity = 
            await dbContext.RequestForQuotationSuppliers.FirstOrDefaultAsync(rfqs => rfqs.SupplierId == supplierId && rfqs.RequestForQuotationId == id);

            if (entity is null)
            {
                return TypedResults.NotFound();
            }

            entity.UpdateStatus(status);
            //entity.OnUpdate(currentUser.User!.Id);

            dbContext.Update(entity);
            await dbContext.SaveChangesAsync();

            return TypedResults.Ok();
        });

        group.MapPut("/{id}/add-item", async Task<Results<Ok, NotFound, BadRequest>> (IHttpContextAccessor httpContext,
            InventoryDbContext dbContext, int id, RequestForQuotationItemRequest request, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            RequestForQuotation? requestForQuotation = await dbContext.RequestForQuotations.FindAsync(id);

            if (requestForQuotation is null)
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

            RequestForQuotationItem requestForQuotationItem = request.ToEntity(productVariant);
            //RequestForQuotationItem.OnCreate(currentUser.User!.TenantId, currentUser.User!.Id);

            requestForQuotation.AddRequestForQuotationItem(requestForQuotationItem);

            dbContext.Update(requestForQuotation);
            await dbContext.SaveChangesAsync();

            return TypedResults.Ok();
        });

        group.MapPut("/{id}/edit-item/{itemId}", async Task<Results<Ok, NotFound, BadRequest>> (IHttpContextAccessor httpContext,
            InventoryDbContext dbContext, int id, int itemId, RequestForQuotationItemRequest request, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            if (itemId == default)
            {
                return TypedResults.BadRequest();
            }

            RequestForQuotation? requestForQuotation = await dbContext.RequestForQuotations
            .Include(q => q.RequestForQuotationItems)
            .FirstOrDefaultAsync(q => q.Id == id);

            if (requestForQuotation is null)
            {
                return TypedResults.NotFound();
            }

            RequestForQuotationItem? requestForQuotationItem = await dbContext.RequestForQuotationItems.FindAsync(itemId);

            if (requestForQuotationItem is null)
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

            requestForQuotationItem.Update(request, product);
            //RequestForQuotationItem.OnUpdate(currentUser.User!.Id);

            //requestForQuotation.UpdateRequestForQuotationItem(requestForQuotationItem);

            dbContext.Update(requestForQuotation);
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

            RequestForQuotation? requestForQuotation = await dbContext.RequestForQuotations.FindAsync(id);

            if (requestForQuotation is null)
            {
                return TypedResults.NotFound();
            }

            RequestForQuotationItem? requestForQuotationItem = await dbContext.RequestForQuotationItems.FindAsync(itemId);

            if (requestForQuotationItem is null)
            {
                return TypedResults.NotFound();
            }

            requestForQuotation.RemoveRequestForQuotationItem(requestForQuotationItem);
            //RequestForQuotation.OnUpdate(currentUser.User!.Id);

            dbContext.Update(requestForQuotation);
            await dbContext.SaveChangesAsync();

            return TypedResults.Ok();
        });

        group.MapDelete("/{id}", async Task<Results<NotFound, Ok>> (InventoryDbContext dbContext, int id) =>
        {
            int rowsAffected = await dbContext.RequestForQuotations
            .Where(rfq => rfq.Id == id)
            .ExecuteDeleteAsync();

            return rowsAffected == 0 ? TypedResults.NotFound() : TypedResults.Ok();
        });

        return group;
    }

}

