using Edgias.Inventory.Management.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edgias.Inventory.Management.Infrastructure.Data.ModelConfiguration
{
    internal class BinConfig : IEntityTypeConfiguration<Bin>
    {
        public void Configure(EntityTypeBuilder<Bin> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
                .ValueGeneratedOnAdd();

            builder.Property(b => b.Name)
                .HasMaxLength(180)
                .IsRequired();

            builder.Property(b => b.SerialNumber)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(b => b.Color)
                .HasMaxLength(90)
                .IsRequired();

            builder.HasIndex(b => new { b.Name, b.SerialNumber })
                .IsUnique();
        }
    }
}
