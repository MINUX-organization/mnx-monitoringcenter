using MNX.MonitoringCenter.Management.Contracts.Overclocking;
using MNX.MonitoringCenter.Management.Contracts.Overclocking.Cpu;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.Overclockings;

public class CpuOverclockingModelBuilder :
    BaseOverclockingModelBuilder<CpuOverclockingModelBuilder, CpuOverclockingModel>
{
    protected int _coreClockLock = 0;
    protected int _coreVoltage = 0;

    public CpuOverclockingModelBuilder WithCoreClockLock(int coreClockLock)
    {
        _coreClockLock = coreClockLock;
        return this;
    }

    public CpuOverclockingModelBuilder WithCoreVoltage(int coreVoltage)
    {
        _coreVoltage = coreVoltage;
        return this;
    }

    public override CpuOverclockingModel Build()
    {
        return new CpuOverclockingModel
        {
            CoreClockLock = _coreClockLock,
            CoreVoltage = _coreVoltage,
        };
    }
}
