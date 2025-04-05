using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice;

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
    /// Получить список устройств, с привязкой к конкретному пресету.
    /// </summary>
    /// <param name="presetId">Идентификатор пресета. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <returns> Коллекцию объектов <see cref="MiningDeviceInfo"/>. </returns>
    Task<List<MiningDeviceInfo>> GetAvailableByPresetId(Guid presetId,
                                                        Guid userId);

    /// <summary>
    /// Получить майнинг-устройство по идентификатору.
    /// </summary>
    /// <remarks>
    /// !!! ВНИМАНИЕ: НЕБЕЗОПАСНО С ТОЧКИ ЗРЕНИЯ БЕЗОПАСНОСТИ ПОЛЬЗОВАТЕЛЬСКИХ ДАННЫХ В БД.
    /// ДАННЫЙ МЕТОД ЯВЛЯЕТСЯ ЧАСТЬЮ КОСТЫЛЯ И ДОЛЖЕН БЫТЬ УДАЛЕН СРАЗУ, КАК ДАННЫЙ КОСТЫТЬ
    /// ПЕРЕСТАНЕТ БЫТЬ ЧАСТЬЮ СИСТЕМЫ !!!
    /// </remarks>
    /// <param name="id"> Идентификатор устройства. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Майнинг-устройство. </returns>
    public Task<MiningDeviceInfo> GetById(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Получить активное майнинг устройство по идентификатору.
    /// </summary>
    /// <param name="id"> Идентификатор. </param>
    /// <param name="userId"> идентификатор пользователя. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Майнинг устройство. </returns>
    Task<MiningDeviceInfo?> GetActiveDeviceById(Guid id,
                                                Guid userId,
                                                CancellationToken cancellationToken);

    /// <summary>
    /// Получить признак существования устройства с переданным названием.
    /// </summary>
    /// <param name="name"> Название. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns>
    /// <see langword="true"/>, если существует, иначе <see langword="false"/>.
    /// </returns>
    Task<bool> Exists(string name, Guid userId, CancellationToken cancellationToken);

    /// <summary>
    /// Выполнить обновление пресета с разгоном на устройстве.
    /// </summary>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <param name="presetId"> Идентификатор пресета. </param>
    /// <param name="deviceIds"> Идентификаторы устройств. </param>
    Task SetPreset(Guid presetId,
                   CancellationToken cancellationToken,
                   params Guid[] deviceIds);

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

    /// <summary>
    /// Задать статус подтверждения полетного листа
    /// на майнинг-устройстве как <see cref="FlightSheetConfirmationState.Error"/>.
    /// </summary>
    /// <param name="devicesIds"> Идентификаторы устройств. </param>
    Task SetFlightSheetConfirmationStateToError(Guid[] devicesIds);

    /// <summary>
    /// Получить разгон майнинг устройства.
    /// </summary>
    /// <param name="deviceId"> Идентификатор устройства. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <returns> Разгон майнинг устройства. </returns>
    Task<IOverclocking?> GetOverclocking(Guid deviceId, Guid userId);

    /// <summary>
    /// Задать разгон майнинг устройству напрямую.
    /// </summary>
    /// <param name="overclocking"> Разгон. </param>
    /// <param name="device"> Майнинг-устройство. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    Task SetOverclocking(MiningDeviceInfo device,
                         IOverclocking overclocking,
                         CancellationToken cancellationToken);
}
