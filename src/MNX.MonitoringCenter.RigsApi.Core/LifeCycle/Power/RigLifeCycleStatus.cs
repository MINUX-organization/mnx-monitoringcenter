namespace MNX.MonitoringCenter.RigsApi.Core.LifeCycle.Power;

/// <summary>
/// Статус жизненного цикла рига.
/// </summary>
public enum RigLifeCycleStatus
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
