namespace EdgIQ.Inventory.Shared.Responses;

public record PaginatedResponse<T>(IReadOnlyList<T> Items, int TotalCount, int Page, int PageSize);
