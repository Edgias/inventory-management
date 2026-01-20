using EdgIQ.Inventory.Domain.Entities.StockAdjustmentAggregate;

namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class StockAdjustmentConfig : BaseEntityConfig<StockAdjustment>
{
    public override void Configure(EntityTypeBuilder<StockAdjustment> builder)
    {
        base.Configure(builder);

        builder.HasIndex(sa => sa.AdjustmentNumber)
            .IsUnique();

        builder.Property(sa => sa.AdjustmentNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(sa => sa.AdjusterId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(sa => sa.ApproverId)
            .HasMaxLength(50);

        builder.Property(sa => sa.ReasonDescription)
            .HasMaxLength(255);

        builder.Property(sa => sa.ReferenceDocument)
            .HasMaxLength(50);

        builder.Property(sa => sa.Notes)
            .HasMaxLength(255);
    }
}
