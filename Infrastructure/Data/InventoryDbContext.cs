using Edgias.Inventory.Management.ApplicationCore.Entities;
using Edgias.Inventory.Management.ApplicationCore.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Edgias.Inventory.Management.Infrastructure.Data
{
    public class InventoryDbContext : DbContext
    {
        private readonly IMediator _mediator;

        public InventoryDbContext(DbContextOptions<InventoryDbContext> options, IMediator mediator)
            : base(options)
        {
            _mediator = mediator;
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

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            // ignore events if no dispatcher provided
            if (_mediator == null) return result;

            // dispatch events only if save was successful
            BaseEntity[] entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
                .Select(e => e.Entity)
                .Where(e => e.Events.Any())
                .ToArray();

            foreach (BaseEntity entity in entitiesWithEvents)
            {
                BaseDomainEvent[] events = entity.Events.ToArray();
                entity.Events.Clear();

                foreach (BaseDomainEvent domainEvent in events)
                {
                    await _mediator.Publish(domainEvent).ConfigureAwait(false);
                }
            }

            return result;
        }
    }
}
