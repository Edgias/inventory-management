using EdgIQ.Inventory.Domain.Entities.QuotationAggregate;

namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class QuotationItemConfig : BaseEntityConfig<QuotationItem>
{
    public override void Configure(EntityTypeBuilder<QuotationItem> builder)
    {
        base.Configure(builder);

        builder.OwnsOne(qi => qi.QuotedProduct, iq =>
        {
            iq.WithOwner();

            iq.Property(pq => pq.ProductName)
                .HasMaxLength(160)
                .IsRequired();

        });

        builder.Property(qi => qi.UnitPrice)
            .HasPrecision(18, 2);

        builder.Ignore(qi => qi.TenantId);
    }
}
