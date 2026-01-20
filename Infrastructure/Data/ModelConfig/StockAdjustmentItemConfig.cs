using EdgIQ.Inventory.Domain.Entities.StockAdjustmentAggregate;

namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class StockAdjustmentItemConfig : BaseEntityConfig<StockAdjustmentItem>
{
    public override void Configure(EntityTypeBuilder<StockAdjustmentItem> builder)
    {
        base.Configure(builder);

        builder.Property(sa => sa.UnitPrice)
            .HasPrecision(18, 2);

        builder.Property(sa => sa.Notes)
            .HasMaxLength(255);

        builder.OwnsOne(sa => sa.AdjustedStock, s =>
        {
            s.WithOwner();

            s.Property(pq => pq.ProductName)
                .HasMaxLength(90)
                .IsRequired();

        });

        builder.Ignore(sa => sa.TenantId);
    }
}
