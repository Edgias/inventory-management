using EdgIQ.Inventory.Domain.Entities.PurchaseOrderAggregate;

namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class PurchaseOrderApprovalConfig : BaseEntityConfig<PurchaseOrderApproval>
{
    public override void Configure(EntityTypeBuilder<PurchaseOrderApproval> builder)
    {
        base.Configure(builder);

        builder.Property(poa => poa.ApproverId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(poa => poa.Comments)
            .HasMaxLength(255);
    }
}
