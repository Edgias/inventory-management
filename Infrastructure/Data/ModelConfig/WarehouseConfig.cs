namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class WarehouseConfig : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.HasKey(w => w.Id);

        builder.Property(w => w.Id)
            .ValueGeneratedOnAdd();

        builder.Property(w => w.Name)
            .HasMaxLength(180)
            .IsRequired();
    }
}
