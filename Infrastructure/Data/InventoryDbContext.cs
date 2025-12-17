namespace EdgIQ.Inventory.Infrastructure.Data;

public class InventoryDbContext(DbContextOptions<InventoryDbContext> options) : IdentityDbContext(options)
{
    public DbSet<Bin> Bins { get; set; }

    public DbSet<Location> Locations { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<ProductCategory> ProductCategories { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DbSet<Tenant> Tenants { get; set; }

    public DbSet<Warehouse> Warehouses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Set audit fields for entities inheriting from BaseEntity
        foreach (EntityEntry<BaseEntity> entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    if (entry.Entity.TenantId == default)
                    {
                        throw new ArgumentNullException(nameof(entry.Entity.TenantId));
                    }
                    if (entry.Entity.CreatedBy == default)
                    {
                        throw new ArgumentNullException(nameof(entry.Entity.TenantId));
                    }
                    break;

                case EntityState.Modified:
                    if (entry.Entity.CreatedBy == default)
                    {
                        throw new ArgumentNullException(nameof(entry.Entity.TenantId));
                    }
                    break;
            }
        }

        int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        // ignore events if no dispatcher provided
        //if (_mediator == null)
        //{
        //    return result;
        //}

        // dispatch events only if save was successful
        BaseEntity[] entitiesWithEvents = [.. ChangeTracker.Entries<BaseEntity>()
            .Select(e => e.Entity)
            .Where(e => e.Events.Count != 0)];

        foreach (BaseEntity entity in entitiesWithEvents)
        {
            BaseDomainEvent[] events = [.. entity.Events];
            entity.Events.Clear();

            foreach (BaseDomainEvent domainEvent in events)
            {
                //await _mediator.Publish(domainEvent, cancellationToken).ConfigureAwait(false);
            }
        }

        return result;

    }
}
