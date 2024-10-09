using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MNX.Application.RabbitMQ;
using MNX.MonitoringCenter.Traffic.Controllers;
using MNX.MonitoringCenter.Traffic.Observers;
using MNX.MonitoringCenter.Traffic.Observers.Abstractions;
using MNX.MonitoringCenter.Traffic.Observers.Mapping;
using System.Reflection;

namespace MNX.MonitoringCenter.Traffic.Integration;

/// <summary>
/// Расширения для <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавить обработку трафика.
    /// </summary>
    /// <param name="services"> Сервисы. </param>
    /// <param name="configuration"> Конфигурация. </param>
    /// <returns> Сервисы. </returns>
    public static IServiceCollection AddTrafficProcessing(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IUserRigsObserverAggregator, UserRigsObserverAggregator>();
        services.AddSignalR();
        services.AddEasyNetQ(configuration, new Assembly[] { typeof(RigsDynamicIndicatorsConsumer).Assembly });
        services.AddAutoMapper(x => x.AddProfile<MappingProfile>());

        services.Configure<DynamicIndicatorsOptions>(configuration.GetSection(nameof(DynamicIndicatorsOptions)));

        return services;
    }
}
