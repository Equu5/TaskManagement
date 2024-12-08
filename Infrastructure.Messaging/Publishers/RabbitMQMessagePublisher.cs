using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

public class RabbitMqMessagePublisher : IMessagePublisher
{
    private readonly RabbitMqConnectionManager _connectionManager;

    public RabbitMqMessagePublisher(RabbitMqConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
    }

    public async Task PublishAsync<T>(T message, string exchangeName, string routingKey)
    {
        var connection = await _connectionManager.GetConnectionAsync();

        using var channel = await connection.CreateChannelAsync();
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Topic, durable: true);
        await channel.BasicPublishAsync(
            exchange: exchangeName,
            routingKey: routingKey,
            body: body
        );

        await Task.CompletedTask;
    }
}
