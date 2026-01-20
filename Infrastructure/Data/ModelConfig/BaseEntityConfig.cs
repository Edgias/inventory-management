namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class BaseEntityConfig<T> : IEntityTypeConfiguration<T> where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(be => be);

        builder.Property(be => be.Id)
            .ValueGeneratedOnAdd();

        builder.Property(be => be.CreatedBy)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(be => be.ModifiedBy)
            .HasMaxLength(50);

        builder.Ignore(be => be.Events);
    }
}
