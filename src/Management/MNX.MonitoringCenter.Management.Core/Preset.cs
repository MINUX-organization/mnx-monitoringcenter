namespace MNX.MonitoringCenter.Management.Core;

/// <summary>
/// Пресет
/// </summary>
public class Preset
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Название пресета
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Название GPU
    /// </summary>
    public string GpuName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Guid OverclockingId { get; set; }

    /// <summary>
    /// Модель с разгоном
    /// </summary>
    public Overclocking? Overclocking { get; set; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long UserId { get; set; }
}