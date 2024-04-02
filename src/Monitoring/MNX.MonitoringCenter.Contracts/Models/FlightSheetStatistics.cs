namespace MNX.MonitoringCenter.Monitoring.Contracts.Models;

// <summary>
/// Статистика полётного листа.
/// </summary>
public class FlightSheetStatistics
{
    /// <summary>
    /// Название.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Монета.
    /// </summary>
    public string Coin { get; set; }

    /// <summary>
    /// Майнер.
    /// </summary>
    public string Miner { get; set; }

    /// <summary>
    /// Скорость хеширования.
    /// </summary>
    public ParameterModelWithMeasureUnit HashRate { get; set; }

    /// <summary>
    /// Шеры.
    /// </summary>
    public SharesModel Shares { get; set; }
}

