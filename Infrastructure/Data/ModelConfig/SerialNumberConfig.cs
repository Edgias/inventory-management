namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class SerialNumberConfig : BaseEntityConfig<SerialNumber>
{
    public override void Configure(EntityTypeBuilder<SerialNumber> builder)
    {
        base.Configure(builder);

        builder.HasIndex(sn => sn.SerialNo)
            .IsUnique();

        builder.Property(sn => sn.SerialNo)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(sn => sn.PurchaseCost)
            .HasPrecision(18, 2);

        builder.Property(sn => sn.WarrantyInformation)
            .HasMaxLength(255);

        builder.Property(sn => sn.Notes)
            .HasMaxLength(255);
    }
}
