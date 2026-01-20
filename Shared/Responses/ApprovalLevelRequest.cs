using EdgIQ.Inventory.Shared.Enums;

namespace EdgIQ.Inventory.Shared.Responses;

public record ApprovalLevelResponse : BaseResponse
{
    public string Name { get; init; } = default!;

    public int Level { get; init; }

    public decimal MinAmount { get; init; }

    public decimal MaxAmount { get; init; }

    public ReferenceDataResponse<string> Role { get; init; } = default!;

    public ReferenceDataResponse<int>? Department { get; init; }

    public DocumentType DocumentType { get; init; }
}
