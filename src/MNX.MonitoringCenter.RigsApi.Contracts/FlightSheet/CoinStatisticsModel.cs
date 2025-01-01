using MNX.MonitoringCenter.RigsApi.Contracts.Abstractions;
using MNX.MonitoringCenter.RigsApi.Contracts.Args;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining.FlightSheet;

namespace MNX.MonitoringCenter.RigsApi.Contracts.FlightSheet;

/// <summary>
/// Статистика майнинга монеты.
/// </summary>
public class CoinStatisticsModel : IConverterFrom<List<CoinStatisticsModel>>
{
    /// <summary>
    /// Идентификатор монеты.
    /// </summary>
    public string? CoinName { get; init; } = string.Empty;

    /// <summary>
    /// Скорость хеширования.
    /// </summary>
    public int HashRate { get; set; }

    /// <summary>
    /// Решения.
    /// </summary>
    public required SharesModel Shares { get; set; }

    public static List<CoinStatisticsModel>? ConvertFrom<TSource>(TSource source)
    {
        if (source is CoinStatisticsModelArgs args)
        {
            return args.CoinStatistics.Select(coinStatistic => new CoinStatisticsModel
            {
                CoinName = args.CoinNames.GetValueOrDefault(coinStatistic.CoinId),
                Shares = coinStatistic.Shares,
                HashRate = coinStatistic.HashRate
            })
            .ToList();
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
    public static CoinStatisticsModel operator +(CoinStatisticsModel first, CoinStatisticsModel second)
    {
        if (first.CoinName.Contains(second.CoinName))
        {
            throw new ArgumentException("Нельзя складывать статистику разных монет");
        }

        return new CoinStatisticsModel
        {
            CoinName = first.CoinName,
            HashRate = first.HashRate + second.HashRate,
            Shares = first.Shares + second.Shares
        };
    }
}
