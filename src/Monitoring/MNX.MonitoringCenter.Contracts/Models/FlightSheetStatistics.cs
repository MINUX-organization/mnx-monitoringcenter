namespace MNX.MonitoringCenter.Monitoring.Contracts.Models;

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
    /// Монета.
    /// </summary>
    public string Coin { get; set; }

    /// <summary>
    /// Алгоритм.
    /// </summary>
    public string Algorithm { get; set; }

    /// <summary>
    /// Майнер.
    /// </summary>
    public string Miner { get; set; }

    /// <summary>
    /// Скорость хеширования.
    /// </summary>
    public int HashRate { get; set; }

    /// <summary>
    /// Шеры.
    /// </summary>
    public SharesModel Shares { get; set; }
}

