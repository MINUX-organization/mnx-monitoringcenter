namespace MNX.MonitoringCenter.Monitoring.UseCases.Queries.GetRigsInformation;

/// <summary>
/// Модель монеты.
/// </summary>
public class CoinModel
{
    /// <summary>
    /// Полное название.
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// Короткое название.
    /// </summary>
    public string ShortName { get; set; }

    /// <summary>
    /// Майнер.
    /// </summary>
    public string Miner { get; set; }
}
