using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using MNX.MonitoringCenter.Management.UseCases.Mapping;

namespace MNX.MonitoringCenter.Management.Integration;

/// <summary>
/// Расширение <see cref="IServiceCollection"/> для интеграции модуля управления.
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
        services.AddAutoMapper(cfg => cfg.AddProfiles(new List<Profile>()
        {
            new CryptocurrencyMappingProfile(),
            new FlightSheetMappingProfile(),
            new MinerMappingProfile(),
            new MiningConfigMappingProfile(),
            new MiningDeviceMappingProfile(),
            new OverclockingMappingProfile(),
            new PoolMappingProfile(),
            new PresetMappingProfile(),
            new WalletMappingProfile()
        }));

        return services;
    }
}