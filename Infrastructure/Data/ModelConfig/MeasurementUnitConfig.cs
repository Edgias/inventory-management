namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class MeasurementUnitConfig : BaseEntityConfig<MeasurementUnit>
{
    public override void Configure(EntityTypeBuilder<MeasurementUnit> builder)
    {
        base.Configure(builder);

        builder.HasIndex(mu => mu.Name)
            .IsUnique();

        builder.Property(mu => mu.Name).
            HasMaxLength(50)
            .IsRequired();
    }
}
