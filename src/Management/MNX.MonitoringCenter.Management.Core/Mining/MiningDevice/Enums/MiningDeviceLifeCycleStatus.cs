namespace MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;

/// <summary>
/// Статус жизненного цикла майнинг устройства.
/// </summary>
public enum MiningDeviceLifeCycleStatus
{
    /// <summary>
    /// В сети.
    /// </summary>
    Online,

    /// <summary>
    /// Не в сети.
    /// </summary>
    Offline,

    /// <summary>
    /// Неактивно.
    /// </summary>
    Inactive
}
