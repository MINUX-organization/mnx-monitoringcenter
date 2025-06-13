using MNX.MonitoringCenter.RigsApi.Core.LifeCycle.Mining;
using MNX.MonitoringCenter.RigsApi.Core.LifeCycle.Power;

namespace MNX.MonitoringCenter.RigsApi.UseCases.GetRigs;

/// <summary>
/// Модель рига.
/// </summary>
public class RigModel
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Идентификатор владельца.
    /// </summary>
    public Guid OwnerId { get; init; }

    /// <summary>
    /// Название.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Идентификатор текущей инвентаризации.
    /// </summary>
    public Guid? CurrentInventoryId { get; init; }

    /// <summary>
    /// Признак нахождения "В сети".
    /// </summary>
    public bool IsOnline { get; init; }

    /// <summary>
    /// Статус жизненного цикла.
    /// </summary>
    public RigLifeCycleStatus LifeCycleStatus { get; init; }

    /// <summary>
    /// Статус жизненного цикла майнинга.
    /// </summary>
    public MiningLifeCycleStatus MiningLifeCycleStatus { get; init; }
}
