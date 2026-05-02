using EasyNetQ.AutoSubscribe;
using MessagePack;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MNX.MonitoringCenter.RigsApi.Service.Tests;

public sealed class ConsumerHost<T> : IAsyncDisposable where T : class
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConnection _connection;

    private IModel? _channel;
    private AsyncEventingBasicConsumer? _consumer;
    private CancellationTokenSource? _cts;

    public ConsumerHost(IServiceProvider serviceProvider, string host, int port)
    {
        _serviceProvider = serviceProvider;

        var factory = new ConnectionFactory
        {
            HostName = host,
            Port = port,
            UserName = "guest",
            Password = "guest",
            DispatchConsumersAsync = true
        };

        _connection = factory.CreateConnection();
    }

    public Task StartAsync(string queueName, CancellationToken externalCt = default)
    {
        if (_channel is not null)
            throw new InvalidOperationException("Consumer already started");

        _cts = CancellationTokenSource.CreateLinkedTokenSource(externalCt);

        _channel = _connection.CreateModel();

        var dlxName = $"{queueName}_dlx";
        _channel.ExchangeDeclare(
            exchange: dlxName,
            type: ExchangeType.Fanout,
            durable: false,
            autoDelete: false);

        var errorQueue = $"{queueName}_error";

        _channel.QueueDeclare(
            queue: errorQueue,
            durable: false,
            exclusive: false,
            autoDelete: false);

        _channel.QueueBind(
            queue: errorQueue,
            exchange: dlxName,
            routingKey: "");

        _channel.QueueDeclare(
            queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: new Dictionary<string, object>
            {
                {
                    "x-dead-letter-exchange", dlxName
                }
            });

        _consumer = new AsyncEventingBasicConsumer(_channel);
        _consumer.Received += HandleMessage;

        _channel.BasicConsume(
            queue: queueName,
            autoAck: false,
            consumer: _consumer);

        return Task.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        await StopAsync();

        _connection.Dispose();

        _cts?.Dispose();
    }

    private async Task HandleMessage(object sender, BasicDeliverEventArgs ea)
    {
        if (_channel is null || _cts is null) return;

        try
        {
            using var scope = _serviceProvider.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<IConsumeAsync<T>>();

            var json = Encoding.UTF8.GetString(ea.Body.ToArray());
            var options = MessagePackSerializerOptions.Standard
                .WithResolver(MessagePack.Resolvers.ContractlessStandardResolver.Instance);
            var message = MessagePackSerializer.Deserialize<T>(ea.Body.ToArray(), options) ??
                throw new InvalidOperationException("Failed to deserialize message");

            await handler.ConsumeAsync(message, _cts.Token);

            _channel.BasicAck(ea.DeliveryTag, multiple: false);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Consumer error: {ex}");

            _channel?.BasicNack(ea.DeliveryTag, multiple: false, requeue: false);
        }
    }

    private async Task StopAsync()
    {
        if (_channel is null) return;

        _cts?.Cancel();

        _consumer!.Received -= HandleMessage;

        _channel.Close();
        _channel.Dispose();

        _channel = null;
        _consumer = null;

        await Task.CompletedTask;
    }
}
