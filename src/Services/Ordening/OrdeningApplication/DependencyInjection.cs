using Microsoft.Extensions.DependencyInjection;


namespace Ordening.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        //services.AddMediatR(cfg =>
        //{
        //    cfg.RegisterServicesFromAssembly(assembly.GetExecutingAssembly());
        //});
        return services;
    }
}
