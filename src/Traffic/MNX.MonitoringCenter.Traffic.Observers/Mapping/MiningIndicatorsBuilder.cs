using AutoMapper;
using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts.FlightSheet;
using MNX.MonitoringCenter.Management.Contracts.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Queries;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice.Queries;
using MNX.MonitoringCenter.Traffic.Contracts.Bus;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts;

namespace MNX.MonitoringCenter.Traffic.Observers.Mapping;

/// <summary>
/// Билдер показателей майнига.
/// </summary>
public class MiningIndicatorsBuilder
{
    private readonly IMapper _mapper;

    private readonly IMediator _mediator;

    public MiningIndicatorsBuilder(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Построить показатели майнинга.
    /// </summary>
    /// <param name="indicatorsList"> Список показателей. </param>
    /// <returns> Показатели майнинга. </returns>
    public async Task<IEnumerable<RigDynamicMiningIndicators>> Build(IEnumerable<RigDynamicIndicators> indicatorsList)
    {
        var indicators = new List<RigDynamicMiningIndicators>(indicatorsList.Count());

        foreach (var rigIndicators in indicatorsList)
        {
            var miningIndicators = _mapper.Map<RigDynamicMiningIndicators>(rigIndicators);

            foreach (var deviceIndicators in miningIndicators.Devices)
            {
                var deviceGettingResult = await _mediator.Send(
                    new GetDeviceByIdQuery(deviceIndicators.DeviceId, rigIndicators.UserId));

                if (!TryGetValue(deviceGettingResult, out MiningDeviceModel device))
                {
                    continue;
                }

                if (!device.FlightSheetId.HasValue)
                {
                    continue;
                }

                var flightSheetGettingResult = await _mediator.Send(
                        new GetFlightSheetByIdQuery(device.FlightSheetId.Value, rigIndicators.UserId));

                if (!TryGetValue(flightSheetGettingResult, out FlightSheetModel flightSheet))
                {
                    continue;
                }

                // todo: перейти на сравнение по типу устройства.
                var flightSheetTarget = flightSheet.Targets.First(x => x.Miner.Name == deviceIndicators.FlightSheet!.MinerName);

                deviceIndicators.FlightSheet!.Id = flightSheet.Id;
                deviceIndicators.FlightSheet!.MinerId = flightSheetTarget.Miner.Id;

                for (int i = 0; i < deviceIndicators.FlightSheet.Coins.Count; i++)
                {
                    var coinMetrics = deviceIndicators.FlightSheet.Coins[i];
                    coinMetrics.CoinId = flightSheetTarget.MiningConfig.CoinConfigs[i].Pool!.CryptocurrencyId;
                }
            }

            indicators.Add(miningIndicators);
        }

        return indicators;
    }

    private static bool TryGetValue<T>(Result<T> result, out T value)
    {
        if (result.IsSuccess)
        {
            value = result.GetValue();
            return true;
        }

        value = default!;
        return false;
    }
}
