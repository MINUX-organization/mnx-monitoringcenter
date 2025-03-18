using AutoMapper;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.RigsApi.Contracts.Abstractions;
using MNX.MonitoringCenter.RigsApi.Contracts.Args;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining.FlightSheet;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts.Devices.FlightSheet;

namespace MNX.MonitoringCenter.RigsApi.Contracts.FlightSheet;

/// <summary>
/// Статистика полётного листа.
/// </summary>
public class FlightSheetStatisticsModel : IConverterFrom<FlightSheetStatisticsModel>
{
    /// <summary>
    /// Идентификатор полётного листа.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Название полётного листа.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Майнер.
    /// </summary>
    public string? MinerName { get; init; }

    /// <summary>
    /// Статистика майнинга монет.
    /// </summary>
    public List<CoinStatisticsModel> Coins { get; init; } = new(0);

    public static FlightSheetStatisticsModel? ConvertFrom<TSource>(TSource source)
    {
        if (source is FlightSheetStatisticsModelArgs args)
        {
            if (args.FlightSheetStatistics is null)
            {
                return null;
            }

            (Guid flightSheetId, Guid minerId, List<CoinStatistics> coinStatistics) = args.FlightSheetStatistics;

            var miningCombinationsKey = (flightSheetId, minerId, coinStatistics.First().CoinId);
            var (flightSheetName, minerName, _) = args.MiningCombinations
                .GetValueOrDefault(miningCombinationsKey) ?? new MiningCombinations();

            var coinStatisticModel = coinStatistics
                .Aggregate(new List<CoinStatisticsModel>(), (acc, coinStatistic) =>
                {
                    if (args.MiningCombinations.TryGetValue(
                        (flightSheetId, minerId, coinStatistic.CoinId),
                        out MiningCombinations? miningCombinations))
                    {
                        acc.Add(new CoinStatisticsModel
                        {
                            Shares = coinStatistic.Shares,
                            CoinName = miningCombinations.Coin,
                            HashRate = coinStatistic.HashRate
                        });
                    }

                    return acc;
                });

            return new FlightSheetStatisticsModel
            {
                Id = flightSheetId,
                Name = flightSheetName,
                MinerName = minerName,
                Coins = coinStatisticModel
            };
        }

        return null;
    }
}