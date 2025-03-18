using MessagePack;

namespace MNX.MonitoringCenter.Management.Agent.Commands.Mining.ApplySettings.Models;

/// <summary>
/// Базовые настройки для майнинга.
/// </summary>
[Union(0, typeof(CpuMiningSettingsModel))]
[Union(1, typeof(GpuMiningSettingsModel))]
public abstract class BaseMiningSettingsModel
{
    /// <summary>
    /// Название майнера.
    /// </summary>
    public required string MinerName { get; init; }
    
    /// <summary>
    /// Версия майнера.
    /// </summary>
    public required string MinerVersion { get; init; }

    /// <summary>
    /// Список конфигураций для майнинга монет.
    /// </summary>
    public List<MiningCoinConfigModel> CoinConfigs { get; set; } = new(0);

    /// <summary>
    /// Дополнительные аргументы для майнинга.
    /// </summary>
    public string? AdditionalArguments { get; init; }

    /// <summary>
    /// Содержание конфиг файла.
    /// </summary>
    public string? ConfigFileContent { get; init; }
}
