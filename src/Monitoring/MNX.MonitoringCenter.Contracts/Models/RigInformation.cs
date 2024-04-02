using MNX.MonitoringCenter.Monitoring.Core;

namespace MNX.MonitoringCenter.Monitoring.Contracts.Models;

/// <summary>
/// Модель рига.
/// </summary>
public class RigInformation : Rig
{
    /// <summary>
    /// Индекс рига.
    /// </summary>
    public int Index { get; set; }
}
