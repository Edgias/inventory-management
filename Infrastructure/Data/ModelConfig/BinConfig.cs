namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class BinConfig : BaseEntityConfig<Bin>
{
    public override void Configure(EntityTypeBuilder<Bin> builder)
    {
        base.Configure(builder);

        builder.Property(b => b.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(b => b.SerialNumber)
            .HasMaxLength(50);

        builder.Property(b => b.Color)
            .HasMaxLength(30);

        builder.Property(b => b.Weight)
            .HasPrecision(18, 2);
    }
}
