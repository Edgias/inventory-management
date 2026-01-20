using EdgIQ.Inventory.Domain.Entities.PurchaseRequisitionAggregate;

namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class PurchaseRequisitionItemApprovalConfig : BaseEntityConfig<PurchaseRequisitionItemApproval>
{
    public override void Configure(EntityTypeBuilder<PurchaseRequisitionItemApproval> builder)
    {
        base.Configure(builder);

        builder.Property(pria => pria.ApproverId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(pria => pria.Comments)
            .HasMaxLength(255);
    }
}
