using MNX.MonitoringCenter.Management.Core.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases;
using System.Linq.Dynamic.Core;

namespace MNX.MonitoringCenter.Management.DataAccess.MiningDevice;

/// <summary>
/// Расширения для обработчики спецификации.
/// </summary>
internal static class MiningDeviceSpecificationExtensions
{
    /// <summary>
    /// Получить доступные записи.
    /// </summary>
    /// <param name="devices"> Запрашиваемые майнинг устройства. </param>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Доступные майнинга устройства. </returns>
    internal static IQueryable<MiningDeviceInfo> Available(
        this IQueryable<MiningDeviceInfo> devices, Specification specification)
    {
        return devices.Where(device => device.OwnerId == specification.UserId);
    }

    /// <summary>
    /// Фильтровать запрашиваемые майнинг устройства.
    /// </summary>
    /// <param name="devices"> Майнинг устройства. </param>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Отфильтрованные майниг устройства. </returns>
    internal static IQueryable<MiningDeviceInfo> Filter(
        this IQueryable<MiningDeviceInfo> devices, Specification specification)
    {
        if (specification.DevicesIds is not null)
        {
            devices = devices.Where(device => specification.DevicesIds.Contains(device.Id));
        }

        if (!string.IsNullOrWhiteSpace(specification.FilterString) &&
            specification.FilterParameters is not null)
        {
            return devices.Where(specification.FilterString, specification.FilterParameters);
        }

        return devices;
    }
}
