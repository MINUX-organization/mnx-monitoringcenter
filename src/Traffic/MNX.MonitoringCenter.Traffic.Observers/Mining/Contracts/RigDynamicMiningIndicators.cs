using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining.FlightSheet;
using MNX.MonitoringCenter.Traffic.Observers.Abstractions;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts.Devices.Abstractions;

namespace MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts;

/// <summary>
/// Динамические показатели майнинга рига.
/// </summary>
public class RigDynamicMiningIndicators : IRigIndicators<IDeviceDynamicMiningIndicators>
{
    /// <summary>
    /// Уникальный идентификатор рига.
    /// </summary>
    public Guid RigId { get; init; }

    /// <summary>
    /// Время майнинга с момента последнего включения.
    /// </summary>
    public DateTime MiningUpTime { get; init; }

    /// <summary>
    /// Время работы рига с момента последнего включения.
    /// </summary>
    public DateTime BootedUpTime { get; init; }

    /// <summary>
    /// Общее кол-во решений.
    /// </summary>
    private SharesModel? _totalShares;
    public SharesModel TotalShares
    {
        get
        {
            return _totalShares ??= new SharesModel()
            {
                Accepted = Devices.Sum(x =>
                {
                    return x.FlightSheet is not null
                        ? x.FlightSheet.Coins.Sum(coin => coin.Shares.Accepted)
                        : 0;
                }),

                Rejected = Devices.Sum(x =>
                {
                    return x.FlightSheet is not null
                        ? x.FlightSheet.Coins.Sum(coin => coin.Shares.Rejected)
                        : 0;
                })
            };
        }
    }

    /// <summary>
    /// Общая скорость хеширования.
    /// </summary>
    private int? _totalHashRate;
    public int TotalHashRate
    {
        get
        {
            return _totalHashRate ??= Devices.Sum(device =>
            {
                if (device.FlightSheet is not null)
                {
                    return device.FlightSheet.Coins.Sum(coin => coin.HashRate);
                }
                return 0;
            });
        }
    }

    /// <summary>
    /// Получить обобщённую статистику по монетам.
    /// </summary>
    private List<RigCoinStatistics>? _totalCoinsStatistics;
    public List<RigCoinStatistics> TotalCoinStatistics
    {
        get
        {
            if (_totalCoinsStatistics is null)
            {
                var coinsStatistics = new Dictionary<Guid, RigCoinStatistics>(); // ключ - идентификатор монеты

                foreach (var device in Devices)
                {
                    if (device.FlightSheet is null)
                    {
                        continue;
                    }

                    foreach (var coin in device.FlightSheet.Coins)
                    {
                        if (coinsStatistics.TryGetValue(coin.CoinId, out RigCoinStatistics? statistics))
                        {
                            statistics.HashRate += coin.HashRate;
                            statistics.Shares += coin.Shares;
                        }
                        else
                        {
                            var newCoinStatistics = new RigCoinStatistics()
                            {
                                CoinId = coin.CoinId,
                                MinerId = device.FlightSheet.MinerId,
                                FlightSheetId = device.FlightSheet.Id,
                                HashRate = coin.HashRate,
                                Shares = coin.Shares
                            };

                            coinsStatistics.Add(coin.CoinId, newCoinStatistics);
                        }
                    }
                }

                _totalCoinsStatistics = coinsStatistics.Values.ToList();
            }

            return _totalCoinsStatistics;
        }
    }

    /// <summary>
    /// Динамические показатели майнинга устройств.
    /// </summary>
    public List<IDeviceDynamicMiningIndicators> Devices { get; init; } = new(0);
}
