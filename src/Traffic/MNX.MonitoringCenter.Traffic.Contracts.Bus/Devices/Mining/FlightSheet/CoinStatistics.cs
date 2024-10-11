namespace MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining.FlightSheet;

/// <summary>
/// Статистика майнинга монеты.
/// </summary>
public class CoinStatistics
{
    /// <summary>
    /// Идентификатор монеты.
    /// </summary>
    public Guid CoinId { get; init; }

    /// <summary>
    /// Скорость хеширования.
    /// </summary>
    public int HashRate { get; set; }

    /// <summary>
    /// Решения.
    /// </summary>
    public required SharesModel Shares { get; set; }

    /// <summary>
    /// Оператор сложения.
    /// </summary>
    /// <param name="first"> Первое слагаемое. </param>
    /// <param name="second"> Второе слагаемое. </param>
    /// <returns> Сумма статистики монет. </returns>
    /// <exception cref="ArgumentException"> Нельзя складывать статистику разных монет. </exception>
    public static CoinStatistics operator +(CoinStatistics first, CoinStatistics second)
    {
        if (first.CoinId != second.CoinId)
        {
            throw new ArgumentException("Нельзя складывать статистику разных монет");
        }

        return new CoinStatistics
        {
            CoinId = first.CoinId,
            HashRate = first.HashRate + second.HashRate,
            Shares = first.Shares + second.Shares
        };
    }
}
