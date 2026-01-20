namespace EdgIQ.Inventory.Infrastructure.Data.ModelConfig;

internal class ExpiryAlertConfig : BaseEntityConfig<ExpiryAlert>
{
    public override void Configure(EntityTypeBuilder<ExpiryAlert> builder)
    {
        base.Configure(builder);

        builder.Property(ea => ea.AcknowledgerId).
            HasMaxLength(50);

        builder.Property(ea => ea.Action).
            HasMaxLength(255);

    }
}
