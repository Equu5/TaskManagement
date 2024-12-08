using System.Threading.Channels;
using Core.Domain;

public class RabbitMqSubscriptionManager
{
    private readonly IMessageSubscriber _messageSubscriber;
    private readonly TaskEventHandler _taskEventHandler;

    public RabbitMqSubscriptionManager(IMessageSubscriber messageSubscriber, TaskEventHandler taskEventHandler)
    {
        _messageSubscriber = messageSubscriber;
        _taskEventHandler = taskEventHandler;
    }

    public void SubscribeToEvents()
    {
        _messageSubscriber.Subscribe<TaskUpdatedEvent>(
            "task_queue",
            async (message) =>
            {
                await _taskEventHandler.HandleTaskUpdatedEvent(message);
            });

        _messageSubscriber.Subscribe<TaskCreatedEvent>(
            "task_queue",
            async (message) =>
            {
                await _taskEventHandler.HandleTaskCreatedEvent(message);
            });
    }
}
