using EdgIQ.Inventory.Domain.Entities.StockTransferAggregate;

namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class StockTransferConfig : BaseEntityConfig<StockTransfer>
{
    public override void Configure(EntityTypeBuilder<StockTransfer> builder)
    {
        base.Configure(builder);

        builder.HasIndex(st => st.TransferNumber)
            .IsUnique();

        builder.Property(st => st.TransferNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(st => st.RequesterId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(st => st.ApproverId)
            .HasMaxLength(50);

        builder.Property(st => st.Notes)
            .HasMaxLength(255);
    }
}
