using EdgIQ.Inventory.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EdgIQ.Inventory.Infrastructure;

public static class Dependencies
{
    public static IServiceCollection AddPostgreSQLServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<InventoryDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("InventoryConnection")));

        return services;
    }

}
