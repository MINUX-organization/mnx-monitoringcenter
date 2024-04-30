namespace MNX.MonitoringCenter.Monitoring.Core;

/// <summary>
/// Монета.
/// </summary>
public class Coin
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Полное название.
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// Короткое название.
    /// </summary>
    public string ShortName { get; set; }
}
