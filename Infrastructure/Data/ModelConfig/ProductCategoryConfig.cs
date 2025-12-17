namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class ProductCategoryConfig : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.HasKey(pc => pc.Id);

        builder.Property(pc => pc.Id)
            .ValueGeneratedOnAdd();

        builder.Property(pc => pc.Name)
            .HasMaxLength(180)
            .IsRequired();

        builder.Property(pc => pc.Description)
            .HasMaxLength(256)
            .IsRequired();

        builder.HasIndex(pc => pc.Name)
            .IsUnique();
    }
}
