using Edgias.Inventory.Management.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edgias.Inventory.Management.Infrastructure.Data.ModelConfiguration
{
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

            IMutableNavigation navigation = builder.Metadata.FindNavigation(nameof(Warehouse.Bins));

            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
