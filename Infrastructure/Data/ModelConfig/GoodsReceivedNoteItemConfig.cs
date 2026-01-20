using EdgIQ.Inventory.Domain.Entities.GoodsReceivedNoteAggregate;

namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class GoodsReceivedNoteItemConfig : BaseEntityConfig<GoodsReceivedNoteItem>
{
    public override void Configure(EntityTypeBuilder<GoodsReceivedNoteItem> builder)
    {
        base.Configure(builder);

        builder.Property(grni => grni.BatchNumber)
            .HasMaxLength(50);

        builder.Property(grni => grni.Notes)
            .HasMaxLength(255);

        builder.OwnsOne(grni => grni.ReceivedProduct, rp =>
        {
            rp.WithOwner();

            rp.Property(pq => pq.ProductName)
                .HasMaxLength(90)
                .IsRequired();

        });

        builder.Ignore(grni => grni.TenantId);
    }
}
