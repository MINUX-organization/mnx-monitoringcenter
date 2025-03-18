using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.RigsApi.Contracts.Args;

namespace MNX.MonitoringCenter.RigsApi.Contracts.FlightSheet;

/// <summary>
/// Статистика майнинга монеты на риге.
/// </summary>
public class RigCoinStatisticModel : CoinStatisticsModel
{
    /// <summary>
    /// Идентификатор майнера.
    /// </summary>
    public string? MinerName { get; init; }

    /// <summary>
    /// Идентификатор полетного листа.
    /// </summary>
    public string? FlightSheetName { get; init; }

    public new static List<RigCoinStatisticModel>? ConvertFrom<TSource>(TSource source)
    {
        if (source is RigCoinStatisticsModelArgs args)
        {
            return args.RigCoinStatistics
                .Aggregate(new List<RigCoinStatisticModel>(), (acc, rigCoinStatistic) =>
                {
                    if (args.MiningCombinations.TryGetValue(
                        (rigCoinStatistic.FlightSheetId, rigCoinStatistic.MinerId, rigCoinStatistic.CoinId),
                        out MiningCombinations? miningCombinations))
                    {
                        acc.Add(new RigCoinStatisticModel
                        {
                            CoinName = miningCombinations.Coin,
                            MinerName = miningCombinations.Miner,
                            FlightSheetName = miningCombinations.FlightSheet,
                            Shares = rigCoinStatistic.Shares,
                            HashRate = rigCoinStatistic.HashRate
                        });
                    }

                    return acc;
                });
        }

        return null;
    }

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
