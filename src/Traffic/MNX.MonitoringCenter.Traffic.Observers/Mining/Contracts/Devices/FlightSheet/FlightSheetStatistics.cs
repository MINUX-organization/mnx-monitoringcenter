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
    /// Майнер.
    /// </summary>
    public Guid MinerId { get; set; }

    /// <summary>
    /// Статистика майнинга монет.
    /// </summary>
    public List<CoinStatistics> Coins { get; set; } = new(0);

    /// <summary>
    /// Название майнера.
    /// </summary>
    public required string MinerName { get; set; }

    /// <summary>
    /// Деструктор.
    /// </summary>
    public void Deconstruct(out Guid flightSheetId, out Guid minerId, out List<CoinStatistics> coins)
    {
        flightSheetId = Id;
        minerId = MinerId;
        coins = Coins;
    }
}

