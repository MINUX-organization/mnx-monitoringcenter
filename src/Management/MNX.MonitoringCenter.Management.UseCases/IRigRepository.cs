using MNX.MonitoringCenter.Management.Core.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases;

/// <summary>
/// Репозитория для доступа к ригам.
/// </summary>
public interface IRigRepository
{
    /// <summary>
    /// Получить признак существования рига по идентификатору.
    /// </summary>
    /// <param name="id"> Идентификатор рига. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <returns> Признак существования рига. </returns>
    Task<bool> Exists(Guid id, Guid userId);

    /// <summary>
    /// Установить устройства на риг.
    /// </summary>
    /// <param name="rigId"> Идентификатор рига. </param>
    /// <param name="devicesTuple"> Кортеж майнинг-устройств с их разгноном. </param>
    Task SetDevices(Guid rigId,
                    List<(Core.Mining.MiningDevice.MiningDevice Devices, IOverclocking Overclockings)> devicesTuple);

    /// <summary>
    /// Перевести в состояние "оффлайн"
    /// </summary>
    /// <param name="rigId"> Идентификатор рига. </param>
    Task SwitchToOffline(Guid rigId);

    /// <summary>
    /// Получить список ригов пользователя
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <returns> Список идентификаторов ригов. </returns>
    IAsyncEnumerable<Guid> GetOwnedRigs(Guid userId);
}
