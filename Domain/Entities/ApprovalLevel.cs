namespace EdgIQ.Inventory.Domain.Entities;

public class ApprovalLevel : BaseEntity
{
    public string Name { get; private set; }

    public int Level { get; private set; }

    public decimal MinAmount { get; private set; }

    public decimal MaxAmount { get; private set; }

    public string RoleId { get; private set; }

    public int? DepartmentId { get; private set; }
    
    public Department? Department { get; private set; }

    public DocumentType DocumentType { get; private set; }

    public ApprovalLevel(string name, int level, decimal minimalAmount, decimal maxAmount, string roleId, int? departmentId, DocumentType documentType)
    {
        Name = name;
        Level = level;
        MinAmount = minimalAmount;
        MaxAmount = maxAmount;
        RoleId = roleId;
        DepartmentId = departmentId;
        DocumentType = documentType;
    }

    public void Update(string name, int level, decimal minimalAmount, decimal maxAmount, string roleId, int? departmentId, DocumentType documentType)
    {
        Name = name;
        Level = level;
        MinAmount = minimalAmount;
        MaxAmount = maxAmount;
        RoleId = roleId;
        DepartmentId = departmentId;
        DocumentType = documentType;
    }

}
