public interface IMessageSubscriber
{
    Task BindQueueAsync(string queueName, string exchangeName, string routingKey);

    void Subscribe<T>(string queueName, Func<T, Task> messageHandler);
}
