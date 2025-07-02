using Microsoft.Extensions.DependencyInjection;
using MNX.MonitoringCenter.Management.DataAccess;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner;
using MNX.MonitoringCenter.Management.UseCases.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.Mining.Wallet;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets;
using MNX.MonitoringCenter.Management.UseCases.Mining.Cryptocurrency;

namespace MNX.MonitoringCenter.Management.Integration;

/// <summary>
/// Расширение <see cref="IServiceCollection"/> для интеграции профилей мапперов.
/// </summary>
public static class MappingProfileExtensions
{
    /// <summary>
    /// Добавить профили маппинга.
    /// </summary>
    /// <param name="services"> Коллекция сервисов. </param>
    /// <returns> Коллекция сервисов. </returns>
    public static IServiceCollection AddUseCaseMappingProfile(this IServiceCollection services)
    {
        services.AddTransient<OverclockingMappingProfile.OverclockingModelConverter>();
        services.AddAutoMapper(cfg => cfg.AddProfiles(
        [
            new CryptocurrencyMappingProfile(),
            new FlightSheetMappingProfile(),
            new MinerMappingProfile(),
            new MiningConfigMappingProfile(),
            new MiningDeviceMappingProfile(),
            new OverclockingMappingProfile(),
            new PoolMappingProfile(),
            new PresetMappingProfile(),
            new WalletMappingProfile(),
            new DbMappingProfile()
        ]));

        return services;
    }
}