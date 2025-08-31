using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordening.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetConnectionString("Database");

        // Add services to the container.
        //services.AddDbContext<ApplicationDbContext>(options =>
        //    options.UseSqlServer(settings));

        //services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        return services;
    }
}
