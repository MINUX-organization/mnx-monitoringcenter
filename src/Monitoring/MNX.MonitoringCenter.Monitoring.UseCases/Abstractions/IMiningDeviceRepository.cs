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
}
