using MNX.MonitoringCenter.RigsApi.Contracts.Abstractions;
using MNX.MonitoringCenter.RigsApi.Contracts.Rigs;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining.FlightSheet;

namespace MNX.MonitoringCenter.RigsApi.Contracts.Streams.Monitoring;

public class MonitoringIndicatorsResponse : IConverterFrom<MonitoringIndicatorsResponse>
{
    public int TotalPower { get; set; }

    public int TotalHashrate { get; set; }

    public SharesModel TotalShares { get; set; }

    public IEnumerable<CoinStatisticModel> TotalCoinStatistics { get; set; }

    public IEnumerable<MiningRigIndicatorsModel> MiningRigsIndicators { get; set; }

    public static MonitoringIndicatorsResponse? ConvertFrom<TSource>(TSource source)
    {
        if (source is MonitoringIndicatorsResponseArgs monitoringIndicatorsResponseArgs)
        {
            /// TODO: Add mining combinations
            IEnumerable<MiningRigIndicatorsModel> rigsDynamicMiningIndicators = monitoringIndicatorsResponseArgs
                .RigsDynamicMiningIndicators
                .Select(rig => new MiningRigIndicatorsModel
                {
                    RigId = rig.RigId,
                    RigName = "Dummy",
                    TotalShares = rig.TotalShares,
                    TotalHashRate = rig.TotalHashRate,
                    MiningUpTime = rig.MiningUpTime,
                    TotalCoinStatistics = rig.TotalCoinStatistics
                });

            return new MonitoringIndicatorsResponse
            {
                TotalShares = monitoringIndicatorsResponseArgs.TotalShares,
                TotalPower = monitoringIndicatorsResponseArgs.TotalPower,
                TotalHashrate = monitoringIndicatorsResponseArgs.TotalHashrate,
                MiningRigsIndicators = rigsDynamicMiningIndicators
            };
        }

        return null;
    }
}