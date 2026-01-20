namespace EdgIQ.Inventory.WebAPI.Batches;

internal static class BatchesApi
{
    public static RouteGroupBuilder MapBatches(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("/batches");
        group.WithTags("Batches");

        group.RequireAuthorization(pb => pb.RequireCurrentUser());

        // Rate limit all of the APIs
        group.RequirePerUserRateLimit();

        group.MapGet("/", async (InventoryDbContext dbContext, CurrentUser currentUser) =>
        {
            return await dbContext.Batches
            .AsNoTracking()
            .Include(b => b.ProductVariant)
            .ThenInclude(pv => pv.Product)
            .Include(b => b.Supplier)
            .Include(b => b.BatchStocks)
            .ThenInclude(bs => bs.Warehouse)
            .Include(b => b.SerialNumbers)
            .Where(b => b.TenantId == currentUser.User!.TenantId)
            .Select(b => b.AsApiResponse())
            .ToListAsync();

        });

        group.MapGet("/{skip}/{take}", async (InventoryDbContext dbContext,
            int skip, int take, CurrentUser currentUser) =>
        {
            List<BatchResponse> Batches = await dbContext.Batches
            .AsNoTracking()
            .Include(b => b.ProductVariant)
            .ThenInclude(pv => pv.Product)
            .Include(b => b.Supplier)
            .Include(b => b.BatchStocks)
            .ThenInclude(bs => bs.Warehouse)
            .Include(b => b.SerialNumbers)
            .Where(b => b.TenantId == currentUser.User!.TenantId)
            .Select(b => b.AsApiResponse())
            .Skip(skip)
            .Take(take)
            .ToListAsync();

            int BatchesCount = await dbContext.Batches
            .AsNoTracking()
            .CountAsync(b => b.TenantId == currentUser.User!.TenantId);

            PaginatedResponse<BatchResponse> response = new(Batches, BatchesCount, skip, take);

            return response;
        });

        group.MapGet("/products/{productVariantId}/batches", async (InventoryDbContext dbContext,
            int productVariantId, CurrentUser currentUser) =>
        {
            List<BatchResponse> batches = await dbContext.Batches
            .AsNoTracking()
            .Include(b => b.ProductVariant)
            .ThenInclude(pv => pv.Product)
            .Include(b => b.Supplier)
            .Include(b => b.BatchStocks)
            .ThenInclude(bs => bs.Warehouse)
            .Include(b => b.SerialNumbers)
            .Where(b => b.TenantId == currentUser.User!.TenantId && 
                       b.Status == BatchStatus.Active &&
                       b.ProductVariantId == productVariantId)
            .Select(b => b.AsApiResponse())
            .OrderBy(b => b.ExpiryDate ?? DateTime.MaxValue)
            .ToListAsync();

            return TypedResults.Ok(batches);
        });

        group.MapGet("/warehouses/{warehouseId}/expiring/{daysAhead}", async (InventoryDbContext dbContext,
            int warehouseId, int daysAhead, CurrentUser currentUser) =>
        {
            var cutOffDate = DateTime.UtcNow.AddDays(daysAhead);

            List<BatchResponse> batches = await dbContext.Batches
            .AsNoTracking()
            .Include(b => b.ProductVariant)
            .ThenInclude(pv => pv.Product)
            .Include(b => b.Supplier)
            .Include(b => b.BatchStocks)
            .ThenInclude(bs => bs.Warehouse)
            .Include(b => b.SerialNumbers)
            .Where(b => b.TenantId == currentUser.User!.TenantId && b.ExpiryDate.HasValue &&
                       b.ExpiryDate.Value <= cutOffDate &&
                       b.Status == BatchStatus.Active &&
                       b.BatchStocks.Any(bs => bs.WarehouseId == warehouseId && (bs.QuantityOnHand - bs.QuantityReserved) > 0))
            .Select(b => b.AsApiResponse())
            .OrderBy(b => b.ExpiryDate)
            .ToListAsync();

            return TypedResults.Ok(batches);
        });

        group.MapGet("/{id}", async Task<Results<Ok<BatchResponse>, NotFound>>
            (InventoryDbContext dbContext, int id) =>
        {
            return await dbContext.Batches.FindAsync(id) switch
            {
                Batch entity when entity is not null => TypedResults.Ok(entity.AsApiResponse()),
                _ => TypedResults.NotFound()
            };
        });

        group.MapPost("/", async Task<Created<BatchResponse>> (InventoryDbContext dbContext,
            BatchRequest request, CurrentUser currentUser) =>
        {
            Batch entity = request.ToEntity();
            //entity.OnCreate(currentUser.User!.TenantId, currentUser.User!.Id);

            dbContext.Add(entity);

            if (entity.ExpiryDate.HasValue)
            {
                ProductVariant? variant = await dbContext.ProductVariants.FindAsync(request.ProductVariantId);

                if (variant?.ExpiryAlertDays > 0)
                {
                    DateTime alertDate = entity.ExpiryDate.Value.AddDays(-variant.ExpiryAlertDays.Value);

                    if (alertDate > DateTime.UtcNow)
                    {
                        AlertLevel alertLevel = variant.ExpiryAlertDays.Value <= 7 ? AlertLevel.Critical :
                                        variant.ExpiryAlertDays.Value <= 30 ? AlertLevel.Warning : AlertLevel.Info;

                        ExpiryAlert expiryAlert = new(entity.Id, alertDate, variant.ExpiryAlertDays.Value, alertLevel);

                        entity.AddExpiryAlert(expiryAlert);
                    }
                }
                
            }

            await dbContext.SaveChangesAsync();

            return TypedResults.Created($"/batches/{entity.Id}", entity.AsApiResponse());
        });

        group.MapPut("/{id}", async Task<Results<Ok, NotFound, BadRequest>> (InventoryDbContext dbContext,
            int id, BatchRequest request, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            Batch? entity = await dbContext.Batches.FindAsync(id);

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

        group.MapPatch("/{id}/update-status/{status}", async Task<Results<Ok, NotFound, BadRequest>> (InventoryDbContext dbContext,
            int id, BatchStatus status, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            Batch? entity = await dbContext.Batches.FindAsync(id);

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

        group.MapDelete("/{id}", async Task<Results<NotFound, Ok>> (InventoryDbContext dbContext, int id) =>
        {
            int rowsAffected = await dbContext.Batches
            .Where(b => b.Id == id)
            .ExecuteDeleteAsync();

            return rowsAffected == 0 ? TypedResults.NotFound() : TypedResults.Ok();
        });

        return group;
    }

}

