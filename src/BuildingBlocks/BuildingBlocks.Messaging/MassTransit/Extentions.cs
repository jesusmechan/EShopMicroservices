using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Drawing;
using System.Net;
using System.Reflection;

namespace BuildingBlocks.Messaging.MassTransit;
public static class Extentions
{
    public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
    {
        //Implement RabbitMQ MassTransit configuration here
        
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter(); //SetKebabCaseEndpointNameFormatter() → configura que los nombres de los endpoints sean en kebab-case (ejemplo: basket-checkout-event).
            if(assembly is not null)
            {
                config.AddConsumers(assembly);
            }

            config.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
                {
                    host.Username(configuration["MessageBroker:Username"]!);
                    host.Password(configuration["MessageBroker:Password"]!);
                });
                configurator.ConfigureEndpoints(context); //ConfigureEndpoints(context) → automáticamente crea y vincula colas para cada Consumer registrado.
            });

        });




        return services;
    }
        
}
