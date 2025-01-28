using MNX.MonitoringCenter.RigsApi.Contracts.Abstractions;
using MNX.MonitoringCenter.RigsApi.Contracts.Args;
using MNX.MonitoringCenter.RigsApi.Contracts.Device;
using MNX.MonitoringCenter.RigsApi.Contracts.FlightSheet;

namespace MNX.MonitoringCenter.RigsApi.Contracts.Streams;

public class DevicesIndicatorsStreamResponse : IConverterFrom<DevicesIndicatorsStreamResponse>
{
    public IEnumerable<CpuDynamicTotalIndicators>? CpuDynamicTotalIndicators { get; set; }

    public IEnumerable<GpuDynamicTotalIndicatorsModel>? GpuDynamicTotalIndicators { get; set; }

    public static DevicesIndicatorsStreamResponse? ConvertFrom<TSource>(TSource source)
    {
        if (source is DevicesIndicatorsStreamResponseArgs devicesIndicatorsResponseArgs)
        {
            var cpuDynamicHardwareIndicatorsDictionary = devicesIndicatorsResponseArgs
                        .CpuDynamicHardwareIndicators
                        .ToDictionary(c => c.DeviceId);

            var gpuDynamicHardwareIndicatorsDictionary = devicesIndicatorsResponseArgs
                        .GpuDynamicHardwareIndicators
                        .ToDictionary(g => g.DeviceId);

            IEnumerable<CpuDynamicTotalIndicators> cpuDynamicTotalIndicators = devicesIndicatorsResponseArgs
                .CpuDynamicMiningIndicators
                .Select(cpu => {
                    var cpuDynamicHardwareIndicatorsItem = cpuDynamicHardwareIndicatorsDictionary
                        .GetValueOrDefault(cpu.DeviceId);

                    return new CpuDynamicTotalIndicators
                    {
                        DeviceId = cpu.DeviceId,
                        DeviceName = devicesIndicatorsResponseArgs.CpusNames.GetValueOrDefault(cpu.DeviceId),
                        MiningState = cpu.MiningState,
                        FanSpeed = cpuDynamicHardwareIndicatorsItem?.FanSpeed ?? 0,
                        Temperature = cpuDynamicHardwareIndicatorsItem?.Temperature ?? 0,
                        Power = cpuDynamicHardwareIndicatorsItem?.Power ?? 0,
                        FlightSheet = FlightSheetStatisticsModel.ConvertFrom(new FlightSheetStatisticsModelArgs
                        { 
                            FlightSheetStatistics = cpu.FlightSheet, 
                            MiningCombinations = devicesIndicatorsResponseArgs.MiningCombinations 
                        }),
                    };
                });

            IEnumerable<GpuDynamicTotalIndicatorsModel> gpuDynamicMiningIndicators = devicesIndicatorsResponseArgs
                .GpuDynamicMiningIndicators
                .Select(gpu => {
                    var gpuDynamicHardwareIndicatorsItem = gpuDynamicHardwareIndicatorsDictionary
                        .GetValueOrDefault(gpu.DeviceId);

                    return new GpuDynamicTotalIndicatorsModel
                    {
                        DeviceId = gpu.DeviceId,
                        DeviceName = devicesIndicatorsResponseArgs.GpusNames.GetValueOrDefault(gpu.DeviceId),
                        FanSpeed = gpuDynamicHardwareIndicatorsItem?.FanSpeed ?? 0,
                        CoreTemperature = gpuDynamicHardwareIndicatorsItem?.CoreTemperature ?? 0,
                        MemoryTemperature = gpuDynamicHardwareIndicatorsItem?.MemoryTemperature ?? 0,
                        AvarageTemperature = gpuDynamicHardwareIndicatorsItem?.GetAverageTemperature() ?? 0,
                        Power = gpuDynamicHardwareIndicatorsItem?.Power ?? 0,
                        MiningState = gpu.MiningState,
                        FlightSheet = FlightSheetStatisticsModel.ConvertFrom(new FlightSheetStatisticsModelArgs
                        {
                            FlightSheetStatistics = gpu.FlightSheet,
                            MiningCombinations = devicesIndicatorsResponseArgs.MiningCombinations
                        }),
                    };
                });
        }

        return null;
    }
}
