namespace MNX.MonitoringCenter.Monitoring.Contracts.Bus.Models;

/// <summary>
/// Статистика майнинга монеты.
/// </summary>
public class CoinStatistics
{
    /// <summary>
    /// Название.
    /// </summary>
    public string Name { get; set; }

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

    public static CoinStatistics operator +(CoinStatistics first, CoinStatistics second)
    {
        if (first.Name != second.Name || first.Algorithm != second.Algorithm)
        {
            throw new ArgumentException("Нельзя складывать статистику разных монет");
        }

        return new CoinStatistics
        {
            Name = first.Name,
            Algorithm = first.Algorithm,
            Miner = first.Miner,
            HashRate = first.HashRate + second.HashRate,
            Shares = first.Shares + second.Shares
        };
    }
}
