using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;

namespace MNX.MonitoringCenter.RigsApi.GrainWrapper;

/// <summary>
/// Зерно рига.
/// </summary>
public interface IRigGrain
{
    /// <summary>
    /// Идентификатор рига.
    /// </summary>
    RigId RigId { get; }

    /// <summary>
    /// Идентификатор владельца рига.
    /// </summary>
    Guid OwnerId { get; }
    
    /// <summary>
    /// Выведен ли риг из строя.
    /// </summary>
    bool IsDecommissioned { get; }

    /// <summary>
    /// Задать инвентаризацию.
    /// </summary>
    /// <param name="inventoryId"> Идентификатор инвентаризации. </param>
    /// <returns> Результат действия над ригом. </returns>
    Task<RigGrainActionResult> SetInventory(long inventoryId);

    /// <summary>
    /// Инициировать включение.
    /// </summary>
    /// <param name="initiatorId"> Идентификатор инициатора. </param>
    /// <returns> Результат действия над ригом. </returns>
    Task<RigGrainActionResult> InitiateTurnOn(Guid initiatorId);

    /// <summary>
    /// Включить.
    /// </summary>
    /// <returns> Результат действия над ригом. </returns>
    Task<RigGrainActionResult> TurnOn();

    /// <summary>
    /// Инициировать выключение.
    /// </summary>
    /// <param name="initiatorId"> Идентификатор инициатора. </param>
    /// <returns> Результат действия над ригом. </returns>
    Task<RigGrainActionResult> InitiatePowerOff(Guid initiatorId);

    /// <summary>
    /// Выключить.
    /// </summary>
    /// <returns> Результат действия над ригом. </returns>
    Task<RigGrainActionResult> PowerOff();

    /// <summary>
    /// Инициировать перезагрузку.
    /// </summary>
    /// <param name="initiatorId"> Идентификатор инициатора. </param>
    /// <returns> Результат действия над ригом. </returns>
    Task<RigGrainActionResult> InitiateReboot(Guid initiatorId);

    /// <summary>
    /// Инициировать запуск майнинга.
    /// </summary>
    /// <param name="initiatorId"> Идентификатор инициатора. </param>
    /// <returns> Результат действия над ригом. </returns>
    Task<RigGrainActionResult> InitiateStartMining(Guid initiatorId);

    /// <summary>
    /// Прервать запуск майнинга.
    /// </summary>
    /// <returns> Результат действия над ригом. </returns>
    Task<RigGrainActionResult> TerminateStartMining();

    /// <summary>
    /// Запустить майнинг.
    /// </summary>
    /// <returns> Результат действия над ригом. </returns>
    Task<RigGrainActionResult> StartMining();

    /// <summary>
    /// Инициировать остановку майнинга.
    /// </summary>
    /// <param name="initiatorId"> Идентификатор инициатора. </param>
    /// <returns> Результат действия над ригом. </returns>
    Task<RigGrainActionResult> InitiateStopMining(Guid initiatorId);

    /// <summary>
    /// Прервать остановку майнинга.
    /// </summary>
    /// <returns> Результат действия над ригом. </returns>
    Task<RigGrainActionResult> TerminateStopMining();

    /// <summary>
    /// Остановить майнинг.
    /// </summary>
    /// <returns> Результат действия над ригом. </returns>
    Task<RigGrainActionResult> StopMining();
    
    /// <summary>
    /// Переименовать риг.
    /// </summary>
    /// <param name="newName"> Новое имя рига. </param>
    /// <returns> Результат действия над ригом. </returns>
    Task<RigGrainActionResult> Rename(string newName);
    
    /// <summary>
    /// Вывести риг из строя.
    /// </summary>
    /// <returns> Результат действия над ригом. </returns>
    Task<RigGrainActionResult> Decommission();
}
