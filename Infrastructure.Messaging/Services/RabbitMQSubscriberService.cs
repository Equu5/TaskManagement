using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Messaging.Services
{
    public class RabbitMqSubscriberService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public RabbitMqSubscriberService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var subscriptionManager = scope.ServiceProvider.GetRequiredService<RabbitMqSubscriptionManager>();
            subscriptionManager.SubscribeToEvents();

            return Task.CompletedTask;
        }
    }
}
