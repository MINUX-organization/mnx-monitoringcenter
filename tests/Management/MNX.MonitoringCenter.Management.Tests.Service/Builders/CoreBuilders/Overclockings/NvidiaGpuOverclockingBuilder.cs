using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.Overclockings.Fans;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.Overclockings;

public class NvidiaGpuOverclockingBuilder :
    BaseOverclockingBuilder<NvidiaGpuOverclockingBuilder, NvidiaGpuOverclocking>
{
    protected IFanOverclocking? _fanOverclocking = null;
    protected int _coreClockLock = 0;
    protected int _coreClockOffset = 0;
    protected int _coreVoltage = 0;
    protected int _coreVoltageOffset = 0;
    protected int _memoryClockLock = 0;
    protected int _memoryClockOffset = 0;
    protected int _memoryVoltage = 0;
    protected int _memoryVoltageOffset = 0;
    protected int _powerLimit = 0;

    public NvidiaGpuOverclockingBuilder WithFanOverclocking(Func<IFanOverclocking> factory)
    {
        _fanOverclocking = factory();
        return this;
    }

    public NvidiaGpuOverclockingBuilder WithCoreClockLock(int coreClockLock)
    {
        _coreClockLock = coreClockLock;
        return this;
    }

    public NvidiaGpuOverclockingBuilder WithCoreClockOffset(int coreClockOffset)
    {
        _coreClockOffset = coreClockOffset;
        return this;
    }

    public NvidiaGpuOverclockingBuilder WithCoreVoltage(int coreVoltage)
    {
        _coreVoltage = coreVoltage;
        return this;
    }
    
    public NvidiaGpuOverclockingBuilder WithCoreVoltageOffset(int coreVoltageOffset)
    {
        _coreVoltageOffset = coreVoltageOffset;
        return this;
    }
    
    public NvidiaGpuOverclockingBuilder WithMemoryClockLock(int memoryClockLock)
    {
        _memoryClockLock = memoryClockLock;
        return this;
    }
    
    public NvidiaGpuOverclockingBuilder WithMemoryClockOffset(int memoryClockOffset)
    {
        _memoryClockOffset = memoryClockOffset;
        return this;
    }
    
    public NvidiaGpuOverclockingBuilder WithMemoryVoltage(int memoryVoltage)
    {
        _memoryVoltage = memoryVoltage;
        return this;
    }
    
    public NvidiaGpuOverclockingBuilder WithMemoryVoltageOffset(int memoryVoltageOffset)
    {
        _memoryVoltageOffset = memoryVoltageOffset;
        return this;
    }
    
    public NvidiaGpuOverclockingBuilder WithPowerLimit(int powerLimit)
    {
        _powerLimit = powerLimit;
        return this;
    }
    
    public override IOverclocking Build()
    {
        return new NvidiaGpuOverclocking
        {
            Id = _id,
            FanOverclocking = _fanOverclocking ??
                new FanOverclockingWithTargetSpeedBuilder().Build(),
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
