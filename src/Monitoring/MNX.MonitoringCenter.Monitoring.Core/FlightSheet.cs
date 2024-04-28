namespace MNX.MonitoringCenter.Monitoring.Core;

/// <summary>
/// Полетный лист.
/// </summary>
public class FlightSheet
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Название.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Список монет.
    /// </summary>
    public List<FlightSheetCoin> Coins { get; set; }
}
