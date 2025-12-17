using EdgIQ.Inventory.Infrastructure.Data;
using EdgIQ.Inventory.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EdgIQ.Inventory.WebAPI.Extensions;

public static class MigrationExtensions
{
    public static async Task ApplyMigrations(this WebApplication app)
    {
        // Create a scope to get services
        using IServiceScope scope = app.Services.CreateScope();
        IServiceProvider services = scope.ServiceProvider;
        try
        {
            // Get the DbContext service
            var dbContext = services.GetRequiredService<InventoryDbContext>();

            // Get the logger service
            var logger = services.GetRequiredService<ILogger<WebApplication>>();

            logger.LogInformation("Checking for and applying database migrations...");

            // Apply any pending migrations
            dbContext.Database.Migrate();

            logger.LogInformation("Database migrations checked/applied successfully.");

            
            await dbContext.SaveChangesAsync();
            logger.LogInformation("Data seeding completed.");
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<WebApplication>>();
            logger.LogError(ex, "An error occurred while applying database migrations.");
        }
    }

    public static async Task SeedDemoUserAsync(this WebApplication app)
    {
        // Seed default user
        using IServiceScope scope = app.Services.CreateScope();
        IServiceProvider services = scope.ServiceProvider;

        // Get the logger service
        var logger = services.GetRequiredService<ILogger<WebApplication>>();

        try
        {
            logger.LogInformation("Seeding data.");

            UserManager<ApplicationUser> userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            RoleManager<IdentityRole> roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            await IdentitySeed.SeedAsync(userManager, roleManager);
        }
        catch (Exception e)
        {
            logger.LogError($"An error occurred whilst seeding data.{e.Message}");
        }

    }
}
