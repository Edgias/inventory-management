namespace EdgIQ.Inventory.WebAPI.GoodsReceivedNotes;

internal static class GoodsReceivedNotesApi
{
    public static RouteGroupBuilder MapGoodsReceivedNotes(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("/goods-received-notes");
        group.WithTags("Goods Received Notes");

        group.RequireAuthorization(pb => pb.RequireCurrentUser());

        // Rate limit all of the APIs
        group.RequirePerUserRateLimit();

        group.MapGet("/", async (IHttpContextAccessor httpContext, InventoryDbContext dbContext, CurrentUser currentUser) =>
        {
            return await dbContext.GoodsReceivedNotes
            .AsNoTracking()
            .Where(grn => grn.TenantId == currentUser.User!.TenantId)
            .Include(grn => grn.Supplier)
            .Include(grn => grn.Warehouse)
            .Include(grn => grn.PurchaseOrder)
            .Include(grn => grn.GoodsReceivedNoteItems)
            .ThenInclude(grni => grni.ReceivedProduct)
            .Select(grn => grn.AsApiResponse())            
            .ToListAsync();

        });

        group.MapGet("/{skip}/{take}", async (IHttpContextAccessor httpContext, 
            InventoryDbContext dbContext, int skip, int take, CurrentUser currentUser) =>
        {
            List<GoodsReceivedNoteResponse> goodsReceivedNotes = await dbContext.GoodsReceivedNotes
            .AsNoTracking()
            .Where(grn => grn.TenantId == currentUser.User!.TenantId)
            .Include(grn => grn.Supplier)
            .Include(grn => grn.Warehouse)
            .Include(grn => grn.PurchaseOrder)
            .Include(grn => grn.GoodsReceivedNoteItems)
            .ThenInclude(grni => grni.ReceivedProduct)
            .Select(grn => grn.AsApiResponse())
            .Skip(skip)
            .Take(take)
            .ToListAsync();

            int goodsReceivedNotesCount = await dbContext.GoodsReceivedNotes
            .AsNoTracking()
            .CountAsync(q => q.TenantId == currentUser.User!.TenantId);

            PaginatedResponse<GoodsReceivedNoteResponse> response = new(goodsReceivedNotes, goodsReceivedNotesCount, skip, take);

            return response;
        });

        group.MapGet("/{id}", async Task<Results<Ok<GoodsReceivedNoteResponse>, NotFound>>
            (InventoryDbContext dbContext, int id) =>
        {
            GoodsReceivedNote? goodsReceivedNote = await dbContext.GoodsReceivedNotes
            .AsNoTracking()
            .Include(grn => grn.Supplier)
            .Include(grn => grn.Warehouse)
            .Include(grn => grn.PurchaseOrder)
            .Include(grn => grn.GoodsReceivedNoteItems)
            .ThenInclude(grni => grni.ReceivedProduct)
            .FirstOrDefaultAsync(q => q.Id == id);

            return goodsReceivedNote is not null ? TypedResults.Ok(goodsReceivedNote.AsApiResponse()) : TypedResults.NotFound();
        });

        group.MapGet("/{id}/goods-received-note-item/{itemId}", async Task<Results<Ok<GoodsReceivedNoteItemResponse>, NotFound>>
            (InventoryDbContext dbContext, int id, int itemId) =>
        {
            if(id == default)
            {
                return TypedResults.NotFound();
            }

            GoodsReceivedNote? goodsReceivedNote = await dbContext.GoodsReceivedNotes
            .AsNoTracking()
            .Include(grn => grn.GoodsReceivedNoteItems)
            .ThenInclude(grni => grni.ReceivedProduct)
            .FirstOrDefaultAsync(q => q.Id == id);

            if (goodsReceivedNote is null)
            {
                return TypedResults.NotFound();
            }

            GoodsReceivedNoteItem? goodsReceivedNoteItem = goodsReceivedNote.GoodsReceivedNoteItems.FirstOrDefault(grn => grn.Id == itemId);

            if (goodsReceivedNoteItem is null)
            {
                return TypedResults.NotFound();
            }

            return goodsReceivedNoteItem is not null ? TypedResults.Ok(goodsReceivedNoteItem.AsApiResponse()) : TypedResults.NotFound();
        });

        group.MapPost("/", async Task<Results<Created<GoodsReceivedNoteResponse>, BadRequest<string>>> (IHttpContextAccessor httpContext, 
            InventoryDbContext dbContext, GoodsReceivedNoteRequest request, INumberGeneratorService numberGeneratorService, CurrentUser currentUser) =>
        {
            PurchaseOrder? po = await dbContext.PurchaseOrders
            .Include(p => p.PurchaseOrderItems)
            .ThenInclude(l => l.OrderedProduct)
            .FirstOrDefaultAsync(p => p.Id == request.PurchaseOrderId);

            if (po == null || po.Status != ApprovalStatus.Approved)
            {
                return TypedResults.BadRequest("Purchase order not found or not approved");
            }

            IEnumerable<int> productIds = request.GoodsReceivedNoteItems.Select(p => p.ProductVariantId);

            List<ProductVariant> productVariants = await dbContext.ProductVariants
            .AsNoTracking()
            .Include(pv => pv.Product)
            .Where(p => p.TenantId == currentUser.User!.TenantId && productIds.Contains(p.Id))
            .ToListAsync();

            string goodsReceivedNoteNumber = await numberGeneratorService.GenerateNextNumberAsync();

            GoodsReceivedNote goodsReceivedNote = request.ToEntity(goodsReceivedNoteNumber, currentUser.Id, productVariants);
            //GoodsReceivedNote.OnCreate(currentUser.User!.TenantId, currentUser.User!.Id);

            foreach (GoodsReceivedNoteItem goodsReceivedNoteItem in goodsReceivedNote.GoodsReceivedNoteItems)
            {
                //goodsReceivedNoteItem.OnCreate(currentUser.User!.TenantId, currentUser.User!.Id);
                var poLine = po.PurchaseOrderItems.FirstOrDefault(l => l.Id == goodsReceivedNoteItem.PurchaseOrderItemId);
                if (poLine == null) continue;

                 // Update warehouse stock
                WarehouseStock? warehouseStock = await dbContext.WarehouseStocks
                    .FirstOrDefaultAsync(ws => ws.WarehouseId == request.WarehouseId &&
                                              ws.ProductVariantId == poLine.OrderedProduct.ProductVariantId);

                if (warehouseStock == null)
                {
                    //WarehouseStock stock = new(goodsReceivedNoteItem.ReceivedProduct.ProductVariantId, goodsReceivedNote.WarehouseId, null, null, goodsReceivedNoteItem.QuantityReceived,
                    //    0, DateTime.Now.ToUtc());

                    //dbContext.Add(stock);
                }
                else
                {
                    int quantity = warehouseStock.QuantityOnHand + goodsReceivedNoteItem.QuantityReceived;
                    warehouseStock.UpdateQuantityOnHand(quantity);
                }

                // Update bin stock if bin specified
                if (goodsReceivedNoteItem.BinId.HasValue)
                {
                    WarehouseStock? binStock = await dbContext.WarehouseStocks
                        .FirstOrDefaultAsync(bs => bs.WarehouseId == goodsReceivedNoteItem.BinId.Value &&
                                                  bs.ProductVariantId == poLine.OrderedProduct.ProductVariantId);

                    if (binStock == null)
                    {
                        WarehouseStock stock = new(goodsReceivedNoteItem.ReceivedProduct.ProductVariantId, goodsReceivedNote.WarehouseId, null,
                            goodsReceivedNoteItem.QuantityReceived, 0);

                        dbContext.Add(stock);
                    }
                    else
                    {
                        int quantity = binStock.QuantityOnHand + goodsReceivedNoteItem.QuantityReceived;
                        binStock.UpdateQuantityOnHand(quantity);
                    }
                }
            }

            dbContext.Add(goodsReceivedNote);
            await dbContext.SaveChangesAsync();

            return TypedResults.Created($"/goods-received-notes/{goodsReceivedNote.Id}", goodsReceivedNote.AsApiResponse());
        });

        group.MapPut("/{id}", async Task<Results<Ok, NotFound, BadRequest>> (IHttpContextAccessor httpContext, 
            InventoryDbContext dbContext, int id, GoodsReceivedNoteRequest request, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            GoodsReceivedNote? entity = await dbContext.GoodsReceivedNotes.FindAsync(id);

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
            InventoryDbContext dbContext, int id, GoodsReceivedNoteItemRequest request, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            GoodsReceivedNote? goodsReceivedNote = await dbContext.GoodsReceivedNotes.FindAsync(id);

            if (goodsReceivedNote is null)
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

            GoodsReceivedNoteItem GoodsReceivedNoteItem = request.ToEntity(productVariant);
            //GoodsReceivedNoteItem.OnCreate(currentUser.User!.TenantId, currentUser.User!.Id);

            goodsReceivedNote.AddGoodsReceivedNoteItem(GoodsReceivedNoteItem);

            dbContext.Update(goodsReceivedNote);
            await dbContext.SaveChangesAsync();

            return TypedResults.Ok();
        });

        group.MapPut("/{id}/edit-item/{itemId}", async Task<Results<Ok, NotFound, BadRequest>> (IHttpContextAccessor httpContext,
            InventoryDbContext dbContext, int id, int itemId, GoodsReceivedNoteItemRequest request, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            if (itemId == default)
            {
                return TypedResults.BadRequest();
            }

            GoodsReceivedNote? goodsReceivedNote = await dbContext.GoodsReceivedNotes
            .Include(grn => grn.GoodsReceivedNoteItems)
            .FirstOrDefaultAsync(q => q.Id == id);

            if (goodsReceivedNote is null)
            {
                return TypedResults.NotFound();
            }

            GoodsReceivedNoteItem? goodsReceivedNoteItem = await dbContext.GoodsReceivedNoteItems.FindAsync(itemId);

            if (goodsReceivedNoteItem is null)
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

            goodsReceivedNoteItem.Update(request, product);
            //GoodsReceivedNoteItem.OnUpdate(currentUser.User!.Id);

            //goodsReceivedNote.UpdateGoodsReceivedNoteItem(goodsReceivedNoteItem);

            dbContext.Update(goodsReceivedNote);
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

            GoodsReceivedNote? goodsReceivedNote = await dbContext.GoodsReceivedNotes.FindAsync(id);

            if (goodsReceivedNote is null)
            {
                return TypedResults.NotFound();
            }

            GoodsReceivedNoteItem? goodsReceivedNoteItem = await dbContext.GoodsReceivedNoteItems.FindAsync(itemId);

            if (goodsReceivedNoteItem is null)
            {
                return TypedResults.NotFound();
            }

            goodsReceivedNote.RemoveGoodsReceivedNoteItem(goodsReceivedNoteItem);
            //GoodsReceivedNote.OnUpdate(currentUser.User!.Id);

            dbContext.Update(goodsReceivedNote);
            await dbContext.SaveChangesAsync();

            return TypedResults.Ok();
        });

        group.MapDelete("/{id}", async Task<Results<NotFound, Ok>> (InventoryDbContext dbContext, int id) =>
        {
            int rowsAffected = await dbContext.GoodsReceivedNotes
            .Where(grn => grn.Id == id)
            .ExecuteDeleteAsync();

            return rowsAffected == 0 ? TypedResults.NotFound() : TypedResults.Ok();
        });

        return group;
    }

}

