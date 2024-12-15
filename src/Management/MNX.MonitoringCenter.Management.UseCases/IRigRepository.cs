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
    /// <param name="devices"> Устройства. </param>
    Task SetDevices(Guid rigId, List<Core.Mining.MiningDevice.MiningDevice> devices);
}
