namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class WarehouseConfig : BaseEntityConfig<Warehouse>
{
    public override void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        base.Configure(builder);

        builder.Property(w => w.Name)
            .HasMaxLength(60)
            .IsRequired();

        builder.Property(w => w.Phone)
            .HasMaxLength(20);

        builder.Property(w => w.ManagerId)
            .HasMaxLength(50);

        builder.OwnsOne(w => w.LocationAddress, a =>
        {
            a.WithOwner();

            a.Property(a => a.Street)
                .HasMaxLength(180);

            a.Property(a => a.State)
                .HasMaxLength(60);

            a.Property(a => a.Country)
                .HasMaxLength(90);

            a.Property(a => a.City)
                .HasMaxLength(100);

            a.Property(a => a.ZipCode)
                .HasMaxLength(60);
        });
    }
}
