using MNX.MonitoringCenter.Monitoring.Contracts.Abstractions;
using MNX.MonitoringCenter.Monitoring.Contracts.Enums;

namespace MNX.MonitoringCenter.Monitoring.Contracts.Models;

/// <summary>
/// Состояние рига.
/// </summary>
public class RigState : SystemData
{
    /// <summary>
    /// Состояния видеокарт.
    /// </summary>
    public List<GpusState> GpusStates { get; set; } = new();

    /// <summary>
    /// Признак активности рига.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Уровень интернет соединения.
    /// </summary>
    public OnlineState OnlineState { get; set; }
}
