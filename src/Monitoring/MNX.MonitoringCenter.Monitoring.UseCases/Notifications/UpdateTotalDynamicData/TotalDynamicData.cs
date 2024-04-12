using MNX.MonitoringCenter.Monitoring.Contracts.Models;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Notifications.UpdateTotalDynamicData;

/// <summary>
/// Обобщённые динамические данные.
/// </summary>
public class TotalDynamicData
{
    /// <summary>
    /// Мощность.
    /// </summary>
    public int Power { get; }

    /// <summary>
    /// Шеры.
    /// </summary>
    public SharesModel Shares { get; }

    /// <summary>
    /// Статистика по монетам.
    /// </summary>
    public List<CoinStatistics> CoinsStatistics { get; }

    public TotalDynamicData(int power, SharesModel shares, List<CoinStatistics> coinsStatistics)
    {
        Power = power;
        Shares = shares;
        CoinsStatistics = coinsStatistics;
    }
}
