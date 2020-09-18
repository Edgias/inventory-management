using Edgias.Inventory.Management.ApplicationCore.Entities;
using Edgias.Inventory.Management.ApplicationCore.Events;
using Edgias.Inventory.Management.ApplicationCore.Exceptions;
using Edgias.Inventory.Management.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Edgias.Inventory.Management.Infrastructure.Data
{
    public class InventoryDbContext : DbContext
    {
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        public InventoryDbContext(DbContextOptions<InventoryDbContext> options,
            IDomainEventDispatcher domainEventDispatcher)
            : base(options)
        {
            _domainEventDispatcher = domainEventDispatcher;
        }

        public DbSet<Bin> Bins { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<Warehouse> Warehouses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                int result = await base.SaveChangesAsync(cancellationToken);

                // dispatch events only if save was successful
                BaseEntity[] entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
                    .Select(e => e.Entity)
                    .Where(e => e.Events.Any())
                    .ToArray();

                foreach (BaseEntity entity in entitiesWithEvents)
                {
                    BaseDomainEvent[] events = entity.Events.ToArray();

                    entity.Events.Clear();

                    foreach (var domainEvent in events)
                    {
                        _domainEventDispatcher.Dispatch(domainEvent);
                    }
                }

                return result;
            }

            catch (DbUpdateException e)
            {
                throw new DataStoreException(e.Message, e);
            }

        }
    }
}
