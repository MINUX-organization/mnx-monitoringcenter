using MNX.MonitoringCenter.Inventory.Contracts.Devices.CountDevices;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.RigsApi.Contracts.Abstractions;
using MNX.MonitoringCenter.RigsApi.Contracts.Args;
using MNX.MonitoringCenter.RigsApi.Contracts.FlightSheet;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining.FlightSheet;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Network;
using MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts.Devices.FlightSheet;

namespace MNX.MonitoringCenter.RigsApi.Contracts.Streams;

public class MonitoringIndicatorsStreamResponse : IConverterFrom<MonitoringIndicatorsStreamResponse>
{
    /// <summary>
    /// Общая мощность.
    /// </summary>
    public int TotalPower { get; set; }

    /// <summary>
    /// Общая хэшрейт.
    /// </summary>
    public int TotalHashrate { get; set; }

    /// <summary>
    /// Общие решения.
    /// </summary>
    public SharesModel? TotalShares { get; set; }

    /// <summary>
    /// Общее количество устройств.
    /// </summary>
    public ModelWithCountDevices? TotalDevices { get; set; }

    /// <summary>
    /// Общая статистика монет.
    /// </summary>
    public IEnumerable<CoinStatisticsModel>? TotalCoinStatistics { get; set; }

    /// <summary>
    /// Показатели ригов.
    /// </summary>
    public IEnumerable<GeneralRigIndicatorsModel>? MiningRigsIndicators { get; set; }

    public static MonitoringIndicatorsStreamResponse? ConvertFrom<TSource>(TSource source)
    {
        if (source is MonitoringIndicatorsStreamResponseArgs monitoringArgs)
        {
            var hardwareIndicatorsRigs = monitoringArgs.RigDynamicHardwareIndicators?
                .ToDictionary(rig => rig.RigId) ?? new Dictionary<Guid, RigDynamicHardwareIndicators>();

            var totalCpusCount = 0;
            var totalGpusCount = 0;
            var cpuManufacturerCounts = new Dictionary<string, int>();
            var gpuManufacturerCounts = new Dictionary<string, int>();
            var coinNames = new Dictionary<Guid, string>();

            var rigsIndicators = monitoringArgs.RigsDynamicMiningIndicators?
                .Select(rig =>
            {
                var inventoryRig = monitoringArgs.Rigs.GetValueOrDefault(rig.RigId);

                totalCpusCount += inventoryRig?.CountDevices.TotalCpusCount ?? 0;
                foreach (var (manufacturer, count) in inventoryRig?.CountDevices.TotalCpusCountGroupedByManufacturer 
                    ?? Enumerable.Empty<KeyValuePair<string, int>>())
                {
                    cpuManufacturerCounts[manufacturer] = 
                        cpuManufacturerCounts.GetValueOrDefault(manufacturer, 0) + count;
                }

                totalGpusCount += inventoryRig?.CountDevices.TotalGpusCount ?? 0;
                foreach (var (manufacturer, count) in inventoryRig?.CountDevices.TotalGpusCountGroupedByManufacturer 
                    ?? Enumerable.Empty<KeyValuePair<string, int>>())
                {
                    gpuManufacturerCounts[manufacturer] = 
                        gpuManufacturerCounts.GetValueOrDefault(manufacturer, 0) + count;
                }

                var rigTotalCoinStatistics = rig.TotalCoinStatistics
                    .Aggregate(new List<RigCoinStatisticModel>(), (acc, rigCoinStatistic) =>
                    {
                        if (monitoringArgs.MiningCombinations.TryGetValue(
                            (rigCoinStatistic.FlightSheetId, rigCoinStatistic.MinerId, rigCoinStatistic.CoinId),
                            out MiningCombination? miningCombinations))
                        {
                            coinNames.Add(rigCoinStatistic.CoinId, miningCombinations?.Coin ?? "Unknown Coin");
                            acc.Add(new RigCoinStatisticModel
                            {
                                CoinName = miningCombinations?.Coin ?? "Unknown Coin",
                                MinerName = miningCombinations?.Miner ?? "Unknown Miner",
                                FlightSheetName = miningCombinations?.FlightSheet ?? "Unknown FlightSheet",
                                Shares = rigCoinStatistic.Shares,
                                HashRate = rigCoinStatistic.HashRate
                            });
                        }

                        return acc;
                    });

                return new GeneralRigIndicatorsModel
                {
                    RigId = rig.RigId,
                    RigName = inventoryRig?.Name ?? "Unknown Rig",
                    AverageMiningDevicesFanSpeed = hardwareIndicatorsRigs.GetValueOrDefault(rig.RigId)?.AverageMiningDevicesFanSpeed ?? 0,
                    AverageMiningDevicesTemperature = hardwareIndicatorsRigs.GetValueOrDefault(rig.RigId)?.AverageMiningDevicesTemperature ?? 0,
                    TotalPower = hardwareIndicatorsRigs.GetValueOrDefault(rig.RigId)?.TotalPower ?? 0,
                    InternetSpeed = hardwareIndicatorsRigs.GetValueOrDefault(rig.RigId)?.InternetSpeed ?? 0,
                    OnlineState = hardwareIndicatorsRigs.GetValueOrDefault(rig.RigId)?.OnlineState ?? OnlineState.Zero,
                    TotalShares = rig.TotalShares ?? new SharesModel(),
                    TotalHashRate = rig.TotalHashRate,
                    MiningUpTimeInSeconds = rig.MiningUpTimeInSeconds,
                    BootedUpTimeInSeconds = hardwareIndicatorsRigs.GetValueOrDefault(rig.RigId)?.BootedUpTimeInSeconds ?? 0,
                    TotalCoinStatistics = rigTotalCoinStatistics,
                    LocalIp = inventoryRig?.LocalIP ?? "0.0.0.0",
                    MinuxVersion = inventoryRig?.Software?.MinuxVersion ?? "0.0.0",
                    CountDevices = inventoryRig?.CountDevices ?? new ModelWithCountDevices()
                };
            }) ?? Enumerable.Empty<GeneralRigIndicatorsModel>();

            var coinStatsArgs = new CoinStatisticsModelArgs
            {
                CoinNames = coinNames,
                CoinStatistics = monitoringArgs?.TotalCoinStatistics ?? Enumerable.Empty<CoinStatistics>()
            };

            return new MonitoringIndicatorsStreamResponse
            {
                TotalShares = monitoringArgs?.TotalShares ?? new SharesModel(),
                TotalPower = monitoringArgs?.TotalPower ?? 0,
                TotalHashrate = monitoringArgs?.TotalHashrate ?? 0,
                MiningRigsIndicators = rigsIndicators,
                TotalDevices = new ModelWithCountDevices
                {
                    TotalCpusCount = totalCpusCount,
                    TotalGpusCount = totalGpusCount,
                    TotalCpusCountGroupedByManufacturer = cpuManufacturerCounts,
                    TotalGpusCountGroupedByManufacturer = gpuManufacturerCounts
                },
                TotalCoinStatistics = CoinStatisticsModel.ConvertFrom(coinStatsArgs)
            };
        }

        return null;
    }
}