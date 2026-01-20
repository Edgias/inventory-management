namespace EdgIQ.Inventory.Domain.Entities;

public class Supplier : BaseEntity
{
    public string Name { get; private set; } = default!;

    public string? ContactName { get; private set; } 

    public string? Phone { get; private set; } 

    public string? Email { get; private set; } 

    public Address? Address { get; private set; } 

    public Supplier(string name, string? contactName, string? phone, string? email, Address? address)
    {
        Name = name;
        ContactName = contactName;
        Phone = phone;
        Email = email;
        Address = address;
    }

    public void Update(string name, string? contactName, string? phone, string? email, Address? address)
    {
        Name = name;
        ContactName = contactName;
        Phone = phone;
        Email = email;
        Address = address;
    }
}
