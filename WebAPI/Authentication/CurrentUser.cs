using EdgIQ.Inventory.Infrastructure.Identity;
using System.Security.Claims;

namespace EdgIQ.Inventory.WebAPI.Authentication;

// A scoped service that exposes the current user information
public class CurrentUser
{
    public ApplicationUser? User { get; set; }

    public ClaimsPrincipal Principal { get; set; } = default!;

    public string Id => Principal.FindFirstValue(ClaimTypes.NameIdentifier)!;

    public bool IsAdmin => Principal.IsInRole("admin");
}