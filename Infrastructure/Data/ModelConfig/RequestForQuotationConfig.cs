using EdgIQ.Inventory.Domain.Entities.RequestForQuotationAggregate;

namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class RequestForQuotationConfig : BaseEntityConfig<RequestForQuotation>
{
    public override void Configure(EntityTypeBuilder<RequestForQuotation> builder)
    {
        base.Configure(builder);

        builder.HasIndex(rfq => rfq.RFQNumber)
            .IsUnique();

        builder.Property(rfq => rfq.RFQNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(rfq => rfq.RequesterId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(rfq => rfq.Terms)
            .HasMaxLength(100);

        builder.Property(rfq => rfq.Description)
            .HasMaxLength(255);

        builder.OwnsOne(rfq => rfq.DeliveryLocation, a =>
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
