namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class SupplierConfig : BaseEntityConfig<Supplier>
{
    public override void Configure(EntityTypeBuilder<Supplier> builder)
    {
        base.Configure(builder);

        builder.Property(s => s.Name).
            HasMaxLength(60)
            .IsRequired();

        builder.Property(s => s.ContactName)
            .HasMaxLength(100);

        builder.Property(s => s.Phone)
            .HasMaxLength(20);

        builder.Property(s => s.Email)
            .HasMaxLength(50);

        builder.OwnsOne(sa => sa.Address, a =>
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
