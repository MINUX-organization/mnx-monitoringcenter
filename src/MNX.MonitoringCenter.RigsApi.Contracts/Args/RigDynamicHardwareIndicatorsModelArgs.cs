using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts;

namespace MNX.MonitoringCenter.RigsApi.Contracts.Args;

public class RigDynamicHardwareIndicatorsModelArgs
{
    public IEnumerable<RigDynamicHardwareIndicators>? RigDynamicHardwareIndicators { get; set; }

    public Dictionary<Guid, Rig> Rigs { get; set; }

    public RigDynamicHardwareIndicatorsModelArgs(
        IEnumerable<RigDynamicHardwareIndicators>? rigDynamicHardwareIndicators,
        Dictionary<Guid, Rig> rigs)
    {
        RigDynamicHardwareIndicators = rigDynamicHardwareIndicators;
        Rigs = rigs;
    }
}
