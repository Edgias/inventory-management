using EdgIQ.Inventory.Domain.Entities.PurchaseOrderAggregate;

namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class PurchaseOrderItemApprovalConfig : BaseEntityConfig<PurchaseOrderItemApproval>
{
    public override void Configure(EntityTypeBuilder<PurchaseOrderItemApproval> builder)
    {
        base.Configure(builder);

        builder.Property(poia => poia.ApproverId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(poia => poia.Comments)
            .HasMaxLength(255);
    }
}
