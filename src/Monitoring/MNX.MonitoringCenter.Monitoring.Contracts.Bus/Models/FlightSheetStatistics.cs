namespace MNX.MonitoringCenter.Monitoring.Contracts.Bus.Models;

/// <summary>
/// Статистика полётного листа.
/// </summary>
public class FlightSheetStatistics
{
    /// <summary>
    /// Название.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Статистика майнинга монеты.
    /// </summary>
    public List<CoinStatistics> Coins { get; set; }
}

