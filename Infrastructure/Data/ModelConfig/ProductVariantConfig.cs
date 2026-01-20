namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class ProductVariantConfig : BaseEntityConfig<ProductVariant>
{
    public override void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        base.Configure(builder);

        builder.HasIndex(pv => pv.SKU)
            .IsUnique();

        builder.Property(pv => pv.SKU)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(pv => pv.Barcode)
            .HasMaxLength(90);

        builder.Property(pv => pv.Color)
            .HasMaxLength(30);

        builder.Property(pv => pv.Size)
            .HasMaxLength(20);

        builder.Property(pv => pv.UnitPrice)
            .HasPrecision(18, 2);

    }
}
