namespace EdgIQ.Inventory.WebAPI.StockAdjustments;

internal static class StockAdjustmentsApi
{
    public static RouteGroupBuilder MapStockAdjustments(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("/stock-adjustments");
        group.WithTags("Stock Adjustments");

        group.RequireAuthorization(pb => pb.RequireCurrentUser());

        // Rate limit all of the APIs
        group.RequirePerUserRateLimit();

        group.MapGet("/", async (InventoryDbContext dbContext, CurrentUser currentUser) =>
        {
            return await dbContext.StockAdjustments
            .AsNoTracking()
            .Include(sa => sa.Warehouse)
            .Include(sa => sa.StockAdjustmentItems)
            .ThenInclude(sai => sai.Bin)
            .Where(sa => sa.TenantId == currentUser.User!.TenantId)
            .Select(sa => sa.AsApiResponse())
            .ToListAsync();

        });

        group.MapGet("/{skip}/{take}", async (InventoryDbContext dbContext,
            int skip, int take, CurrentUser currentUser) =>
        {
            List<StockAdjustmentResponse> stockAdjustments = await dbContext.StockAdjustments
            .AsNoTracking()
            .Include(sa => sa.Warehouse)
            .Where(sa => sa.TenantId == currentUser.User!.TenantId)
            .Select(sa => sa.AsApiResponse())
            .Skip(skip)
            .Take(take)
            .ToListAsync();

            int stockAdjustmentsCount = await dbContext.StockAdjustments
            .AsNoTracking()
            .CountAsync(sa => sa.TenantId == currentUser.User!.TenantId);

            PaginatedResponse<StockAdjustmentResponse> response = new(stockAdjustments, stockAdjustmentsCount, skip, take);

            return response;
        });

        group.MapGet("/{id}", async Task<Results<Ok<StockAdjustmentResponse>, NotFound>>
            (InventoryDbContext dbContext, int id) =>
        {
            StockAdjustment? stockAdjustment = await dbContext.StockAdjustments
            .Include(sa => sa.Warehouse)
            .Include(sa => sa.StockAdjustmentItems)
            .ThenInclude(sai => sai.AdjustedStock)
            .Include(sa => sa.StockAdjustmentItems)
            .ThenInclude(sai => sai.Batch)
            .Include(sa => sa.StockAdjustmentItems)
            .ThenInclude(sai => sai.SerialNumber)
            .Include(sa => sa.StockAdjustmentItems)
            .ThenInclude(sai => sai.Bin)
            .FirstOrDefaultAsync(sa => sa.Id == id);

            if (stockAdjustment == null)
            {
                TypedResults.NotFound();
            }

            return TypedResults.Ok(stockAdjustment?.AsApiResponse());
        });

        group.MapPost("/", async Task<Created<StockAdjustmentResponse>> (InventoryDbContext dbContext,
            StockAdjustmentRequest request, INumberGeneratorService numberGeneratorService, CurrentUser currentUser) =>
        {
            IEnumerable<int> productIds = request.StockAdjustmentItems.Select(p => p.ProductVariantId);

            List<ProductVariant> products = await dbContext.ProductVariants
            .Include(pv => pv.Product)
            .AsNoTracking()
            .Include(pv => pv.Product)
            .Where(pv => pv.TenantId == currentUser.User!.TenantId && productIds.Contains(pv.Id))
            .ToListAsync();

            string adjustmentNumber = await numberGeneratorService.GenerateNextNumberAsync();

            StockAdjustment entity = request.ToEntity(adjustmentNumber, currentUser.Id, products);
            //entity.OnCreate(currentUser.User!.TenantId, currentUser.User!.Id);

            dbContext.Add(entity);
            await dbContext.SaveChangesAsync();

            return TypedResults.Created($"/stock-adjustments/{entity.Id}", entity.AsApiResponse());
        });

        group.MapPut("/{id}", async Task<Results<Ok, NotFound, BadRequest>> (InventoryDbContext dbContext,
            int id, StockAdjustmentRequest request, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            StockAdjustment? entity = await dbContext.StockAdjustments.FindAsync(id);

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

        group.MapPut("/{id}/approve", async Task<Results<Ok, NotFound, BadRequest>> (InventoryDbContext dbContext,
            int id, StockAdjustmentRequest request, INumberGeneratorService numberGeneratorService, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            StockAdjustment? entity = await dbContext.StockAdjustments
            .Include(sa => sa.StockAdjustmentItems)
            .FirstOrDefaultAsync(sa => sa.Id == id);

            if (entity is null)
            {
                return TypedResults.NotFound();
            }

            // Update stock levels
            foreach (var line in entity.StockAdjustmentItems)
            {
                if (line.BatchId.HasValue)
                {
                    WarehouseStock? batchStock = await dbContext.WarehouseStocks
                        .FirstOrDefaultAsync(bs => bs.BatchId == line.BatchId.Value &&
                                                  bs.WarehouseId == entity.WarehouseId);

                    batchStock?.UpdateQuantityOnHand(line.ActualQuantity);
                }
                else
                {
                    WarehouseStock? warehouseStock = await dbContext.WarehouseStocks
                        .FirstOrDefaultAsync(ws => ws.ProductVariantId == line.AdjustedStock.ProductVariantId &&
                                                  ws.WarehouseId == entity.WarehouseId);

                    if (warehouseStock != null)
                    {
                        int quantity = warehouseStock.QuantityOnHand + line.GetAdjustedQuantity();
                        warehouseStock.UpdateQuantityOnHand(quantity);
                    }
                }

                // Create stock transaction
                string transactionNumber = await numberGeneratorService.GenerateNextNumberAsync();
                StockTransaction stockTransaction = new(transactionNumber, entity.AdjustmentDate, line.AdjustedStock.ProductVariantId, line.BatchId,
                    line.SerialNumberId, entity.WarehouseId, line.BinId, line.GetAdjustedQuantity(), line.UnitPrice, TransactionType.Adjustment, 
                    DocumentType.StockAdjustment, entity.Id);
                dbContext.Add(stockTransaction);
            }

            entity.Approve(currentUser.Id);

            dbContext.Update(entity);
            await dbContext.SaveChangesAsync();

            return TypedResults.Ok();
        });

        group.MapPatch("/{id}/reject/{reason}", async Task<Results<Ok, NotFound, BadRequest>> (InventoryDbContext dbContext,
            int id, string reason, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            StockAdjustment? entity = await dbContext.StockAdjustments.FindAsync(id);

            if (entity is null)
            {
                return TypedResults.NotFound();
            }

            entity.Reject(currentUser.Id, reason);
            //entity.OnUpdate(currentUser.User!.Id);

            dbContext.Update(entity);
            await dbContext.SaveChangesAsync();

            return TypedResults.Ok();
        });

        group.MapDelete("/{id}", async Task<Results<NotFound, Ok>> (InventoryDbContext dbContext, int id) =>
        {
            int rowsAffected = await dbContext.StockAdjustments
            .Where(sa => sa.Id == id)
            .ExecuteDeleteAsync();

            return rowsAffected == 0 ? TypedResults.NotFound() : TypedResults.Ok();
        });

        return group;
    }

}
