using EasyNetQ.AutoSubscribe;
using MediatR;
using MessagePack;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace MNX.MonitoringCenter.RigsApi.Service.Tests;

internal static class TestsEnvironmentService
{
    public static ConsumerHost<T> CreateHost<T>(IMessageBrokerFixture broker, Action<IServiceCollection> configure, params Type[] commandTypes)
        where T : class
    {
        var provider = CreateServiceProvider(configure, commandTypes);
        return new ConsumerHost<T>(provider, broker.Host, broker.Port);
    }

    public static async Task Publish(string queueName, object message, IMessageBrokerFixture brokerFixture)
    {
        var body = Serialize(message);

        var factory = new ConnectionFactory
        {
            HostName = brokerFixture.Host,
            Port = brokerFixture.Port,
            UserName = "guest",
            Password = "guest",
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.BasicPublish(
            exchange: "",
            routingKey: queueName,
            basicProperties: null,
            body: body);

        await Task.Delay(1000);
    }

    public static IServiceCollection AddEventHandler<TEvent, TEventHandler>(this IServiceCollection services)
        where TEventHandler : class, INotificationHandler<TEvent>
        where TEvent : class, INotification
    {
        services.AddTransient<INotificationHandler<TEvent>, TEventHandler>();
        return services;
    }

    public static IServiceCollection AddConsumer<TMessage, TConsumer>(this IServiceCollection services)
        where TConsumer : class, IConsumeAsync<TMessage>
        where TMessage : class
    {
        services.AddTransient<IConsumeAsync<TMessage>, TConsumer>();
        return services;
    }

    private static ServiceProvider CreateServiceProvider(Action<IServiceCollection>? configure = null, params Type[] commandTypes)
    {
        IServiceCollection services = new ServiceCollection();
        services.AddLogging();
        services.AddMediatR(cfg =>
        {
            foreach (var type in commandTypes)
            {
                cfg.RegisterServicesFromAssemblies(type.Assembly);
            }
        });

        configure?.Invoke(services);

        return services.BuildServiceProvider();
    }

    private static byte[]? Serialize(object message)
    {
        var options = MessagePackSerializerOptions.Standard
            .WithResolver(MessagePack.Resolvers.ContractlessStandardResolver.Instance);
        return MessagePackSerializer.Serialize(message, options);
    }

}
