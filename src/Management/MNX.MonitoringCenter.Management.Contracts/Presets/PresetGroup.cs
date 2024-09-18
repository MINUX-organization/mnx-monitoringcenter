namespace MNX.MonitoringCenter.Management.Contracts.Presets;

/// <summary>
/// Группа пресетов.
/// </summary>
public class PresetGroup
{
    /// <summary>
    /// Название группы.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Пресеты.
    /// </summary>
    public List<PresetModel> Presets { get; set; } = new(0);
}
