namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class DepartmentConfig : BaseEntityConfig<Department>
{
    public override void Configure(EntityTypeBuilder<Department> builder)
    {
        base.Configure(builder);

        builder.HasIndex(d => d.Name)
            .IsUnique();

        builder.Property(d => d.Name).
            HasMaxLength(50)
            .IsRequired();
    }
}
