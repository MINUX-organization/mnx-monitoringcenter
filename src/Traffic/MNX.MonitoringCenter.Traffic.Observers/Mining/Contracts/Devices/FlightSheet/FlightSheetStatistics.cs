namespace MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts.Devices.FlightSheet;

/// <summary>
/// Статистика полётного листа.
/// </summary>
public class FlightSheetStatistics
{
    /// <summary>
    /// Идентификатор полётного листа.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор майнера.
    /// </summary>
    public Guid MinerId { get; set; }

    /// <summary>
    /// Название майнера.
    /// </summary>
    public required string MinerName { get; set; }

    /// <summary>
    /// Статистика майнинга монет.
    /// </summary>
    public List<CoinStatistics> Coins { get; set; } = new(0);
}
