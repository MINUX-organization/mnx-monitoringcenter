using MNX.MonitoringCenter.RigsApi.Contracts.Abstractions;
using MNX.MonitoringCenter.RigsApi.Contracts.Args;
using MNX.MonitoringCenter.RigsApi.Contracts.Device;
using MNX.MonitoringCenter.RigsApi.Contracts.FlightSheet;

namespace MNX.MonitoringCenter.RigsApi.Contracts.Streams;

public class DevicesIndicatorsStreamResponse : IConverterFrom<DevicesIndicatorsStreamResponse>
{
    public IEnumerable<CpuDynamicMiningIndicatorsModel>? CpuDynamicMiningIndicators { get; set; }

    public IEnumerable<CpuDynamicHardwareIndicatorsModel>? CpuDynamicHardwareIndicators { get; set; }

    public IEnumerable<GpuDynamicMiningIndicatorsModel>? GpuDynamicMiningIndicators { get; set; }

    public IEnumerable<GpuDynamicHardwareIndicatorsModel>? GpuDynamicHardwareIndicators { get; set; }

    public static DevicesIndicatorsStreamResponse? ConvertFrom<TSource>(TSource source)
    {
        // TODO: работа с (Guid, string) кортежами имен девайсов.
        if (source is DevicesIndicatorsStreamResponseArgs devicesIndicatorsResponseArgs)
        {
            IEnumerable<CpuDynamicMiningIndicatorsModel> cpuDynamicMiningIndicators = devicesIndicatorsResponseArgs
                .CpuDynamicMiningIndicators
                .Select(cpu => new CpuDynamicMiningIndicatorsModel
                {
                    DeviceId = cpu.DeviceId,
                    DeviceName = devicesIndicatorsResponseArgs.CpusNames.GetValueOrDefault(cpu.DeviceId),
                    MiningState = cpu.MiningState,
                    FlightSheet = FlightSheetStatisticsModel.ConvertFrom(new FlightSheetStatisticsModelArgs
                    { 
                        FlightSheetStatistics = cpu.FlightSheet, 
                        MiningCombinations = devicesIndicatorsResponseArgs.MiningCombinations 
                    }),
                });

            IEnumerable<CpuDynamicHardwareIndicatorsModel> cpuDynamicHardwareIndicators = devicesIndicatorsResponseArgs
                .CpuDynamicHardwareIndicators
                .Select(cpu => new CpuDynamicHardwareIndicatorsModel
                {
                    DeviceId = cpu.DeviceId,
                    DeviceName = devicesIndicatorsResponseArgs.CpusNames.GetValueOrDefault(cpu.DeviceId),
                    Power = cpu.Power,
                    FanSpeed = cpu.FanSpeed,
                    Temperature = cpu.Temperature,
                });

            IEnumerable<GpuDynamicMiningIndicatorsModel> gpuDynamicMiningIndicators = devicesIndicatorsResponseArgs
                .GpuDynamicMiningIndicators
                .Select(gpu => new GpuDynamicMiningIndicatorsModel
                {
                    DeviceId = gpu.DeviceId,
                    DeviceName = devicesIndicatorsResponseArgs.GpusNames.GetValueOrDefault(gpu.DeviceId),
                    MiningState = gpu.MiningState,
                    FlightSheet = FlightSheetStatisticsModel.ConvertFrom(new FlightSheetStatisticsModelArgs
                    {
                        FlightSheetStatistics = gpu.FlightSheet,
                        MiningCombinations = devicesIndicatorsResponseArgs.MiningCombinations
                    }),
                });
                
            IEnumerable<GpuDynamicHardwareIndicatorsModel> gpuDynamicHardwareIndicators = devicesIndicatorsResponseArgs
                .GpuDynamicHardwareIndicators
                .Select(gpu => new GpuDynamicHardwareIndicatorsModel
                {
                    DeviceId = gpu.DeviceId,
                    DeviceName = devicesIndicatorsResponseArgs.GpusNames.GetValueOrDefault(gpu.DeviceId),
                    Power = gpu.Power,
                    AvarageTemperature = gpu.GetAverageTemperature(),
                    FanSpeed = gpu.FanSpeed,
                });
        }

        return null;
    }
}
