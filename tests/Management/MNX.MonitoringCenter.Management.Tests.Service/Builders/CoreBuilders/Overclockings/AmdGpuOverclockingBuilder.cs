using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.Overclockings.Fans;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.Overclockings;

public class AmdGpuOverclockingBuilder :
    BaseOverclockingBuilder<AmdGpuOverclockingBuilder, AmdGpuOverclocking>
{
    protected IFanOverclocking? _fanOverclocking = null;
    protected bool _alternativeDownVoltage = false;
    protected int _coreClockLock = 0;
    protected int _coreClockState = 0;
    protected int _coreVoltage = 0;
    protected int _coreVoltageOffset = 0;
    protected bool _enhancedOverclock = false;
    protected int _memoryClockLock = 0;
    protected int _memoryClockState = 0;
    protected int _memoryControllerVoltage = 0;
    protected string? _memoryTweak = null;
    protected int _memoryVoltage = 0;
    protected int _powerLimit = 0;
    protected int _socFrequency = 0;
    protected int _socVoltage = 0;

    public AmdGpuOverclockingBuilder WithFanOverclocking(Func<IFanOverclocking> factory)
    {
        _fanOverclocking = factory();
        return this;
    }

    public AmdGpuOverclockingBuilder WithAlternativeDownVoltage(bool alternativeDownVoltage)
    {
        _alternativeDownVoltage = alternativeDownVoltage;
        return this;
    }

    public AmdGpuOverclockingBuilder WithCoreClockLock(int coreClockLock)
    {
        _coreClockLock = coreClockLock;
        return this;
    }

    public AmdGpuOverclockingBuilder WithCoreClockState(int coreClockState)
    {
        _coreClockState = coreClockState;
        return this;
    }

    public AmdGpuOverclockingBuilder WithCoreVoltage(int coreVoltage)
    {
        _coreVoltage = coreVoltage;
        return this;
    }

    public AmdGpuOverclockingBuilder WithCoreVoltageOffset(int coreVoltageOffset)
    {
        _coreVoltageOffset = coreVoltageOffset;
        return this;
    }

    public AmdGpuOverclockingBuilder WithEnhancedOverclock(bool enhancedOverclock = true)
    {
        _enhancedOverclock = enhancedOverclock;
        return this;
    }

    public AmdGpuOverclockingBuilder WithMemoryClockLock(int memoryClockLock)
    {
        _memoryClockLock = memoryClockLock;
        return this;
    }

    public AmdGpuOverclockingBuilder WithMemoryClockState(int memoryClockState)
    {
        _memoryClockState = memoryClockState;
        return this;
    }

    public AmdGpuOverclockingBuilder WithMemoryControllerVoltage(int memoryControllerVoltage)
    {
        _memoryControllerVoltage = memoryControllerVoltage;
        return this;
    }

    public AmdGpuOverclockingBuilder WithMemoryTweak(string? memoryTweak)
    {
        _memoryTweak = memoryTweak;
        return this;
    }

    public AmdGpuOverclockingBuilder WithMemoryVoltage(int memoryVoltage)
    {
        _memoryVoltage = memoryVoltage;
        return this;
    }

    public AmdGpuOverclockingBuilder WithPowerLimit(int powerLimit)
    {
        _powerLimit = powerLimit;
        return this;
    }

    public AmdGpuOverclockingBuilder WithSocFrequency(int socFrequency)
    {
        _socFrequency = socFrequency;
        return this;
    }

    public AmdGpuOverclockingBuilder WithSocVoltage(int socVoltage)
    {
        _socVoltage = socVoltage;
        return this;
    }

    public override AmdGpuOverclocking Build()
    {
        return new AmdGpuOverclocking
        {
            Id = _id,
            FanOverclocking = _fanOverclocking
                ?? new FanOverclockingWithTargetSpeedBuilder().Build(),
            AlternativeDownVoltage = _alternativeDownVoltage,
            CoreClockLock = _coreClockLock,
            CoreClockState = _coreClockState,
            CoreVoltage = _coreVoltage,
            CoreVoltageOffset = _coreVoltageOffset,
            EnhancedOverclock = _enhancedOverclock,
            MemoryClockLock = _memoryClockLock,
            MemoryClockState = _memoryClockState,
            MemoryControllerVoltage = _memoryControllerVoltage,
            MemoryTweak = _memoryTweak,
            MemoryVoltage = _memoryVoltage,
            PowerLimit = _powerLimit,
            SocFrequency = _socFrequency,
            SocVoltage = _socVoltage,
        };
    }
}
