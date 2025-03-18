namespace MNX.MonitoringCenter.Management.Agent.Commands.Mining.ApplySettings.Models;

/// <summary>
/// Настройки воркера.
/// </summary>
public class WorkerSettings
{
    /// <summary>
    /// Идентификатор воркера.
    /// </summary>
    public Guid WorkerId { get; init; }

    /// <summary>
    /// Модель с настройками.
    /// </summary>
    public BaseMiningSettingsModel? SettingsModel { get; init; }
}
