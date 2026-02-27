using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.Overclockings.Fan;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.Overclockings;

public class NvidiaGpuOverclockingModelBuilder :
    BaseOverclockingModelBuilder<NvidiaGpuOverclockingModelBuilder, NvidiaGpuOverclockingModel>
{
    private IFanOverclockingModel? _fanOverclocking = null;
    private int _coreClockLock = 0;
    private int _coreClockOffset = 0;
    private int _coreVoltage = 0;
    private int _coreVoltageOffset = 0;
    private int _memoryClockLock = 0;
    private int _memoryClockOffset = 0;
    private int _memoryVoltage = 0;
    private int _memoryVoltageOffset = 0;
    private int _powerLimit = 0;

    public NvidiaGpuOverclockingModelBuilder WithFanOverclocking(Func<IFanOverclockingModel> factory)
    {
        _fanOverclocking = factory();
        return this;
    }

    public NvidiaGpuOverclockingModelBuilder WithCoreClockLock(int coreClockLock)
    {
        _coreClockLock = coreClockLock;
        return this;
    }

    public NvidiaGpuOverclockingModelBuilder WithCoreClockOffset(int coreClockOffset)
    {
        _coreClockOffset = coreClockOffset;
        return this;
    }

    public NvidiaGpuOverclockingModelBuilder WithCoreVoltage(int coreVoltage)
    {
        _coreVoltage = coreVoltage;
        return this;
    }

    public NvidiaGpuOverclockingModelBuilder WithCoreVoltageOffset(int coreVoltageOffset)
    {
        _coreVoltageOffset = coreVoltageOffset;
        return this;
    }

    public NvidiaGpuOverclockingModelBuilder WithMemoryClockLock(int memoryClockLock)
    {
        _memoryClockLock = memoryClockLock;
        return this;
    }

    public NvidiaGpuOverclockingModelBuilder WithMemoryClockOffset(int memoryClockOffset)
    {
        _memoryClockOffset = memoryClockOffset;
        return this;
    }

    public NvidiaGpuOverclockingModelBuilder WithMemoryVoltage(int memoryVoltage)
    {
        _memoryVoltage = memoryVoltage;
        return this;
    }

    public NvidiaGpuOverclockingModelBuilder WithMemoryVoltageOffset(int memoryVoltageOffset)
    {
        _memoryVoltageOffset = memoryVoltageOffset;
        return this;
    }

    public NvidiaGpuOverclockingModelBuilder WithPowerLimit(int powerLimit)
    {
        _powerLimit = powerLimit;
        return this;
    }

    public override NvidiaGpuOverclockingModel Build()
    {
        return new NvidiaGpuOverclockingModel
        {
            FanOverclocking = _fanOverclocking ??
                new FanOverclockingWithTargetSpeedModelBuilder().Build(),
            CoreClockLock = _coreClockLock,
            CoreClockOffset = _coreClockOffset,
            CoreVoltage = _coreVoltage,
            CoreVoltageOffset = _coreVoltageOffset,
            MemoryClockLock = _memoryClockLock,
            MemoryClockOffset = _memoryClockOffset,
            MemoryVoltage = _memoryVoltage,
            MemoryVoltageOffset = _memoryVoltageOffset,
            PowerLimit = _powerLimit,
        };
    }
}
