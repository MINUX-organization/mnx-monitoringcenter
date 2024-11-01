using MNX.MonitoringCenter.Management.Core.MiningDevice;

namespace MNX.MonitoringCenter.Management.UseCases.MiningDevice;

/// <summary>
/// Репозиторий для доступа к майнинг устройствам.
/// </summary>
public interface IMiningDeviceRepository
{
    /// <summary>
    /// Получить активное майнинг устройство по идентификатору.
    /// </summary>
    /// <param name="id"> Идентификатор. </param>
    /// <param name="userId"> идентификатор пользователя. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Майнинг устройство. </returns>
    Task<MiningDeviceDetails?> GetActiveDeviceById(Guid id, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Задать текущие устройства ригов.
    /// </summary>
    /// <param name="devices"> Устройства. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    Task SetCurrentRigsDevices(List<Core.MiningDevice.MiningDevice> devices, CancellationToken cancellationToken);

    /// <summary>
    /// Деактивировать устройства установленные на риге по переданному идентификатору рига.
    /// </summary>
    /// <param name="rigId"> Идентификатор рига. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    Task DeactivateDevicesByRigId(Guid rigId, CancellationToken cancellationToken);

    /// <summary>
    /// Установить полётный лист на устройство.
    /// </summary>
    /// <param name="devicesIds"> Идентификаторы устройств. </param>
    /// <param name="flightSheetId"> Идентификатор полётного листа. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    Task SetFlightSheet(Guid[] devicesIds, Guid flightSheetId, CancellationToken cancellationToken);
}
