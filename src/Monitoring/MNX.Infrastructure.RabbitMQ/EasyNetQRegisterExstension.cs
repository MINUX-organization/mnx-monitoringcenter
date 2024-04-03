using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using EasyNetQ.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MNX.Infrastructure.RabbitMQ;

/// <summary>
/// Расширение для интеграции EasyNetQ в DI.
/// </summary>
public static class EasyNetQRegisterExstension
{
    /// <summary>
    /// Добавить EasyNetQ в DI.
    /// </summary>
    /// <param name="services"> Сервисы DI. </param>
    /// <param name="configuration"> Конфигурация приложения. </param>
    /// <param name="assemblies"> Сборки. </param>
    /// <returns> Коллекция сервисов DI. </returns>
    /// <exception cref="ArgumentNullException">
    /// Не задана строка подключения к RabbitMQ или не узказано имя сервиса.
    /// </exception>
    public static IServiceCollection AddEasyNetQ(this IServiceCollection services,
                                                 IConfiguration configuration,
                                                 Assembly[] assemblies)
    {

        var connectionString = configuration["RabbitMQConnection"];

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentNullException(null, "Не задана строка подключения к RabbitMQ");
        }

        var subscriptionIdPrefix = configuration["ServiceName"];

        if (string.IsNullOrEmpty(subscriptionIdPrefix))
        {
            throw new ArgumentNullException(null, "Не указано название сервиса");
        }

        services.RegisterEasyNetQ(connectionString, register =>
        {
            register.EnableMicrosoftLogging();
        });

        List<Type> consumers = new();

        foreach (var consumer in GetConsumers(assemblies))
        {
            services.AddTransient(consumer);
            consumers.Add(consumer);
        }

        services.AddTransient(provider => new AutoSubscriberWrapper(provider.GetRequiredService<IServiceResolver>(),
                                                                   consumers.ToArray(),
                                                                   subscriptionIdPrefix));

        services.AddHostedService<BusSubscriber<AutoSubscriberWrapper>>();

        return services;
    }

    /// <summary>
    /// Добавить потребителей сообщений в DI.
    /// </summary>
    /// <param name="assemblies"> Сборки. </param>
    private static IEnumerable<Type> GetConsumers(Assembly[] assemblies)
    {
        var types = assemblies.SelectMany(assembly => assembly.GetTypes());

        foreach (var type in types)
        {
            if (type.IsClass &&
                !type.IsAbstract &&
                !type.ContainsGenericParameters &&
                type.GetInterfaces().Any(type => type.IsGenericType &&
                                                 type.GetGenericTypeDefinition() is var definition &&
                                                (definition == typeof(IConsumeAsync<>) || definition == typeof(IConsume<>))))
            {
                yield return type;
            }
        }
    }
}
