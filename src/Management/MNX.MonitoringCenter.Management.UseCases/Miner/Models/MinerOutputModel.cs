using MNX.MonitoringCenter.Management.Core.Enums;

namespace MNX.MonitoringCenter.Management.UseCases.Miner.Models;

/// <summary>
/// Выходная модель майнера.
/// </summary>
public class MinerOutputModel
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Название.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Версия.
    /// </summary>
    public required string Version { get; set; }

    /// <summary>
    /// Типы девайсов.
    /// </summary>
    public List<SupportedDeviceEnum> DeviceTypes { get; set; } = [];

    /// <summary>
    /// Режим майнинга монет (для GPU).
    /// </summary>
    public GpuMiningModeEnum MiningMode { get; set; }
}