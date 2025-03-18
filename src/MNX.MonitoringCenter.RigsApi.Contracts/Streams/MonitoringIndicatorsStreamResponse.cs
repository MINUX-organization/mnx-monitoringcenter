using MNX.MonitoringCenter.Inventory.Contracts.Devices.CountDevices;
using MNX.MonitoringCenter.RigsApi.Contracts.Abstractions;
using MNX.MonitoringCenter.RigsApi.Contracts.Args;
using MNX.MonitoringCenter.RigsApi.Contracts.FlightSheet;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining.FlightSheet;

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
        if (source is MonitoringIndicatorsStreamResponseArgs monitoringIndicatorsResponseArgs)
        {
            var hardwareIndicatorsRigs = monitoringIndicatorsResponseArgs.RigDynamicHardwareIndicators.ToDictionary(rig => rig.RigId);

            int totalCpusCount = 0;
            int totalGpusCount = 0;
            Dictionary<string, int> totalCpusCountGroupedByManufacturer = new();
            Dictionary<string, int> totalGpusCountGroupedByManufacturer = new();

            IEnumerable<GeneralRigIndicatorsModel> rigsDynamicMiningIndicators = monitoringIndicatorsResponseArgs
                .RigsDynamicMiningIndicators
                .Select(rig =>
                {
                    var inventoryRig = monitoringIndicatorsResponseArgs.Rigs.GetValueOrDefault(rig.RigId);

                    totalCpusCount += inventoryRig?.CountDevices.TotalCpusCount ?? 0;
                    totalGpusCount += inventoryRig?.CountDevices.TotalGpusCount ?? 0;

                    foreach (var (manufacturer, count) in inventoryRig?.CountDevices.TotalCpusCountGroupedByManufacturer 
                        ?? Enumerable.Empty<KeyValuePair<string, int>>())
                    {
                        if (!totalCpusCountGroupedByManufacturer.TryAdd(manufacturer, count))
                        {
                            totalCpusCountGroupedByManufacturer[manufacturer] = 
                                totalCpusCountGroupedByManufacturer.GetValueOrDefault(manufacturer) + count;
                        }
                    }

                    foreach (var (manufacturer, count) in inventoryRig?.CountDevices.TotalGpusCountGroupedByManufacturer 
                        ?? Enumerable.Empty<KeyValuePair<string, int>>())
                    {
                        if (!totalGpusCountGroupedByManufacturer.TryAdd(manufacturer, count))
                        {
                            totalGpusCountGroupedByManufacturer[manufacturer] = 
                                totalGpusCountGroupedByManufacturer.GetValueOrDefault(manufacturer) + count;
                        }
                    }
                    
                    var rigCoinStatisticsModelArgs = new RigCoinStatisticsModelArgs
                    {
                        RigCoinStatistics = rig.TotalCoinStatistics,
                        MiningCombinations = monitoringIndicatorsResponseArgs.MiningCombinations
                    };

                    return new GeneralRigIndicatorsModel
                    {
                        RigId = rig.RigId,
                        RigName = inventoryRig?.Name,
                        AverageMiningDevicesFanSpeed = hardwareIndicatorsRigs.GetValueOrDefault(rig.RigId)?.AverageMiningDevicesFanSpeed,
                        AverageMiningDevicesTemperature = hardwareIndicatorsRigs.GetValueOrDefault(rig.RigId)?.AverageMiningDevicesTemperature,
                        TotalPower = hardwareIndicatorsRigs.GetValueOrDefault(rig.RigId)?.TotalPower,
                        InternetSpeed = hardwareIndicatorsRigs.GetValueOrDefault(rig.RigId)?.InternetSpeed,
                        OnlineState = hardwareIndicatorsRigs.GetValueOrDefault(rig.RigId)?.OnlineState,
                        TotalShares = rig.TotalShares,
                        TotalHashRate = rig.TotalHashRate,
                        MiningUpTimeInSeconds = rig.MiningUpTimeInSeconds,
                        BootedUpTimeInSeconds = hardwareIndicatorsRigs.GetValueOrDefault(rig.RigId)?.BootedUpTimeInSeconds,
                        TotalCoinStatistics = RigCoinStatisticModel.ConvertFrom(rigCoinStatisticsModelArgs)
                                                                    ?? new List<RigCoinStatisticModel>(),
                        LocalIp = inventoryRig?.LocalIP,
                        MinuxVersion = inventoryRig?.Software.MinuxVersion,
                        CountDevices = inventoryRig?.CountDevices,
                    };
                });

            return new MonitoringIndicatorsStreamResponse
            {
                TotalShares = monitoringIndicatorsResponseArgs.TotalShares,
                TotalPower = monitoringIndicatorsResponseArgs.TotalPower,
                TotalHashrate = monitoringIndicatorsResponseArgs.TotalHashrate,
                MiningRigsIndicators = rigsDynamicMiningIndicators,
                TotalDevices = new ModelWithCountDevices
                {
                    TotalCpusCount = totalCpusCount,
                    TotalGpusCount = totalGpusCount,
                    TotalCpusCountGroupedByManufacturer = totalCpusCountGroupedByManufacturer,
                    TotalGpusCountGroupedByManufacturer = totalGpusCountGroupedByManufacturer,
                }
            };
        }

        return null;
    }
}