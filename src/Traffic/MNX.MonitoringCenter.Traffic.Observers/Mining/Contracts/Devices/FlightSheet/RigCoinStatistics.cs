namespace MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts.Devices.FlightSheet;

/// <summary>
/// Статистика майнинга монеты на риге.
/// </summary>
public class RigCoinStatistics : CoinStatistics
{
    /// <summary>
    /// Идентификатор майнера.
    /// </summary>
    public Guid MinerId { get; init; }

    /// <summary>
    /// Идентификатор полетного листа.
    /// </summary>
    public Guid FlightSheetId { get; init; }

    /// <summary>
    /// Оператор сложения.
    /// </summary>
    /// <param name="first"> Первое слагаемое. </param>
    /// <param name="second"> Второе слагаемое. </param>
    /// <returns> Сумма статистики монет. </returns>
    /// <exception cref="ArgumentException"> Нельзя складывать статистику разных монет. </exception>
    public static RigCoinStatistics operator +(RigCoinStatistics first, RigCoinStatistics second)
    {
        if (first.CoinId != second.CoinId)
        {
            throw new ArgumentException("Нельзя складывать статистику разных монет");
        }

        return new RigCoinStatistics
        {
            CoinId = first.CoinId,
            MinerId = first.MinerId,
            FlightSheetId = second.FlightSheetId,
            HashRate = first.HashRate + second.HashRate,
            Shares = first.Shares + second.Shares
        };
    }
}
