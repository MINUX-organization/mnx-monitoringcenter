using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.RigsApi.Contracts.Abstractions;
using MNX.MonitoringCenter.RigsApi.Contracts.Args;
using MNX.MonitoringCenter.RigsApi.Contracts.Device;
using MNX.MonitoringCenter.RigsApi.Contracts.FlightSheet;
using MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts.Devices;

namespace MNX.MonitoringCenter.RigsApi.Contracts.Streams;

public class DevicesIndicatorsStreamResponse : IConverterFrom<DevicesIndicatorsStreamResponse>
{
    public IEnumerable<CpuDynamicTotalIndicators>? CpuDynamicTotalIndicators { get; set; }

    public IEnumerable<GpuDynamicTotalIndicatorsModel>? GpuDynamicTotalIndicators { get; set; }

    public static DevicesIndicatorsStreamResponse? ConvertFrom<TSource>(TSource source)
    {
        if (source is DevicesIndicatorsStreamResponseArgs devicesIndicatorsResponseArgs)
        {
            var cpuDynamicHardwareIndicatorsDictionary = devicesIndicatorsResponseArgs.CpuDynamicHardwareIndicators?
                .ToDictionary(c => c.DeviceId) ?? new Dictionary<Guid, CpuDynamicHardwareIndicators>();

            var gpuDynamicHardwareIndicatorsDictionary = devicesIndicatorsResponseArgs.GpuDynamicHardwareIndicators?
                .ToDictionary(g => g.DeviceId) ?? new Dictionary<Guid, GpuDynamicHardwareIndicators>();

            var cpuDynamicTotalIndicators = devicesIndicatorsResponseArgs.CpuDynamicMiningIndicators?
                .Select(cpu =>
                {
                    var hardware = cpuDynamicHardwareIndicatorsDictionary.GetValueOrDefault(cpu.DeviceId);
                    return new CpuDynamicTotalIndicators
                    {
                        DeviceId = cpu.DeviceId,
                        DeviceName = devicesIndicatorsResponseArgs.CpusNames.GetValueOrDefault(cpu.DeviceId) 
                            ?? "Unknown CPU",
                        MiningState = cpu.MiningState,
                        FanSpeed = hardware?.FanSpeed ?? 0,
                        Temperature = hardware?.Temperature ?? 0,
                        Power = hardware?.Power ?? 0,
                        FlightSheet = FlightSheetStatisticsModel.ConvertFrom(new FlightSheetStatisticsModelArgs
                        {
                            FlightSheetStatistics = cpu.FlightSheet,
                            MiningCombinations = devicesIndicatorsResponseArgs.MiningCombinations 
                                ?? new Dictionary<(Guid, Guid, Guid), MiningCombination>()
                        }) ?? new FlightSheetStatisticsModel()
                    };
                }) ?? Enumerable.Empty<CpuDynamicTotalIndicators>();

            var gpuDynamicTotalIndicators = devicesIndicatorsResponseArgs.GpuDynamicMiningIndicators?
                .Select(gpu =>
                {
                    var hardware = gpuDynamicHardwareIndicatorsDictionary.GetValueOrDefault(gpu.DeviceId);
                    return new GpuDynamicTotalIndicatorsModel
                    {
                        DeviceId = gpu.DeviceId,
                        DeviceName = devicesIndicatorsResponseArgs.GpusNames.GetValueOrDefault(gpu.DeviceId) 
                            ?? "Unknown GPU",
                        FanSpeed = hardware?.FanSpeed ?? 0,
                        CoreTemperature = hardware?.CoreTemperature ?? 0,
                        MemoryTemperature = hardware?.MemoryTemperature ?? 0,
                        AvarageTemperature = hardware?.GetAverageTemperature() ?? 0,
                        Power = hardware?.Power ?? 0,
                        MiningState = gpu.MiningState,
                        FlightSheet = FlightSheetStatisticsModel.ConvertFrom(new FlightSheetStatisticsModelArgs
                        {
                            FlightSheetStatistics = gpu.FlightSheet,
                            MiningCombinations = devicesIndicatorsResponseArgs.MiningCombinations 
                                ?? new Dictionary<(Guid, Guid, Guid), MiningCombination>()
                        }) ?? new FlightSheetStatisticsModel()
                    };
                }) ?? Enumerable.Empty<GpuDynamicTotalIndicatorsModel>();

            return new DevicesIndicatorsStreamResponse
            {
                CpuDynamicTotalIndicators = cpuDynamicTotalIndicators,
                GpuDynamicTotalIndicators = gpuDynamicTotalIndicators
            };
        }

        return null;
    }
}
