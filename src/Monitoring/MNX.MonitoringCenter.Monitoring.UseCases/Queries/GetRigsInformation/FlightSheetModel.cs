namespace MNX.MonitoringCenter.Monitoring.UseCases.Queries.GetRigsInformation;

/// <summary>
/// Модель полётного листа.
/// </summary>
public class FlightSheetModel
{
    /// <summary>
    /// Название.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Майнер.
    /// </summary>
    public string Miner { get; set; }

    /// <summary>
    /// Список монет.
    /// </summary>
    public List<CoinModel> Coins { get; set; }
}
