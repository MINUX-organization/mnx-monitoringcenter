using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Overclocking;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.OuterModelBuilders.Overclockings;

public class InventoryAmdGpuOverclockingBuilder :
    InventoryGpuOverclockingBuilder<InventoryAmdGpuOverclockingBuilder, AmdGpuOverclocking>
{
    protected int? _coreClockLock = null;
    protected int? _coreClockState = null;
    protected int? _coreVoltage = null;
    protected int? _coreVoltageOffset = null;
    protected int? _memoryClockLock = null;
    protected int? _memoryClockState = null;
    protected int? _memoryVoltage = null;
    protected int? _memoryControllerVoltage = null;
    protected string? _memoryTweak = null;
    protected int? _socFrequency = null;
    protected int? _socVoltage = null;
    protected bool? _alternativeDownVoltage = null;
    protected bool? _enhancedOverclock = null;

    public InventoryAmdGpuOverclockingBuilder WithCoreClockLock(int coreClockLock = default)
    {
        _coreClockLock = coreClockLock;
        return this;
    }

    public InventoryAmdGpuOverclockingBuilder WithCoreClockState(int coreClockState = default)
    {
        _coreClockState = coreClockState;
        return this;
    }

    public InventoryAmdGpuOverclockingBuilder WithCoreVoltage(int coreVoltage = default)
    {
        _coreVoltage = coreVoltage;
        return this;
    }

    public InventoryAmdGpuOverclockingBuilder WithCoreVoltageOffset(int coreVoltageOffset = default)
    {
        _coreVoltageOffset = coreVoltageOffset;
        return this;
    }

    public InventoryAmdGpuOverclockingBuilder WithMemoryClockLock(int memoryClockLock = default)
    {
        _memoryClockLock = memoryClockLock;
        return this;
    }

    public InventoryAmdGpuOverclockingBuilder WithMemoryClockState(int memoryClockState = default)
    {
        _memoryClockState = memoryClockState;
        return this;
    }

    public InventoryAmdGpuOverclockingBuilder WithMemoryVoltage(int memoryVoltage = default)
    {
        _memoryVoltage = memoryVoltage;
        return this;
    }

    public InventoryAmdGpuOverclockingBuilder WithMemoryControllerVoltage(int memoryControllerVoltage = default)
    {
        _memoryControllerVoltage = memoryControllerVoltage;
        return this;
    }

    public InventoryAmdGpuOverclockingBuilder WithMemoryTweak(string? memoryTweak = default)
    {
        _memoryTweak = memoryTweak;
        return this;
    }

    public InventoryAmdGpuOverclockingBuilder WithSocFrequency(int socFrequency = default)
    {
        _socFrequency = socFrequency;
        return this;
    }

    public InventoryAmdGpuOverclockingBuilder WithSocVoltage(int socVoltage = default)
    {
        _socVoltage = socVoltage;
        return this;
    }

    public InventoryAmdGpuOverclockingBuilder WithAlternativeDownVoltage(bool alternativeDownVoltage = default)
    {
        _alternativeDownVoltage = alternativeDownVoltage;
        return this;
    }

    public InventoryAmdGpuOverclockingBuilder WithEnhancedOverclock(bool enhancedOverclock = default)
    {
        _enhancedOverclock = enhancedOverclock;
        return this;
    }

    public override AmdGpuOverclocking Build()
    {
        return new AmdGpuOverclocking
        {
            CoreClockLock = _coreClockLock,
            CoreClockState = _coreClockState,
            CoreVoltage = _coreVoltage,
            CoreVoltageOffset = _coreVoltageOffset,
            MemoryClockLock = _memoryClockLock,
            MemoryClockState = _memoryClockState,
            MemoryVoltage = _memoryVoltage,
            MemoryControllerVoltage = _memoryControllerVoltage,
            MemoryTweak = _memoryTweak,
            SocFrequency = _socFrequency,
            SocVoltage = _socVoltage,
            AlternativeDownVoltage = _alternativeDownVoltage,
            EnhancedOverclock = _enhancedOverclock,
            FanSpeed = _fanSpeed,
            PowerLimit = _powerLimit,
        };
    }
}
