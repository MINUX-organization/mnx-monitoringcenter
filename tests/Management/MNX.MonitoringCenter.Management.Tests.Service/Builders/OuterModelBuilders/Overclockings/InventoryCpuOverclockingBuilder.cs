using MNX.MonitoringCenter.Inventory.Contracts.Devices.Cpu;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.OuterModelBuilders.Overclockings;

public class InventoryCpuOverclockingBuilder :
    InventoryOverclockingBuilder<InventoryCpuOverclockingBuilder, CpuOverclocking>
{
    protected int? _coreClockLock = null;
    protected int? _coreVoltage = null;

    public InventoryCpuOverclockingBuilder WithCoreClockLock(int coreClockLock = 0)
    {
        _coreClockLock = coreClockLock;
        return this;
    }

    public InventoryCpuOverclockingBuilder WithCoreVoltage(int coreVoltage = 0)
    {
        _coreVoltage = coreVoltage;
        return this;
    }

    public override CpuOverclocking Build()
    {
        return new CpuOverclocking
        {
            CoreClockLock = _coreClockLock,
            CoreVoltage = _coreVoltage,
        };
    }
}
