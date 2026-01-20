namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class ProductConfig : BaseEntityConfig<Product>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);

        builder.Property(p => p.Name)
            .HasMaxLength(90)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasMaxLength(255);
    }
}
