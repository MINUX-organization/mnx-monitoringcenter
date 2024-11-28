using MNX.MonitoringCenter.Management.Core.MiningDevice;
using MNX.MonitoringCenter.Management.Core.MiningDevice.Enums;

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
    /// Задать текущие устройства ригов.
    /// </summary>
    /// <param name="devices"> Устройства. </param>
    Task SetCurrentRigsDevices(List<Core.MiningDevice.MiningDevice> devices);

    /// <summary>
    /// Установить <see cref="MiningDeviceLifeCycleStatus"/> для устройств рига.
    /// </summary>
    /// <param name="rigId"> Идентификатор рига. </param>
    /// <param name="status"> Статус жизненного цикла устройства. </param>
    Task SetStatusForRigDevices(Guid rigId, MiningDeviceLifeCycleStatus status);

    /// <summary>
    /// Установить полётный лист на устройства.
    /// </summary>
    /// <param name="devicesIds"> Идентификаторы устройств. </param>
    /// <param name="flightSheetId"> Идентификатор полётного листа. </param>
    Task SetFlightSheet(Guid[] devicesIds, Guid flightSheetId);

    /// <summary>
    /// Снять полётный лист с устройств.
    /// </summary>
    /// <param name="devicesIds"> Идентификаторы устройств. </param>
    Task RemoveFlightSheet(Guid[] devicesIds);

    /// <summary>
    /// Подтвердить значение полётного листа на устройствах.
    /// </summary>
    /// <param name="devicesIds"> Идентификаторы устройств. </param>
    Task ConfirmFlightSheet(Guid[] devicesIds);
}
