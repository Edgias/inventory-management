using EdgIQ.Inventory.Domain.Entities.QuotationAggregate;

namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class QuotationConfig : BaseEntityConfig<Quotation>
{
    public override void Configure(EntityTypeBuilder<Quotation> builder)
    {
        base.Configure(builder);

        builder.HasIndex(q => q.QuotationNumber)
            .IsUnique();

        builder.Property(q => q.QuotationNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(q => q.Terms)
            .HasMaxLength(100);

    }
}
