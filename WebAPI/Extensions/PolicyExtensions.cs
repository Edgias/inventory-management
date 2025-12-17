using EdgIQ.Inventory.Infrastructure.Identity;
using System.Security.Claims;

namespace EdgIQ.Inventory.WebAPI.Extensions;

public static class PolicyExtensions
{
    public static void AddPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            string[] permissions = Enum.GetNames<Permission>();

            foreach (string permission in permissions)
            {
                options.AddPolicy(permission, policy => policy.RequireClaim(ClaimTypes.Role, permission));
            }

        });
    }
}
