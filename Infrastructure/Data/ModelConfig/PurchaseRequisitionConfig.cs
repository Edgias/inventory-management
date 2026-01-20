using EdgIQ.Inventory.Domain.Entities.PurchaseRequisitionAggregate;

namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class PurchaseRequisitionConfig : BaseEntityConfig<PurchaseRequisition>
{
    public override void Configure(EntityTypeBuilder<PurchaseRequisition> builder)
    {
        base.Configure(builder);

        builder.HasIndex(pr => pr.RequisitionNumber)
            .IsUnique();

        builder.Property(pr => pr.RequisitionNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(pr => pr.RequesterId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(pr => pr.Purpose)
            .HasMaxLength(255);
    }
}
