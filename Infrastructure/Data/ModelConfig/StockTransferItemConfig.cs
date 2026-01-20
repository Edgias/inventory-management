using EdgIQ.Inventory.Domain.Entities.StockTransferAggregate;

namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class StockTransferItemConfig : BaseEntityConfig<StockTransferItem>
{
    public override void Configure(EntityTypeBuilder<StockTransferItem> builder)
    {
        base.Configure(builder);

        builder.Property(sti => sti.Notes)
            .HasMaxLength(255);

        builder.Ignore(sti => sti.TenantId);
    }
}
