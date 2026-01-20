using EdgIQ.Inventory.Domain.Entities.GoodsReceivedNoteAggregate;
using EdgIQ.Inventory.Domain.Entities.InvoiceAggregate;
using EdgIQ.Inventory.Domain.Entities.PurchaseOrderAggregate;
using EdgIQ.Inventory.Domain.Entities.PurchaseRequisitionAggregate;
using EdgIQ.Inventory.Domain.Entities.QuotationAggregate;
using EdgIQ.Inventory.Domain.Entities.RequestForQuotationAggregate;
using EdgIQ.Inventory.Domain.Entities.StockAdjustmentAggregate;
using EdgIQ.Inventory.Domain.Entities.StockTransferAggregate;

namespace EdgIQ.Inventory.Infrastructure.Data;

public class InventoryDbContext(DbContextOptions<InventoryDbContext> options) : IdentityDbContext(options)
{
    public DbSet<ApprovalLevel> ApprovalLevels { get; set; }

    public DbSet<Batch> Batches { get; set; }

    public DbSet<BatchStock> BatchStocks { get; set; }

    public DbSet<Bin> Bins { get; set; }

    public DbSet<BinStock> BinStocks { get; set; }

    public DbSet<Department> Departments { get; set; }

    public DbSet<ExpiryAlert> ExpiryAlerts { get; set; }

    public DbSet<GoodsReceivedNote> GoodsReceivedNotes { get; set; }

    public DbSet<GoodsReceivedNoteItem> GoodsReceivedNoteItems { get; set; }

    public DbSet<Invoice> Invoices { get; set; }

    public DbSet<InvoiceItem> InvoicesItem { get; set; }

    public DbSet<MeasurementUnit> MeasurementUnits {  get; set; }

    public DbSet<PurchaseOrder> PurchaseOrders { get; set; }

    public DbSet<PurchaseOrderApproval> PurchaseOrderApprovals { get; set; }

    public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }

    public DbSet<PurchaseOrderItemApproval> PurchaseOrderItemApprovals { get; set; }

    public DbSet<PurchaseRequisition> PurchaseRequisitions { get; set; }

    public DbSet<PurchaseRequisitionApproval> PurchaseRequisitionApprovals { get; set; }

    public DbSet<PurchaseRequisitionItem> PurchaseRequisitionItems { get; set; }

    public DbSet<PurchaseRequisitionItemApproval> PurchaseRequisitionItemApprovals { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<ProductCategory> ProductCategories { get; set; }

    public DbSet<ProductVariant> ProductVariants { get; set; }

    public DbSet<Quotation> Quotations { get; set; }

    public DbSet<QuotationItem> QuotationItems { get; set; }

    public DbSet<RequestForQuotation> RequestForQuotations { get; set; }

    public DbSet<RequestForQuotationItem> RequestForQuotationItems { get; set; }

    public DbSet<RequestForQuotationSupplier> RequestForQuotationSuppliers { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DbSet<SerialNumber> SerialNumbers {  get; set; }

    public DbSet<StockAdjustment> StockAdjustments { get; set; }

    public DbSet<StockAdjustmentItem> StockAdjustmentItems { get; set; }

    public DbSet<StockTransaction> StockTransactions { get; set; }

    public DbSet<StockTransfer> StockTransfers { get; set; }

    public DbSet<StockTransferItem> StockTransferItems { get; set; }

    public DbSet<Supplier> Suppliers { get; set; }

    public DbSet<Tax> Taxes { get; set; }

    public DbSet<Tenant> Tenants { get; set; }

    public DbSet<Warehouse> Warehouses { get; set; }

    public DbSet<WarehouseStock> WarehouseStocks { get; set; }

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
                        throw new ArgumentNullException("tenantid");
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
