
namespace EdgIQ.Inventory.WebAPI.Quotations;

internal static class QuotationsApi
{
    public static RouteGroupBuilder MapQuotations(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("/quotations");
        group.WithTags("Quotations");

        group.RequireAuthorization(pb => pb.RequireCurrentUser());

        // Rate limit all of the APIs
        group.RequirePerUserRateLimit();

        group.MapGet("/", async (IHttpContextAccessor httpContext, InventoryDbContext dbContext, CurrentUser currentUser) =>
        {
            return await dbContext.Quotations
            .AsNoTracking()
            .Where(q => q.TenantId == currentUser.User!.TenantId)
            .Include(q => q.RequestForQuotation)
            .Include(q => q.PurchaseRequisition)
            .Include(q => q.Supplier)
            .Include(q => q.QuotationItems)
            .ThenInclude(qi => qi.QuotedProduct)
            .Select(q => q.AsApiResponse())
            .ToListAsync();

        });

        group.MapGet("/{skip}/{take}", async (IHttpContextAccessor httpContext, 
            InventoryDbContext dbContext, int skip, int take, CurrentUser currentUser) =>
        {
            List<QuotationResponse> quotations = await dbContext.Quotations
            .AsNoTracking()
            .Where(q => q.TenantId == currentUser.User!.TenantId)
            .Include(q => q.RequestForQuotation)
            .Include(q => q.PurchaseRequisition)
            .Include(q => q.Supplier)
            .Include(q => q.QuotationItems)
            .ThenInclude(qi => qi.QuotedProduct)
            .Include(q => q.QuotationItems)
            .ThenInclude(qi => qi.Tax)
            .Select(q => q.AsApiResponse())
            .Skip(skip)
            .Take(take)
            .ToListAsync();

            int quotationsCount = await dbContext.Quotations
            .AsNoTracking()
            .CountAsync(q => q.TenantId == currentUser.User!.TenantId);

            PaginatedResponse<QuotationResponse> response = new(quotations, quotationsCount, skip, take);

            return response;
        });

        group.MapGet("/{id}", async Task<Results<Ok<QuotationResponse>, NotFound>>
            (InventoryDbContext dbContext, int id) =>
        {
            Quotation? quote = await dbContext.Quotations
            .AsNoTracking()
            .Include(q => q.RequestForQuotation)
            .Include(q => q.PurchaseRequisition)
            .Include(q => q.Supplier)
            .Include(q => q.QuotationItems)
            .ThenInclude(qi => qi.QuotedProduct)
            .Include(q => q.QuotationItems)
            .ThenInclude(qi => qi.Tax)
            .FirstOrDefaultAsync(q => q.Id == id);

            return quote is not null ? TypedResults.Ok(quote.AsApiResponse()) : TypedResults.NotFound();
        });

        group.MapGet("/{id}/quote-item/{itemId}", async Task<Results<Ok<QuotationItemResponse>, NotFound>>
            (InventoryDbContext dbContext, int id, int itemId) =>
        {
            if(id == default)
            {
                return TypedResults.NotFound();
            }

            Quotation? quotation = await dbContext.Quotations
            .AsNoTracking()
            .Include(q => q.QuotationItems)
            .ThenInclude(qi => qi.QuotedProduct)
            .FirstOrDefaultAsync(q => q.Id == id);

            if (quotation is null)
            {
                return TypedResults.NotFound();
            }

            QuotationItem? quotationItem = quotation.QuotationItems.FirstOrDefault(q => q.Id == itemId);

            if (quotationItem is null)
            {
                return TypedResults.NotFound();
            }

            return quotationItem is not null ? TypedResults.Ok(quotationItem.AsApiResponse()) : TypedResults.NotFound();
        });

        group.MapPost("/", async Task<Results<Created<QuotationResponse>, BadRequest>> (IHttpContextAccessor httpContext, 
            InventoryDbContext dbContext, QuotationRequest request, INumberGeneratorService numberGeneratorService, CurrentUser currentUser) =>
        {
            IEnumerable<int> productIds = request.QuotationItems.Select(p => p.ProductVariantId);

            List<ProductVariant> products = await dbContext.ProductVariants
            .AsNoTracking()
            .Include(pv => pv.Product)
            .Where(p => p.TenantId == currentUser.User!.TenantId && productIds.Contains(p.Id))
            .ToListAsync();

            string quotationNumber = await numberGeneratorService.GenerateNextNumberAsync();

            Quotation quotation = request.ToEntity(quotationNumber, products);
            //quotation.OnCreate(currentUser.User!.TenantId, currentUser.User!.Id);

            //foreach (QuotationItem quotationItem in quotation.QuotationItems)
            //{
            //    quotationItem.OnCreate(currentUser.User!.TenantId, currentUser.User!.Id);
            //}

            dbContext.Add(quotation);
            await dbContext.SaveChangesAsync();

            return TypedResults.Created($"/quotations/{quotation.Id}", quotation.AsApiResponse());
        });

        group.MapPut("/{id}", async Task<Results<Ok, NotFound, BadRequest>> (IHttpContextAccessor httpContext, 
            InventoryDbContext dbContext, int id, QuotationRequest request, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            Quotation? entity = await dbContext.Quotations.FindAsync(id);

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
            InventoryDbContext dbContext, int id, QuotationItemRequest request, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            Quotation? quotation = await dbContext.Quotations.FindAsync(id);

            if (quotation is null)
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

            QuotationItem quotationItem = request.ToEntity(productVariant);
            //quotationItem.OnCreate(currentUser.User!.TenantId, currentUser.User!.Id);

            quotation.AddQuotationItem(quotationItem);

            dbContext.Update(quotation);
            await dbContext.SaveChangesAsync();

            return TypedResults.Ok();
        });

        group.MapPut("/{id}/edit-item/{itemId}", async Task<Results<Ok, NotFound, BadRequest>> (IHttpContextAccessor httpContext,
            InventoryDbContext dbContext, int id, int itemId, QuotationItemRequest request, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            if (itemId == default)
            {
                return TypedResults.BadRequest();
            }

            Quotation? quotation = await dbContext.Quotations
            .Include(q => q.QuotationItems)
            .FirstOrDefaultAsync(q => q.Id == id);

            if (quotation is null)
            {
                return TypedResults.NotFound();
            }

            QuotationItem? quotationItem = await dbContext.QuotationItems.FindAsync(itemId);

            if (quotationItem is null)
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

            quotationItem.Update(request, product);
            //quotationItem.OnUpdate(currentUser.User!.Id);

            quotation.UpdateQuotationItem(quotationItem);

            dbContext.Update(quotation);
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

            Quotation? quotation = await dbContext.Quotations.FindAsync(id);

            if (quotation is null)
            {
                return TypedResults.NotFound();
            }

            QuotationItem? quotationItem = await dbContext.QuotationItems.FindAsync(itemId);

            if (quotationItem is null)
            {
                return TypedResults.NotFound();
            }

            quotation.RemoveQuotationItem(quotationItem);
            //quotation.OnUpdate(currentUser.User!.Id);

            dbContext.Update(quotation);
            await dbContext.SaveChangesAsync();

            return TypedResults.Ok();
        });

        group.MapDelete("/{id}", async Task<Results<NotFound, Ok>> (InventoryDbContext dbContext, int id) =>
        {
            int rowsAffected = await dbContext.Quotations
            .Where(q => q.Id == id)
            .ExecuteDeleteAsync();

            return rowsAffected == 0 ? TypedResults.NotFound() : TypedResults.Ok();
        });

        return group;
    }

}

