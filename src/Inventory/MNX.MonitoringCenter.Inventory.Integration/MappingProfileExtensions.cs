using Microsoft.Extensions.DependencyInjection;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu;

namespace MNX.MonitoringCenter.Inventory.Integration;

/// <summary>
/// Расширение <see cref="IServiceCollection"/> для интеграции профилей мапперов.
/// </summary>
internal static class MappingProfileExtensions
{
    /// <summary>
    /// Добавить профили маппинга.
    /// </summary>
    /// <param name="services"> Коллекция сервисов. </param>
    /// <returns> Коллекция сервисов. </returns>
    public static IServiceCollection AddInventoryMappingProfile(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => cfg.AddProfiles(
        [
            new GpusMappingProfile(),
        ]));

        return services;
    }
}
