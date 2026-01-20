using EdgIQ.Inventory.Domain.Entities.PurchaseOrderAggregate;

namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class PurchaseOrderItemConfig : BaseEntityConfig<PurchaseOrderItem>
{
    public override void Configure(EntityTypeBuilder<PurchaseOrderItem> builder)
    {
        base.Configure(builder);

        builder.Property(poi => poi.UnitPrice)
            .HasPrecision(18, 2);

        builder.OwnsOne(poi => poi.OrderedProduct, op =>
        {
            op.WithOwner();

            op.Property(pq => pq.ProductName)
                .HasMaxLength(90)
                .IsRequired();

        });

        builder.Ignore(poi => poi.TenantId);
    }
}
