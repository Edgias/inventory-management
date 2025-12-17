using System.Security.Claims;

namespace EdgIQ.Inventory.Infrastructure.Identity;

public class IdentitySeed
{
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        IdentityRole? role = await roleManager.FindByNameAsync(IdentityConstants.ADMINISTRATORS);

        if (role == null)
        {
            await roleManager.CreateAsync(new IdentityRole(IdentityConstants.ADMINISTRATORS));

            role = await roleManager.FindByNameAsync(IdentityConstants.ADMINISTRATORS);

            if(role != null)
            {
                string[] permissions = Enum.GetNames<Permission>();

                foreach (string permission in permissions)
                {
                    Claim claim = new(ClaimTypes.Role, permission);
                    await roleManager.AddClaimAsync(role, claim);
                }
            }
        }

        string userName = "demo@konnektcrm.com";

        var user = await userManager.FindByNameAsync(userName);

        if (user == null)
        {
            // Set up default user info.
            ApplicationUser? defaultUser = new(userName, "Demo", "User")
            {
                EmailConfirmed = true
            };

            defaultUser.SetTenantId(1);
            defaultUser.SetCreatedBy("system");
            defaultUser.SetModifiedBy("system");

            // Create default user
            await userManager.CreateAsync(defaultUser, IdentityConstants.DEFAULT_PASSWORD);

            // Check if user has been created and add to the admin role
            defaultUser = await userManager.FindByNameAsync(defaultUser.UserName!);
            
            if(defaultUser != null)
            {
                await userManager.AddToRoleAsync(defaultUser, IdentityConstants.ADMINISTRATORS);
            }

            // Get role claims and add them to user.
            if(defaultUser != null && role != null)
            {
                IList<Claim> roleClaims = await roleManager.GetClaimsAsync(role);
                await userManager.AddClaimsAsync(defaultUser, roleClaims);
            }
        }
    }
}
