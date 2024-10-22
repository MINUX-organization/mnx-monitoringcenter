using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MNX.Application.Data.DI;
using MNX.Application.UseCases.DI;
using MNX.MonitoringCenter.Management.DataAccess;
using MNX.MonitoringCenter.Management.DataAccess.Algorithm;
using MNX.MonitoringCenter.Management.DataAccess.Cryptocurrency;
using MNX.MonitoringCenter.Management.DataAccess.FlightSheet;
using MNX.MonitoringCenter.Management.DataAccess.Miner;
using MNX.MonitoringCenter.Management.DataAccess.Pool;
using MNX.MonitoringCenter.Management.DataAccess.Preset;
using MNX.MonitoringCenter.Management.DataAccess.Wallet;
using MNX.MonitoringCenter.Management.UseCases;
using MNX.MonitoringCenter.Management.UseCases.Algorithm;
using MNX.MonitoringCenter.Management.UseCases.Algorithm.Queries;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;
using MNX.MonitoringCenter.Management.UseCases.Cryptocurrency;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet;
using MNX.MonitoringCenter.Management.UseCases.Miner;
using MNX.MonitoringCenter.Management.UseCases.Pool;
using MNX.MonitoringCenter.Management.UseCases.Presets;
using MNX.MonitoringCenter.Management.UseCases.Wallet;

namespace MNX.MonitoringCenter.Management.Integration;

/// <summary>
/// Расширение <see cref="IServiceCollection"/> для интеграции модуля управления.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавить модуль управления.
    /// </summary>
    /// <param name="services"> Коллекция сервисов. </param>
    /// <param name="configuration"> Конфигурация. </param>
    /// <returns> Коллекция сервисов. </returns>
    public static IServiceCollection AddManagementModule(this IServiceCollection services,
                                                         IConfiguration configuration)
    {
        services.AddAutoMapper(cfg => cfg.AddProfile(typeof(MappingProfile)));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAvailableAlgorithmsQuery).Assembly));
        services.AddValidationPipelines(typeof(SavePresetValidator).Assembly);
        services.AddDataContext<Context>(configuration);

        services.AddScoped<IAlgorithmRepository, AlgorithmRepository>();
        services.AddScoped<ICryptocurrencyRepository, CryptocurrencyRepository>();
        services.AddScoped<IFlightSheetRepository, FlightSheetRepository>();
        services.AddScoped<IMinerRepository, MinerRepository>();
        services.AddScoped<IPoolRepository, PoolRepository>();
        services.AddScoped<IPresetRepository, PresetRepository>();
        services.AddScoped<IWalletRepository, WalletRepository>();

        return services;
    }
}
