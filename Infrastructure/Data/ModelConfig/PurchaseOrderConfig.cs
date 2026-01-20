using EdgIQ.Inventory.Domain.Entities.PurchaseOrderAggregate;

namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class PurchaseOrderConfig : BaseEntityConfig<PurchaseOrder>
{
    public override void Configure(EntityTypeBuilder<PurchaseOrder> builder)
    {
        base.Configure(builder);

        builder.HasIndex(po => po.PONumber)
            .IsUnique();

        builder.Property(po => po.PONumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(po => po.Terms)
            .HasMaxLength(100);

        builder.OwnsOne(po => po.ShippingAddress, a =>
        {
            a.WithOwner();

            a.Property(a => a.Street)
                .HasMaxLength(180);

            a.Property(a => a.State)
                .HasMaxLength(60);

            a.Property(a => a.Country)
                .HasMaxLength(90);

            a.Property(a => a.City)
                .HasMaxLength(100);

            a.Property(a => a.ZipCode)
                .HasMaxLength(60);
        });
    }
}
