using MNX.MonitoringCenter.RigsApi.Contracts.Rigs;

/// <summary>
/// Статистика майнинга монеты на риге.
/// </summary>
public class RigCoinStatisticModel : CoinStatisticModel
{
    /// <summary>
    /// Идентификатор майнера.
    /// </summary>
    public string MinerName { get; init; } = string.Empty;

    /// <summary>
    /// Идентификатор полетного листа.
    /// </summary>
    public string FlightSheetName { get; init; } = string.Empty;

    /// <summary>
    /// Оператор сложения.
    /// </summary>
    /// <param name="first"> Первое слагаемое. </param>
    /// <param name="second"> Второе слагаемое. </param>
    /// <returns> Сумма статистики монет. </returns>
    /// <exception cref="ArgumentException"> Нельзя складывать статистику разных монет. </exception>
    public static RigCoinStatisticModel operator +(RigCoinStatisticModel first, RigCoinStatisticModel second)
    {
        if (first.CoinName.Contains(second.CoinName))
        {
            throw new ArgumentException("Нельзя складывать статистику разных монет");
        }

        return new RigCoinStatisticModel
        {
            CoinName = first.CoinName,
            MinerName = first.MinerName,
            FlightSheetName = second.FlightSheetName,
            HashRate = first.HashRate + second.HashRate,
            Shares = first.Shares + second.Shares
        };
    }
}
