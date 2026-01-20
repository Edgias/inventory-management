namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class TaxConfig : BaseEntityConfig<Tax>
{
    public override void Configure(EntityTypeBuilder<Tax> builder)
    {
        base.Configure(builder);

        builder.Property(t => t.Name).
            HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.Percentage)
            .HasPrecision(18, 2);
    }
}
