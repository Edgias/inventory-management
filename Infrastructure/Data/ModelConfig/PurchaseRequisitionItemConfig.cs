using EdgIQ.Inventory.Domain.Entities.PurchaseRequisitionAggregate;

namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class PurchaseRequisitionItemConfig : BaseEntityConfig<PurchaseRequisitionItem>
{
    public override void Configure(EntityTypeBuilder<PurchaseRequisitionItem> builder)
    {
        base.Configure(builder);

        builder.Property(pri => pri.UnitPrice)
            .HasPrecision(18, 2);

        builder.Property(pri => pri.Notes)
            .HasMaxLength(255);

        builder.OwnsOne(pri => pri.RequisitionedProduct, rp =>
        {
            rp.WithOwner();

            rp.Property(pq => pq.ProductName)
                .HasMaxLength(90)
                .IsRequired();

        });

        builder.Ignore(grni => grni.TenantId);
    }
}
