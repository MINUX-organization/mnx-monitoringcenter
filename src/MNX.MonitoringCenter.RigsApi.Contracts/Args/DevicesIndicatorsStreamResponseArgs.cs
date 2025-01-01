using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts.Devices;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts.Devices;

namespace MNX.MonitoringCenter.RigsApi.Contracts.Args;

public class DevicesIndicatorsStreamResponseArgs
{
    public IEnumerable<CpuDynamicMiningIndicators> CpuDynamicMiningIndicators { get; set; }

    public IEnumerable<CpuDynamicHardwareIndicators> CpuDynamicHardwareIndicators { get; set; }

    public IEnumerable<GpuDynamicMiningIndicators> GpuDynamicMiningIndicators { get; set; }

    public IEnumerable<GpuDynamicHardwareIndicators> GpuDynamicHardwareIndicators { get; set; }

    public Dictionary<Guid, string> CpusNames { get; set; }

    public Dictionary<Guid, string> GpusNames { get; set; }

    public Dictionary<(Guid FlightSheetId, Guid MinerId, Guid CoinId), MiningCombinations> MiningCombinations { get; set; }

    public DevicesIndicatorsStreamResponseArgs(
        IEnumerable<CpuDynamicMiningIndicators> cpuDynamicMiningIndicators,
        IEnumerable<CpuDynamicHardwareIndicators> cpuDynamicHardwareIndicators,
        IEnumerable<GpuDynamicMiningIndicators> gpuDynamicMiningIndicators,
        IEnumerable<GpuDynamicHardwareIndicators> gpuDynamicHardwareIndicators,
        Dictionary<Guid, string> cpusNames,
        Dictionary<Guid, string> gpusNames,
        Dictionary<(Guid FlightSheetId, Guid MinerId, Guid CoinId), MiningCombinations> miningCombinations)
    {
        CpuDynamicMiningIndicators = cpuDynamicMiningIndicators;
        CpuDynamicHardwareIndicators = cpuDynamicHardwareIndicators;
        GpuDynamicMiningIndicators = gpuDynamicMiningIndicators;
        GpuDynamicHardwareIndicators = gpuDynamicHardwareIndicators;
        CpusNames = cpusNames;
        GpusNames = gpusNames;
        MiningCombinations = miningCombinations;
    }
}