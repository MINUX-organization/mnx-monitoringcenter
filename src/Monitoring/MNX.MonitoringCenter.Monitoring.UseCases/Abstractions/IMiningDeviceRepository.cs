using MNX.MonitoringCenter.Monitoring.Core;
using MNX.MonitoringCenter.Monitoring.Core.Devices;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries.Devices;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;

/// <summary>
/// Репозиторий для майнинг устройств.
/// </summary>
public interface IMiningDeviceRepository
{
    /// <summary>
    /// Получить список майнинг устройств.
    /// </summary>
    /// <returns></returns>
    IAsyncEnumerable<MiningDevice> GetList(DeviceSpecification specification);

    /// <summary>
    /// Получить разгон по идентификатору майнинг устройста.
    /// </summary>
    /// <param name="id"> Идентификатор майнинг устройста. </param>
    /// <returns> Разгон майнинг устройства. </returns>
    Task<Overclocking?> GetOverclockingById(Guid id);

    /// <summary>
    /// Задать разгон по идентификатору майнинг устройста.
    /// </summary>
    /// <param name="id"> Идентификатор майнинг устройста. </param>
    /// <param name="overclocking"> Разгон. </param>
    Task SetOverclockingById(Guid id, Overclocking overclocking);
}
