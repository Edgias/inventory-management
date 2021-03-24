using Edgias.Inventory.Management.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edgias.Inventory.Management.Infrastructure.Data.ModelConfiguration
{
    internal class LocationConfig : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.HasKey(l => l.Id);

            builder.Property(l => l.Id)
                .ValueGeneratedOnAdd();

            builder.Property(l => l.Name)
                .HasMaxLength(256)
                .IsRequired();

            builder.OwnsOne(l => l.LocationAddress, a =>
            {
                a.WithOwner();

                a.Property(a => a.ZipCode)
                    .HasMaxLength(18);

                a.Property(a => a.Street)
                    .HasMaxLength(180);

                a.Property(a => a.State)
                    .HasMaxLength(60);

                a.Property(a => a.Country)
                    .HasMaxLength(90);

                a.Property(a => a.City)
                    .HasMaxLength(100);
            });
        }
    }
}
