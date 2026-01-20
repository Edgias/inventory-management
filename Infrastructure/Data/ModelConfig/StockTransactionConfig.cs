namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class StockTransactionConfig : BaseEntityConfig<StockTransaction>
{
    public override void Configure(EntityTypeBuilder<StockTransaction> builder)
    {
        base.Configure(builder);

        builder.HasIndex(st => st.TransactionNumber)
            .IsUnique();

        builder.Property(st => st.TransactionNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(st => st.UnitPrice)
            .HasPrecision(18, 2);

        builder.Property(st => st.Notes)
            .HasMaxLength(255);
    }
}
