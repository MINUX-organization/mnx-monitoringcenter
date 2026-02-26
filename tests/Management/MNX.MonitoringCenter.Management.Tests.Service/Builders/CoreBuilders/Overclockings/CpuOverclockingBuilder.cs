using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking.Cpu;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.Overclockings;

public class CpuOverclockingBuilder :
    BaseOverclockingBuilder<CpuOverclockingBuilder, CpuOverclocking>
{
    protected int _coreVoltage = 0;
    protected int _coreClockLock = 0;

    public CpuOverclockingBuilder WithCoreVoltage(int coreVoltage)
    {
        _coreVoltage = coreVoltage;
        return this;
    }

    public CpuOverclockingBuilder WithCoreClockLock(int coreClockLock)
    {
        _coreClockLock = coreClockLock;
        return this;
    }

    public override IOverclocking Build()
    {
        return new CpuOverclocking()
        {
            Id = _id,
            CoreVoltage = _coreVoltage,
            CoreClockLock = _coreClockLock,
        };
    }
}
