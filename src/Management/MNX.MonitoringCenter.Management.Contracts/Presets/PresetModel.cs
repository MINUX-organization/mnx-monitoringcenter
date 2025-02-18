using MNX.MonitoringCenter.Management.Contracts.Overclocking;

namespace MNX.MonitoringCenter.Management.Contracts.Presets;

/// <summary>
/// Модель для пресета
/// </summary>
public class PresetModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Название пресета
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Название майнинг устройства.
    /// </summary>
    public required string DeviceName { get; init; }

    /// <summary>
    /// Модель с разгоном
    /// </summary>
    public required IOverclockingModel Overclocking { get; init; }
}
