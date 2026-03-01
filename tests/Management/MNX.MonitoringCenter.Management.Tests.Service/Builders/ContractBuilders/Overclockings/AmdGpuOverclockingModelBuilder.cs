using MNX.MonitoringCenter.Management.Contracts.Overclocking;
using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.Overclockings.Fan;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.Overclockings;

public class AmdGpuOverclockingModelBuilder :
    BaseOverclockingModelBuilder<AmdGpuOverclockingModelBuilder, AmdGpuOverclockingModel>
{
    private IFanOverclockingModel? _fanOverclocking = null;
    private int _powerLimit = 0;
    private int _coreClockLock = 0;
    private int _coreClockState = 0;
    private int _coreVoltage = 0;
    private int _coreVoltageOffset = 0;
    private int _memoryClockLock = 0;
    private int _memoryClockState = 0;
    private int _memoryVoltage = 0;
    private int _memoryControllerVoltage = 0;
    private string? _memoryTweak = null;
    private bool _enhancedOverclock = false;
    private bool _alternativeDownVoltage = false;
    private int _socFrequency = 0;
    private int _socVoltage = 0;

    public AmdGpuOverclockingModelBuilder WithFanOverclocking(Func<IFanOverclockingModel> factory)
    {
        _fanOverclocking = factory();
        return this;
    }

    public AmdGpuOverclockingModelBuilder WithPowerLimit(int powerLimit)
    {
        _powerLimit = powerLimit;
        return this;
    }

    public AmdGpuOverclockingModelBuilder WithCoreClockLock(int coreClockLock)
    {
        _coreClockLock = coreClockLock;
        return this;
    }

    public AmdGpuOverclockingModelBuilder WithCoreClockState(int coreClockState)
    {
        _coreClockState = coreClockState;
        return this;
    }

    public AmdGpuOverclockingModelBuilder WithCoreVoltage(int coreVoltage)
    {
        _coreVoltage = coreVoltage;
        return this;
    }

    public AmdGpuOverclockingModelBuilder WithCoreVoltageOffset(int coreVoltageOffset)
    {
        _coreVoltageOffset = coreVoltageOffset;
        return this;
    }

    public AmdGpuOverclockingModelBuilder WithMemoryClockLock(int memoryClockLock)
    {
        _memoryClockLock = memoryClockLock;
        return this;
    }

    public AmdGpuOverclockingModelBuilder WithMemoryClockState(int memoryClockState)
    {
        _memoryClockState = memoryClockState;
        return this;
    }

    public AmdGpuOverclockingModelBuilder WithMemoryVoltage(int memoryVoltage)
    {
        _memoryVoltage = memoryVoltage;
        return this;
    }

    public AmdGpuOverclockingModelBuilder WithMemoryControllerVoltage(int memoryControllerVoltage)
    {
        _memoryControllerVoltage = memoryControllerVoltage;
        return this;
    }

    public AmdGpuOverclockingModelBuilder WithMemoryTweak(string? memoryTweak)
    {
        _memoryTweak = memoryTweak;
        return this;
    }

    public AmdGpuOverclockingModelBuilder WithEnhancedOverclock(bool enhancedOverclock = true)
    {
        _enhancedOverclock = enhancedOverclock;
        return this;
    }

    public AmdGpuOverclockingModelBuilder WithAlternativeDownVoltage(bool alternativeDownVoltage)
    {
        _alternativeDownVoltage = alternativeDownVoltage;
        return this;
    }

    public AmdGpuOverclockingModelBuilder WithSocFrequency(int socFrequency)
    {
        _socFrequency = socFrequency;
        return this;
    }

    public AmdGpuOverclockingModelBuilder WithSocVoltage(int socVoltage)
    {
        _socVoltage = socVoltage;
        return this;
    }

    public override AmdGpuOverclockingModel Build()
    {
        return new AmdGpuOverclockingModel
        {
            FanOverclocking = _fanOverclocking ??
                new FanOverclockingWithTargetSpeedModelBuilder().Build(),
            PowerLimit = _powerLimit,
            CoreClockLock = _coreClockLock,
            CoreClockState = _coreClockState,
            CoreVoltage = _coreVoltage,
            CoreVoltageOffset = _coreVoltageOffset,
            MemoryClockLock = _memoryClockLock,
            MemoryClockState = _memoryClockState,
            MemoryVoltage = _memoryVoltage,
            MemoryControllerVoltage = _memoryControllerVoltage,
            MemoryTweak = _memoryTweak,
            EnhancedOverclock = _enhancedOverclock,
            AlternativeDownVoltage = _alternativeDownVoltage,
            SocFrequency = _socFrequency,
            SocVoltage = _socVoltage,
        };
    }
}
