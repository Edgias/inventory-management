using Edgias.Inventory.Management.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edgias.Inventory.Management.Infrastructure.Data.ModelConfiguration
{
    internal class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Name)
                .HasMaxLength(180)
                .IsRequired();

            builder.Property(p => p.ProductCode)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasMaxLength(256)
                .IsRequired();

            builder.HasIndex(p => new { p.Name, p.ProductCode })
                .IsUnique();
        }
    }
}
