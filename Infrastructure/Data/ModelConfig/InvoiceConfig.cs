using EdgIQ.Inventory.Domain.Entities.InvoiceAggregate;

namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class InvoiceConfig : BaseEntityConfig<Invoice>
{
    public override void Configure(EntityTypeBuilder<Invoice> builder)
    {
        base.Configure(builder);

        builder.HasIndex(i => i.InvoiceNumber)
            .IsUnique();

        builder.Property(i => i.InvoiceNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(i => i.SupplierInvoiceNumber)
            .HasMaxLength(50);

        builder.Property(i => i.PaymentTerms)
            .HasMaxLength(100);

    }
}
