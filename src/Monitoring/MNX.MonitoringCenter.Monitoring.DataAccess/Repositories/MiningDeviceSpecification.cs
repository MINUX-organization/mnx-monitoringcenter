using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Monitoring.DataAccess.Dto.Devices;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries.Devices;
using System.Linq.Dynamic;

namespace MNX.MonitoringCenter.Monitoring.DataAccess.Repositories;

/// <summary>
/// Набор методов для фильтрации майнинг устройств, исходя из спецификации.
/// </summary>
internal static class MiningDeviceSpecification
{
    /// <summary>
    /// Получить доступные пользователю майнинг устройства.
    /// </summary>
    /// <param name="devices"> Риги. </param>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Доступные риги. </returns>
    internal static IQueryable<MiningDeviceDto> Available(this IQueryable<MiningDeviceDto> devices,
                                                          DeviceSpecification specification)
    {
        return devices.Where(device => device.UserId == specification.UserId);
    }

    /// <summary>
    /// Отфильтровать коллекцию майнинг устройств.
    /// </summary>
    /// <param name="devices"> Майнинг устройства. </param>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Отфильтрованные майнинг устройства. </returns>
    internal static IQueryable<MiningDeviceDto> Filter(this IQueryable<MiningDeviceDto> devices,
                                                       DeviceSpecification specification)
    {
        devices = devices.Where(device => specification.Types.Contains(device.Type));

        if (!string.IsNullOrEmpty(specification.SearchString))
        {
            devices = devices.Search(specification.SearchString);
        }

        if (!string.IsNullOrEmpty(specification.FilterString) && specification.FilterArguments is not null)
        {
            devices = devices.Where(specification.FilterString, specification.FilterArguments);
        }

        return devices;
    }

    /// <summary>
    /// Найти майнинг устройства, исходя из строки поиска.
    /// </summary>
    /// <param name="devices"> майнинг устройства. </param>
    /// <param name="searchString"> Строка поиска. </param>
    /// <returns> Найденные майнинг устройства. </returns>
    private static IQueryable<MiningDeviceDto> Search(this IQueryable<MiningDeviceDto> devices, string searchString)
    {
        return devices.Where(item => EF.Functions.Like(item.Name, $"%{searchString}%"));
    }
}
