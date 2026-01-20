using EdgIQ.Inventory.Domain.Entities.RequestForQuotationAggregate;

namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class RequestForQuotationItemConfig : BaseEntityConfig<RequestForQuotationItem>
{
    public override void Configure(EntityTypeBuilder<RequestForQuotationItem> builder)
    {
        base.Configure(builder);

        builder.Property(rfqi => rfqi.Notes)
            .HasMaxLength(255);

        builder.Property(rfqi => rfqi.Specifications)
            .HasMaxLength(255);

        builder.OwnsOne(rfqi => rfqi.RequestedProduct, rp =>
        {
            rp.WithOwner();

            rp.Property(pq => pq.ProductName)
                .HasMaxLength(90)
                .IsRequired();

        });

        builder.Ignore(rfqi => rfqi.TenantId);
    }
}
