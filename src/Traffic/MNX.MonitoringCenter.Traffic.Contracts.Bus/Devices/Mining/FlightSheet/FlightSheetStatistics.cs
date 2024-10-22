namespace MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining.FlightSheet;

/// <summary>
/// Статистика полётного листа.
/// </summary>
public class FlightSheetStatistics
{
    /// <summary>
    /// Идентификатор полётного листа.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Майнер.
    /// </summary>
    public Guid MinerId { get; init; }

    /// <summary>
    /// Статистика майнинга монет.
    /// </summary>
    public List<CoinStatistics> Coins { get; init; } = new(0);
}

