using MediatR;
using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Models;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries.GetRigsIds;
using System.Collections.Concurrent;

namespace MNX.MonitoringCenter.Monitoring.Service.Infrastructure;

/// <summary>
/// Счётчик динамических данных со всех ригов пользователя.
/// </summary>
public class UserRigsDynamicDataCounter : IDisposable
{
    /// <summary>
    /// Признак утилизированного объекта.
    /// </summary>
    private bool _disposedValue;

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    private readonly long _userId;

    /// <summary>
    /// Динамические данные ригов.
    /// </summary>
    /// <remarks>
    /// Ключ - идентификатор рига.
    /// Значение - динамические данные рига.
    /// </remarks>
    private ConcurrentDictionary<Guid, RigDynamicData> _rigsDynamicData = new();

    /// <summary>
    /// История динамических данных ригов.
    /// </summary>
    /// <remarks>
    /// Ключ - идентификатор рига.
    /// Значение - история динамических данных ригов.
    /// </remarks>
    private ConcurrentDictionary<Guid, List<(DateTimeOffset, RigDynamicData)>> _rigsDynamicDataHistory = new();

    /// <summary>
    /// Количество точек динамических данных в истории.
    /// </summary>
    private readonly int _dynamicDataPointCount;

    public UserRigsDynamicDataCounter(long userId, DynamicDataOptions dynamicDataOptions)
    {
        _userId = userId;
        _dynamicDataPointCount = dynamicDataOptions.PointCount;
    }

    /// <summary>
    /// Получить общую мощность.
    /// </summary>
    /// <returns> Общая мощность. </returns>
    public int GetTotalPower()
    {
        return _rigsDynamicData.Sum(x => x.Value.Power);
    }

    /// <summary>
    /// Получить общие шеры.
    /// </summary>
    /// <returns> Общие шеры. </returns>
    public SharesModel GetTotalShares()
    {
        return new SharesModel()
        {
            Accepted = _rigsDynamicData.Sum(rig => rig.Value.FlightSheetsInfo.Sum(x => x.Coins.Sum(coin => coin.Shares.Accepted))),
            Rejected = _rigsDynamicData.Sum(rig => rig.Value.FlightSheetsInfo.Sum(x => x.Coins.Sum(coin => coin.Shares.Rejected)))
        };
    }

    /// <summary>
    /// Получить общую статистику по монетам.
    /// </summary>
    /// <returns> Общая статистика по монетам. </returns>
    public List<CoinStatistics> GetTotalCoinStatistics()
    {
        var coinsStatistics = new Dictionary<string, CoinStatistics>(); // ключ - название монеты
        
        foreach(var flightSheets in _rigsDynamicData.Select(x => x.Value.FlightSheetsInfo))
        {
            foreach(var flightSheet in flightSheets)
            {
                foreach(var coin in flightSheet.Coins)
                {
                    var newCoinStatistics = new CoinStatistics()
                    {
                        Name = coin.Name,
                        Algorithm = coin.Algorithm,
                        HashRate = coin.HashRate,
                        Shares = coin.Shares
                    };

                    if (coinsStatistics.TryGetValue(coin.Name, out CoinStatistics? coinStatistics))
                    {
                        coinStatistics += newCoinStatistics;
                    }
                    else
                    {
                        coinsStatistics.Add(coin.Name, newCoinStatistics);
                    }
                }
            }
        }

        return coinsStatistics.Values.ToList();
    }

    /// <summary>
    /// Получить общую скорость хеширования.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Общая скорость хеширования. </returns>
    public int GetTotalHashRate(RigsDataSpecification specification)
    {
        int coinHashRate = 0;

        foreach (var flightSheets in _rigsDynamicData.Select(x => x.Value.FlightSheetsInfo))
        {
            foreach (var flightSheet in flightSheets)
            {
                coinHashRate += flightSheet.Coins
                                           .Where(coin => coin.Name == specification.ObservableCoin)
                                           .Sum(coin => coin.HashRate);
            }
        }

        return coinHashRate;
    }

    /// <summary>
    /// Получить историю по общей скорости хеширования.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> История скорости хеширования. </returns>
    public List<(DateTimeOffset, int)> GetTotalHashRateHistory(RigsDataSpecification specification)
    {
        List<(DateTimeOffset, int)> history = new(_dynamicDataPointCount);

        // получаем историю по ригу
        foreach (var rigHistory in _rigsDynamicDataHistory.Select(x => x.Value.OrderBy(x => x.Item2)))
        {
            // берём список полётных листов в конкретный момент времени
            foreach (var flightSheets in rigHistory.Select(x => x.Item2.FlightSheetsInfo))
            {
                int counter = 0;
                int coinHashRate = 0;

                // полётный лист в конкретный момент времени на конкретном риге
                foreach (var flightSheet in flightSheets)
                {
                    coinHashRate += flightSheet.Coins
                                               .Where(coin => coin.Name == specification.ObservableCoin)
                                               .Sum(coin => coin.HashRate);
                }

                if (history.Count <= counter)
                {
                    history.Add((rigHistory.ElementAt(counter).Item1, coinHashRate));
                }
                else
                {
                    var pair = history[counter];
                    pair.Item2 += coinHashRate;
                    history[counter] = pair;
                }

                counter++;
            }
        }

        return history;
    }

    /// <summary>
    /// Получить динамические данные с ригов.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Динамические данные ригов. </returns>
    public async Task<List<RigDynamicData>> GetRigsDynamicData(IMediator mediator,
                                                               RigsDataSpecification specification)
    {
        var ids = await mediator.Send(new GetRigsIdsQuery(new Specification(_userId,
                                                                            specification.RigsSearchString,
                                                                            specification.FilterString,
                                                                            specification.FilterArguments)));

        return _rigsDynamicData.Values.Where(x => ids.Contains(x.Id)).ToList();
    }

    /// <summary>
    /// Обновить данные.
    /// </summary>
    /// <param name="rigsDynamicData"> Динамические данные ригов. </param>
    public void UpdateData(List<RigDynamicData> rigsDynamicData)
    {
        foreach(var rig in rigsDynamicData)
        {
            _rigsDynamicData.AddOrUpdate(rig.Id, rig, (_, value) => value = rig);

            _rigsDynamicDataHistory.AddOrUpdate(rig.Id,
                                                new List<(DateTimeOffset, RigDynamicData)>() { (DateTimeOffset.Now, rig) },
                                                (_, value) =>
                                                {
                                                    if (value.Count >= _dynamicDataPointCount)
                                                    {
                                                        value.RemoveAt(0);
                                                    }

                                                    value.Add((DateTimeOffset.Now, rig));
                                                    return value;
                                                });
        }
    }

    /// <summary>
    /// Освободить ресурсы.
    /// </summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Освободить ресурсы.
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                foreach (var rigData in _rigsDynamicData)
                {
                    _rigsDynamicData.TryRemove(rigData.Key, out var _);
                    _rigsDynamicDataHistory.TryRemove(rigData.Key, out var _);
                }
            }

            _rigsDynamicData = null!;
            _rigsDynamicDataHistory = null!;
            _disposedValue = true;
        }
    }
}
