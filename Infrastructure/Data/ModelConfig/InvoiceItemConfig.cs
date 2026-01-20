using EdgIQ.Inventory.Domain.Entities.InvoiceAggregate;

namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class InvoiceItemConfig : BaseEntityConfig<InvoiceItem>
{
    public override void Configure(EntityTypeBuilder<InvoiceItem> builder)
    {
        base.Configure(builder);

        builder.OwnsOne(ii => ii.InvoicedProduct, ip =>
        {
            ip.WithOwner();

            ip.Property(pq => pq.ProductName)
                .HasMaxLength(90)
                .IsRequired();

        });

        builder.Property(ii => ii.UnitPrice)
            .HasPrecision(18, 2);

        builder.Ignore(ii => ii.TenantId);
    }
}
