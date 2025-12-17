namespace EdgIQ.Inventory.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; private set; } = default!;

    public string LastName { get; private set; } = default!;

    public DateTime CreatedDate { get; private set; } = DateTime.UtcNow;

    public DateTime? ModifiedDate { get; private set; }

    public bool IsActive { get; private set; } = true;

    public string CreatedBy { get; private set; } = default!;

    public string ModifiedBy { get; private set; } = default!;

    public int TenantId { get; private set; }

    public ApplicationUser()
    {
        
    }

    public ApplicationUser(string userName, string firstName, string lastName)
    {
        UserName = userName;
        Email = userName;
        FirstName = firstName;
        LastName = lastName;
    }

    public void SetTenantId(int tenantId)
    {
        if (tenantId <= 0)
        {
            throw new ArgumentException("Tenant ID must be a positive integer.", nameof(tenantId));
        }
        TenantId = tenantId;
    }
    public void SetCreatedBy(string createdBy)
    {
        if (string.IsNullOrWhiteSpace(createdBy))
        {
            throw new ArgumentException("CreatedBy cannot be null or empty.", nameof(createdBy));
        }
        CreatedBy = createdBy;
    }
    public void SetModifiedBy(string modifiedBy)
    {
        if (string.IsNullOrWhiteSpace(modifiedBy))
        {
            throw new ArgumentException("ModifiedBy cannot be null or empty.", nameof(modifiedBy));
        }
        ModifiedBy = modifiedBy;
        ModifiedDate = DateTime.UtcNow;
    }

    public void ChangeStatus()
    {
        IsActive = !IsActive;
    }
}
