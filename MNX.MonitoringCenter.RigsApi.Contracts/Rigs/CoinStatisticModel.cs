using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining.FlightSheet;

namespace MNX.MonitoringCenter.RigsApi.Contracts.Rigs;

/// <summary>
/// Статистика майнинга монеты.
/// </summary>
public class CoinStatisticModel
{
    /// <summary>
    /// Идентификатор монеты.
    /// </summary>
    public string CoinName { get; init; } = string.Empty;

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
    public static CoinStatisticModel operator +(CoinStatisticModel first, CoinStatisticModel second)
    {
        if (first.CoinName.Contains(second.CoinName))
        {
            throw new ArgumentException("Нельзя складывать статистику разных монет");
        }

        return new CoinStatisticModel
        {
            CoinName = first.CoinName,
            HashRate = first.HashRate + second.HashRate,
            Shares = first.Shares + second.Shares
        };
    }
}
