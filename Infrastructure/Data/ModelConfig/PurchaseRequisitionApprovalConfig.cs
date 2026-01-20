using EdgIQ.Inventory.Domain.Entities.PurchaseRequisitionAggregate;

namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class PurchaseRequisitionApprovalConfig : BaseEntityConfig<PurchaseRequisitionApproval>
{
    public override void Configure(EntityTypeBuilder<PurchaseRequisitionApproval> builder)
    {
        base.Configure(builder);

        builder.Property(pra => pra.ApproverId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(pra => pra.Comments)
            .HasMaxLength(255);
    }
}
