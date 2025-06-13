using MNX.MonitoringCenter.Management.Core.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases;

/// <summary>
/// Репозитория для доступа к ригам.
/// </summary>
public interface IRigRepository
{
    /// <summary>
    /// Установить устройства на риг.
    /// </summary>
    /// <param name="rigId"> Идентификатор рига. </param>
    /// <param name="devicesTuple"> Кортеж майнинг-устройств с их разгоном. </param>
    Task SetDevices(Guid rigId, List<(Core.Mining.MiningDevice.MiningDevice Devices, IOverclocking Overclockings)> devicesTuple);

    /// <summary>
    /// Перевести в состояние "оффлайн".
    /// </summary>
    /// <param name="rigId"> Идентификатор рига. </param>
    Task SwitchToOffline(Guid rigId);
}
