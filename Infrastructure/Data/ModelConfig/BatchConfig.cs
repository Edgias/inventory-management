namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class BatchConfig : BaseEntityConfig<Batch>
{
    public override void Configure(EntityTypeBuilder<Batch> builder)
    {
        base.Configure(builder);

        builder.HasIndex(b => b.BatchNumber)
            .IsUnique();

        builder.Property(b => b.BatchNumber).
           HasMaxLength(90)
           .IsRequired();

        builder.Property(b => b.SerialNumber).
           HasMaxLength(90);

        builder.Property(b => b.QualityCertificate).
           HasMaxLength(255);

        builder.Property(b => b.Notes).
           HasMaxLength(255);

        builder.Property(b => b.UnitPrice)
            .HasPrecision(18, 2);
    }
}
