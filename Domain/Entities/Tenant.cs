namespace EdgIQ.Inventory.Domain.Entities;

public class Tenant : BaseEntity
{
    public string? Name { get; private set; } = default!;

    public string Email { get; private set; } = default!;

    public string? PhoneNumber { get; private set; } = default!;

    public Address BillingAddress { get; private set; } = default!;

    private Tenant()
    {
    }

    public Tenant(string email)
    {
        Email = email;
    }

    public Tenant(string? name, string email)
    {
        Name = name;
        Email = email;
    }

    public Tenant(string? name, string email, string? phoneNumber, Address billingAddress)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        BillingAddress = billingAddress;
    }

    public void Update(string? name, string email, string? phoneNumber, Address billingAddress)
    {
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        BillingAddress = billingAddress;
    }
}
