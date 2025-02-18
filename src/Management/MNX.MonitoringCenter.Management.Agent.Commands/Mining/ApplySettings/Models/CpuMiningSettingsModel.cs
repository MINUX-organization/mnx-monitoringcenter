namespace MNX.MonitoringCenter.Management.Agent.Commands.Mining.ApplySettings.Models;

/// <summary>
/// Модель настроек майнинга для процессора.
/// </summary>
public class CpuMiningSettingsModel : BaseMiningSettingsModel
{
    /// <summary>
    /// Страницы.
    /// </summary>
    public int? HugePages { get; init; }

    /// <summary>
    /// Кол-во потоков.
    /// </summary>
    public int? ThreadsCount { get; init; }
}
