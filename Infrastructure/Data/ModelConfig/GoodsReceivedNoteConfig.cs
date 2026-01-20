using EdgIQ.Inventory.Domain.Entities.GoodsReceivedNoteAggregate;

namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class GoodsReceivedNoteConfig : BaseEntityConfig<GoodsReceivedNote>
{
    public override void Configure(EntityTypeBuilder<GoodsReceivedNote> builder)
    {
        base.Configure(builder);

        builder.HasIndex(grn => grn.GRNNumber)
            .IsUnique();

        builder.Property(grn => grn.GRNNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(grn => grn.ReceiverId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(grn => grn.ExternalReference)
            .HasMaxLength(50);

        builder.Property(grn => grn.Notes)
            .HasMaxLength(255);
    }
}
