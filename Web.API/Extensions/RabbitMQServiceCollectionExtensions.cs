using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

public static class RabbitMqServiceCollectionExtensions
{
    public static IServiceCollection AddRabbitMqAsync(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqConnectionString = configuration.GetSection("RabbitMq:ConnectionString").Value;

        if (string.IsNullOrEmpty(rabbitMqConnectionString))
        {
            throw new InvalidOperationException("RabbitMQ ConnectionString is missing or not configured.");
        }

        services.AddSingleton(new RabbitMqConnectionManager(rabbitMqConnectionString));

        services.AddScoped<RabbitMqSubscriptionManager>();
        services.AddScoped<TaskEventHandler>();
        services.AddScoped<IMessagePublisher, RabbitMqMessagePublisher>();
        services.AddScoped<IMessageSubscriber, RabbitMqMessageSubscriber>();
        
        return services;
    }
}
