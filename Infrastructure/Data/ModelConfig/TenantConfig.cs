namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class TenantConfig : BaseEntityConfig<Tenant>
{
    public override void Configure(EntityTypeBuilder<Tenant> builder)
    {
        base.Configure(builder);

        builder.Property(t => t.Name).
            HasMaxLength(60);

        builder.Property(t => t.Email).
            HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.PhoneNumber).
            HasMaxLength(20);

        builder.OwnsOne(t => t.BillingAddress, a =>
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
