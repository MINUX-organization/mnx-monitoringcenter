namespace MNX.MonitoringCenter.Monitoring.Contracts.Bus.Abstractions;

/// <summary>
/// Интерфейс рига.
/// </summary>
public interface IRig
{
    /// <summary>
    /// Идентификатор рига.
    /// </summary>
    Guid Id { get; set; }
}
