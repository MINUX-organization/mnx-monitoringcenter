using AutoMapper;
using MNX.Application.Data.EF.DI;
using MNX.Application.UseCases.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MNX.MonitoringCenter.Management.UseCases;
using MNX.MonitoringCenter.Management.DataAccess;
using MNX.MonitoringCenter.Management.DataAccess.Pool;
using MNX.MonitoringCenter.Management.DataAccess.Miner;
using MNX.MonitoringCenter.Management.DataAccess.Preset;
using MNX.MonitoringCenter.Management.DataAccess.Wallet;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool;
using MNX.MonitoringCenter.Management.DataAccess.Algorithm;
using MNX.RigCommander.MessageQueue.Clients.Bus.Integration;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner;
using MNX.MonitoringCenter.Management.DataAccess.FlightSheet;
using MNX.MonitoringCenter.Management.UseCases.Mining.Wallet;
using MNX.MonitoringCenter.Management.DataAccess.MiningDevice;
using MNX.MonitoringCenter.Management.DataAccess.Cryptocurrency;
using MNX.MonitoringCenter.Management.DataAccess.MinerAlgorithm;
using MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets;
using MNX.MonitoringCenter.Management.UseCases.Mining.Cryptocurrency;
using MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm.Queries;
using MNX.MonitoringCenter.Management.Agent.Commands.Mining.ApplySettings;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Commands.SavePreset;

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
        services.AddAutoMapper(cfg => cfg.AddProfiles(new List<Profile>()
        {
            new MappingProfile(),
            new DbMappingProfile()
        }));

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
            typeof(GetAvailableAlgorithmsQuery).Assembly,
            typeof(ApplyWorkerSettingsCommand).Assembly
        ));
        services.AddValidationPipelines(typeof(SavePresetValidator).Assembly);
        services.AddDataContext<Context>(configuration);

        services.AddScoped<IRigRepository, RigRepository>();
        services.AddScoped<IAlgorithmRepository, AlgorithmRepository>();
        services.AddScoped<ICryptocurrencyRepository, CryptocurrencyRepository>();
        services.AddScoped<IFlightSheetRepository, FlightSheetRepository>();
        services.AddScoped<IMiningDeviceRepository, MiningDeviceRepository>();
        services.AddScoped<IMinerRepository, MinerRepository>();
        services.AddScoped<IMinerAlgorithmRepository, MinerAlgorithmRepository>();
        services.AddScoped<IPoolRepository, PoolRepository>();
        services.AddScoped<IPresetRepository, PresetRepository>();
        services.AddScoped<IWalletRepository, WalletRepository>();
        services.AddAgentQueueClient();

        return services;
    }
}
