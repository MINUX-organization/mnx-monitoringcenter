namespace MNX.MonitoringCenter.Monitoring.Core;

/// <summary>
/// Монета полётного листа.
/// </summary>
public class FlightSheetCoin
{
    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Монета.
    /// </summary>
    public Coin Coin { get; set; }

    /// <summary>
    /// Майнер.
    /// </summary>
    public string Miner { get; set; }
}
