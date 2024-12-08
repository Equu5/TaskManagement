using Core.Domain;
using FluentValidation;
using Infrastructure.Data;
using Infrastructure.Messaging.Services;
using MediatR;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddApplicationServices();
        services.AddDataAccess(Configuration);
        services.AddRabbitMqAsync(Configuration);
        services.AddSwaggerDocumentation();

        services.AddHostedService<RabbitMqSubscriberService>();

        services.AddValidatorsFromAssemblyContaining<CreateTaskCommandValidator>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseSwaggerDocumentation();
        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapControllers());

        using (var scope = app.ApplicationServices.CreateScope())
        {
            var messageSubscriber = scope.ServiceProvider.GetRequiredService<IMessageSubscriber>();

            messageSubscriber.BindQueueAsync("task_queue", "task_exchange", "task.status_changed").Wait();
            messageSubscriber.BindQueueAsync("task_queue", "task_exchange", "task.created").Wait();

            var subscriptionManager = scope.ServiceProvider.GetRequiredService<RabbitMqSubscriptionManager>();
            subscriptionManager.SubscribeToEvents();
        }
    }
}
