using MNX.MonitoringCenter.Management.Core.MiningDevice;

namespace MNX.MonitoringCenter.Management.UseCases.MiningDevice;

/// <summary>
/// Репозиторий для доступа к майнинг устройствам.
/// </summary>
public interface IMiningDeviceRepository
{
    /// <summary>
    /// Получить доступные майнинг устройства по спецификации.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Поток запрашиваемых майнинг устройств. </returns>
    IAsyncEnumerable<MiningDeviceInfo> GetAvailable(Specification specification);

    /// <summary>
    /// Получить активное майнинг устройство по идентификатору.
    /// </summary>
    /// <param name="id"> Идентификатор. </param>
    /// <param name="userId"> идентификатор пользователя. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Майнинг устройство. </returns>
    Task<MiningDeviceInfo?> GetActiveDeviceById(Guid id, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Получить устройства, поддерживаемые полётным листом.
    /// </summary>
    /// <param name="flightSheet"> Полётный лист. </param>
    /// <returns> Устройства, поддерживаемые полётным листом. </returns>
    IAsyncEnumerable<MiningDeviceInfo> GetFlightSheetSupportedDevices(Core.FlightSheet.FlightSheet flightSheet);

    /// <summary>
    /// Задать текущие устройства ригов.
    /// </summary>
    /// <param name="devices"> Устройства. </param>
    Task SetCurrentRigsDevices(List<Core.MiningDevice.MiningDevice> devices);

    /// <summary>
    /// Деактивировать устройства установленные на риге по переданному идентификатору рига.
    /// </summary>
    /// <param name="rigId"> Идентификатор рига. </param>
    Task DeactivateDevicesByRigId(Guid rigId);

    /// <summary>
    /// Установить полётный лист на устройство.
    /// </summary>
    /// <param name="devicesIds"> Идентификаторы устройств. </param>
    /// <param name="flightSheetId"> Идентификатор полётного листа. </param>
    Task SetFlightSheet(Guid[] devicesIds, Guid flightSheetId);
}
