using MNX.MonitoringCenter.RigsApi.Contracts.Abstractions;
using MNX.MonitoringCenter.RigsApi.Contracts.Args;
using MNX.MonitoringCenter.RigsApi.Contracts.FlightSheet;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining.FlightSheet;

namespace MNX.MonitoringCenter.RigsApi.Contracts.Streams;

public class MonitoringIndicatorsStreamResponse : IConverterFrom<MonitoringIndicatorsStreamResponse>
{
    public int TotalPower { get; set; }

    public int TotalHashrate { get; set; }

    public SharesModel? TotalShares { get; set; }

    public IEnumerable<CoinStatisticsModel>? TotalCoinStatistics { get; set; }

    public IEnumerable<GeneralRigIndicatorsModel>? MiningRigsIndicators { get; set; }

    public static MonitoringIndicatorsStreamResponse? ConvertFrom<TSource>(TSource source)
    {

        if (source is MonitoringIndicatorsStreamResponseArgs monitoringIndicatorsResponseArgs)
        {
            var hardwareIndicatorsRigs = monitoringIndicatorsResponseArgs.RigDynamicHardwareIndicators.ToDictionary(rig => rig.RigId);

            IEnumerable<GeneralRigIndicatorsModel> rigsDynamicMiningIndicators = monitoringIndicatorsResponseArgs
                .RigsDynamicMiningIndicators
                .Select(rig =>
                {
                    var rigCoinStatisticsModelArgs = new RigCoinStatisticsModelArgs
                    {
                        RigCoinStatistics = rig.TotalCoinStatistics,
                        MiningCombinations = monitoringIndicatorsResponseArgs.MiningCombinations
                    };

                    return new GeneralRigIndicatorsModel
                    {
                        RigId = rig.RigId,
                        RigName = monitoringIndicatorsResponseArgs.Rigs.GetValueOrDefault(rig.RigId)?.Name,
                        AverageMiningDevicesFanSpeed = hardwareIndicatorsRigs.GetValueOrDefault(rig.RigId)?.AverageMiningDevicesFanSpeed,
                        AverageMiningDevicesTemperature = hardwareIndicatorsRigs.GetValueOrDefault(rig.RigId)?.AverageMiningDevicesTemperature,
                        TotalPower = hardwareIndicatorsRigs.GetValueOrDefault(rig.RigId)?.TotalPower,
                        InternetSpeed = hardwareIndicatorsRigs.GetValueOrDefault(rig.RigId)?.InternetSpeed,
                        OnlineState = hardwareIndicatorsRigs.GetValueOrDefault(rig.RigId)?.OnlineState,
                        TotalShares = rig.TotalShares,
                        TotalHashRate = rig.TotalHashRate,
                        MiningUpTime = rig.MiningUpTime,
                        BootedUpTime = hardwareIndicatorsRigs.GetValueOrDefault(rig.RigId)?.BootedUpTime,
                        TotalCoinStatistics = RigCoinStatisticModel.ConvertFrom(rigCoinStatisticsModelArgs)
                                                                    ?? new List<RigCoinStatisticModel>()
                    };
                });

            return new MonitoringIndicatorsStreamResponse
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