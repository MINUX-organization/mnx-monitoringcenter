using MediatR;
using MNX.MonitoringCenter.Monitoring.Contracts.Models;
using MNX.MonitoringCenter.Monitoring.Service.Messages.Models;
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

    public UserRigsDynamicDataCounter(long userId)
    {
        _userId = userId;
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
            Accepted = _rigsDynamicData.Sum(rig => rig.Value.FlightSheetsInfo.Sum(x => x.Shares.Accepted)),
            Rejected = _rigsDynamicData.Sum(rig => rig.Value.FlightSheetsInfo.Sum(x => x.Shares.Rejected))
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
                var newCoinStatistics = new CoinStatistics()
                {
                    Coin = flightSheet.Coin,
                    Algorithm = flightSheet.Algorithm,
                    HashRate = flightSheet.HashRate,
                    Shares = flightSheet.Shares
                };

                if (coinsStatistics.TryGetValue(flightSheet.Coin, out CoinStatistics? coinStatistics))
                {
                    coinStatistics += newCoinStatistics;
                }
                else
                {
                    coinsStatistics.Add(flightSheet.Coin, newCoinStatistics);
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
    public int GetTotalHashRate(RigsDynamicDataSpecification specification)
    {
        int coinHashRate = 0;

        foreach (var flightSheets in _rigsDynamicData.Select(x => x.Value.FlightSheetsInfo))
        {
            foreach (var flightSheet in flightSheets.Where(x => x.Coin == specification.ObservableCoin))
            {
                coinHashRate += flightSheet.HashRate;
            }
        }

        return coinHashRate;
    }

    /// <summary>
    /// Получить динамические данные с ригов.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Динамические данные ригов. </returns>
    public async Task<List<RigDynamicData>> GetRigsDynamicData(IMediator mediator,
                                                               RigsDynamicDataSpecification specification)
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
                                                    if (value.Count >= 300)
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
