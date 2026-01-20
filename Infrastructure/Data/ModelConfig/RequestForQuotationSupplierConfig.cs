using EdgIQ.Inventory.Domain.Entities.RequestForQuotationAggregate;

namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class RequestForQuotationSupplierConfig : BaseEntityConfig<RequestForQuotationSupplier>
{
    public override void Configure(EntityTypeBuilder<RequestForQuotationSupplier> builder)
    {
        base.Configure(builder);

        builder.Property(rfqs => rfqs.Notes)
            .HasMaxLength(255);

        builder.Ignore(rfqs => rfqs.TenantId);
    }
}
