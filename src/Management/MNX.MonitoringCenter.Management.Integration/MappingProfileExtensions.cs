using Microsoft.Extensions.DependencyInjection;
using MNX.MonitoringCenter.Management.DataAccess.Mapping;
using MNX.MonitoringCenter.Management.DataAccess.Mapping.Converters;
using MNX.MonitoringCenter.Management.Integration.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.Mapping.AgentCommands;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Cryptocurrency;
using MNX.MonitoringCenter.Management.UseCases.Mapping.FlightSheet;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Miner;
using MNX.MonitoringCenter.Management.UseCases.Mapping.MiningConfig;
using MNX.MonitoringCenter.Management.UseCases.Mapping.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Pool;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Preset;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Wallet;
using MNX.MonitoringCenter.Management.UseCases.Mining.Cryptocurrency;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Events;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool;
using MNX.MonitoringCenter.Management.UseCases.Mining.Wallet;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets;

namespace MNX.MonitoringCenter.Management.Integration;

/// <summary>
/// Расширение <see cref="IServiceCollection"/> для интеграции профилей мапперов.
/// </summary>
public static class MappingProfileExtensions
{
    ///
    public static IServiceCollection AddMappingModule(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddProfile(new DbMappingProfile()));
        services.AddTransient<FanOverclockingDtoConverter>();
        services.AddTransient<OverclockingDtoConverter>();

        services.AddScoped<IAgentCommandsMapper, AgentCommandsMapper>();
        services.AddScoped<ICryptocurrencyMapper, CryptocurrencyMapper>();
        services.AddScoped<IFlightSheetMapper, FlightSheetMapper>();
        services.AddScoped<IMinerMapper, MinerMapper>();
        services.AddScoped<IMiningDeviceMapper, MiningDeviceMapper>();
        services.AddScoped<IPoolMapper, PoolMapper>();
        services.AddScoped<IWalletMapper, WalletMapper>();
        services.AddScoped<IPresetMapper, PresetMapper>();
        services.AddScoped<IMiningConfigMapper, MiningConfigMapper>();

        services.AddOverclockingMapping();
        services.AddFanOverclockingMapping();

        return services;
    }
}