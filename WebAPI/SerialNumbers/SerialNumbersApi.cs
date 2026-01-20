namespace EdgIQ.Inventory.WebAPI.SerialNumbers;

internal static class SerialNumbersApi
{
    public static RouteGroupBuilder MapLeadSources(this IEndpointRouteBuilder builder)
    {
        RouteGroupBuilder group = builder.MapGroup("/serial-numbers");
        group.WithTags("Serial Numbers");

        group.RequireAuthorization(pb => pb.RequireCurrentUser());

        // Rate limit all of the APIs
        group.RequirePerUserRateLimit();

        group.MapGet("/", async (InventoryDbContext dbContext, CurrentUser currentUser) =>
        {
            return await dbContext.SerialNumbers
            .AsNoTracking()
            .Include(sn => sn.Batch)
            .Include(sn => sn.Bin)
            .Include(sn => sn.Warehouse)
            .Include(sn => sn.ProductVariant)
            .ThenInclude(sn => sn.Product)
            .Where(sn => sn.TenantId == currentUser.User!.TenantId)
            .Select(sn => sn.AsApiResponse())
            .ToListAsync();

        });

        group.MapGet("/{skip}/{take}", async (InventoryDbContext dbContext, int skip, int take, 
            CurrentUser currentUser) =>
        {
            List<SerialNumberResponse> serialNumbers = await dbContext.SerialNumbers
            .AsNoTracking()
            .Include(sn => sn.Batch)
            .Include(sn => sn.Bin)
            .Include(sn => sn.Warehouse)
            .Include(sn => sn.ProductVariant)
            .ThenInclude(sn => sn.Product)
            .Where(sn => sn.TenantId == currentUser.User!.TenantId)
            .Select(sn => sn.AsApiResponse())
            .Skip(skip)
            .Take(take)
            .ToListAsync();

            int serialNumbersCount = await dbContext.SerialNumbers
            .AsNoTracking()
            .CountAsync(sn => sn.TenantId == currentUser.User!.TenantId);

            PaginatedResponse<SerialNumberResponse> response = new(serialNumbers, serialNumbersCount, skip, take);
            
            return response;
        });

        group.MapGet("/products/{productVariantId}/warehouses/{warehouseId}", async (InventoryDbContext dbContext, 
            int productVariantId, int warehouseId, CurrentUser currentUser) =>
        {
            List<SerialNumberResponse> serialNumbers = await dbContext.SerialNumbers
            .AsNoTracking()
            .Include(sn => sn.Batch)
            .Include(sn => sn.Bin)
            .Include(sn => sn.Warehouse)
            .Include(sn => sn.ProductVariant)
            .ThenInclude(sn => sn.Product)
            .Where(sn => sn.TenantId == currentUser.User!.TenantId && 
            sn.ProductVariantId == productVariantId && 
            sn.WarehouseId == warehouseId && 
            sn.Status == SerialNumberStatus.InStock)
            .Select(sn => sn.AsApiResponse())
            .ToListAsync();

            return TypedResults.Ok(serialNumbers);
        });

        group.MapGet("/{id}", async Task<Results<Ok<SerialNumberResponse>, NotFound>>
            (InventoryDbContext dbContext, int id) =>
        {
            return await dbContext.SerialNumbers.FindAsync(id) switch
            {
                SerialNumber entity when entity is not null => TypedResults.Ok(entity.AsApiResponse()),
                _ => TypedResults.NotFound()
            };
        });

        group.MapPost("/", async Task<Created<SerialNumberResponse>> (InventoryDbContext dbContext, 
            SerialNumberRequest request, CurrentUser currentUser) =>
        {
            SerialNumber entity = request.ToEntity();
            //entity.OnCreate(currentUser.User!.TenantId, currentUser.User!.Id);

            dbContext.Add(entity);
            await dbContext.SaveChangesAsync();

            return TypedResults.Created($"/serial-numbers/{entity.Id}", entity.AsApiResponse());
        });

        group.MapPut("/{id}", async Task<Results<Ok, NotFound, BadRequest>> (InventoryDbContext dbContext, 
            int id, SerialNumberRequest request, CurrentUser currentUser) =>
        {
            if (id == default)
            {
                return TypedResults.BadRequest();
            }

            SerialNumber? entity = await dbContext.SerialNumbers.FindAsync(id);

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
            int id, SerialNumberStatus status, CurrentUser currentUser) =>
        {
            if (id == 0)
            {
                return TypedResults.BadRequest();
            }

            SerialNumber? entity = await dbContext.SerialNumbers.FindAsync(id);

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
            int rowsAffected = await dbContext.SerialNumbers
            .Where(sn => sn.Id == id)
            .ExecuteDeleteAsync();

            return rowsAffected == 0 ? TypedResults.NotFound() : TypedResults.Ok();
        });

        return group;
    }

}
