namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class ApprovalLevelConfig : BaseEntityConfig<ApprovalLevel>
{
    public override void Configure(EntityTypeBuilder<ApprovalLevel> builder)
    {
        base.Configure(builder);

        builder.Property(al => al.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(al => al.RoleId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(al => al.MinAmount)
            .HasPrecision(18, 2);

        builder.Property(al => al.MaxAmount)
            .HasPrecision(18, 2);
        
    }
}
