using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

public class RabbitMqMessageSubscriber : IMessageSubscriber
{
    private readonly RabbitMqConnectionManager _connectionManager;

    public RabbitMqMessageSubscriber(RabbitMqConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
    }

    public async Task BindQueueAsync(string queueName, string exchangeName, string routingKey)
    {
        var connection = await _connectionManager.GetConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Topic, durable: true);
        await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false);
        await channel.QueueBindAsync(queue: queueName, exchange: exchangeName, routingKey: routingKey);

        Console.WriteLine($"Queue '{queueName}' bound to exchange '{exchangeName}' with routing key '{routingKey}'.");
    }

    public void Subscribe<T>(string queueName, Func<T, Task> messageHandler)
    {
        _ = Task.Run(async () =>
        {
            var connection = await _connectionManager.GetConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var jsonMessage = Encoding.UTF8.GetString(body);
                    var message = JsonSerializer.Deserialize<T>(jsonMessage);

                    if (message != null)
                    {
                        await messageHandler(message);
                    }

                    await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                }
                finally
                {
                    await channel.CloseAsync();
                    channel.Dispose();
                }
            };

            await channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);
        });
    }
}
