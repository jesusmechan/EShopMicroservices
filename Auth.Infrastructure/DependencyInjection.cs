namespace Auth.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SecurityConnection");

        // Add services to the container.

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        //services.AddDbContext<ApplicationDbContext>((sp, options) =>
        //{
        //    //Interceptor para actualizar los campos de auditoría y para eventos del dominio.
        //    options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
        //    options.UseSqlServer(connectionString);
        //});

        //services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

        return services;
    }
}
