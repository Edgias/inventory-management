namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class ProductCategoryConfig : BaseEntityConfig<ProductCategory>
{
    public override void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        base.Configure(builder);

        builder.HasIndex(pc => pc.Name)
            .IsUnique();

        builder.Property(pc => pc.Name)
            .HasMaxLength(90)
            .IsRequired();

        builder.Property(pc => pc.Description)
            .HasMaxLength(256);
    }
}
