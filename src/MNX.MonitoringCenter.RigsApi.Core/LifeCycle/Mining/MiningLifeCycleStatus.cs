namespace MNX.MonitoringCenter.RigsApi.Core.LifeCycle.Mining;

/// <summary>
/// Статус жизненного цикла майнинга.
/// </summary>
public enum MiningLifeCycleStatus
{
    /// <summary>
    /// Выключен.
    /// </summary>
    Disable,

    /// <summary>
    /// Ожидает включения.
    /// </summary>
    AwaitsEnable,

    /// <summary>
    /// Включен.
    /// </summary>
    Enable,

    /// <summary>
    /// Ожидает выключения.
    /// </summary>
    AwaitsDisable
}
