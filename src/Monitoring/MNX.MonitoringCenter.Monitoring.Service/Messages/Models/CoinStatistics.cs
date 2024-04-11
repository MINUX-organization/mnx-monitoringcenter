using MNX.MonitoringCenter.Monitoring.Contracts.Models;

namespace MNX.MonitoringCenter.Monitoring.Service.Messages.Models;

/// <summary>
/// Статистика майнинга по монете.
/// </summary>
public class CoinStatistics
{
    /// <summary>
    /// Монета.
    /// </summary>
    public string Coin { get; set; }

    /// <summary>
    /// Алгоритм.
    /// </summary>
    public string Algorithm { get; set; }

    /// <summary>
    /// Скорость хеширования.
    /// </summary>
    public int HashRate { get; set; }

    /// <summary>
    /// Шеры.
    /// </summary>
    public SharesModel Shares { get; set; }

    public static CoinStatistics operator + (CoinStatistics first, CoinStatistics second)
    {
        if (first.Coin != second.Coin || first.Algorithm != second.Algorithm)
        {
            throw new ArgumentException("Нельзя складывать статистику разных монет");
        }

        return new CoinStatistics
        { 
            Coin = first.Coin,
            Algorithm = first.Algorithm,
            HashRate = first.HashRate + second.HashRate,
            Shares = first.Shares + second.Shares
        };
    }
}
