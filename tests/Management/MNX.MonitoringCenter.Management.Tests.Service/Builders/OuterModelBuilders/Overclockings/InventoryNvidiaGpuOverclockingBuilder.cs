using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Overclocking;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.OuterModelBuilders.Overclockings;

public class InventoryNvidiaGpuOverclockingBuilder :
    InventoryGpuOverclockingBuilder<InventoryNvidiaGpuOverclockingBuilder, NvidiaGpuOverclocking>
{
    protected int? _coreClockLock;
    protected int? _coreClockOffset;
    protected int? _coreVoltage;
    protected int? _coreVoltageOffset;
    protected int? _memoryClockLock;
    protected int? _memoryClockOffset;
    protected int? _memoryVoltage;
    protected int? _memoryVoltageOffset;

    public InventoryNvidiaGpuOverclockingBuilder WithCoreClockLock(int coreClockLock = 0)
    {
        _coreClockLock = coreClockLock;
        return this;
    }

    public InventoryNvidiaGpuOverclockingBuilder WithCoreClockOffset(int coreClockOffset = 0)
    {
        _coreClockOffset = coreClockOffset;
        return this;
    }

    public InventoryNvidiaGpuOverclockingBuilder WithCoreVoltage(int coreVoltage = 0)
    {
        _coreVoltage = coreVoltage;
        return this;
    }

    public InventoryNvidiaGpuOverclockingBuilder WithCoreVoltageOffset(int coreVoltageOffset = 0)
    {
        _coreVoltageOffset = coreVoltageOffset;
        return this;
    }

    public InventoryNvidiaGpuOverclockingBuilder WithMemoryClockLock(int memoryClockLock = 0)
    {
        _memoryClockLock = memoryClockLock;
        return this;
    }

    public InventoryNvidiaGpuOverclockingBuilder WithMemoryClockOffset(int memoryClockOffset = 0)
    {
        _memoryClockOffset = memoryClockOffset;
        return this;
    }

    public InventoryNvidiaGpuOverclockingBuilder WithMemoryVoltage(int memoryVoltage = 0)
    {
        _memoryVoltage = memoryVoltage;
        return this;
    }

    public InventoryNvidiaGpuOverclockingBuilder WithMemoryVoltageOffset(int memoryVoltageOffset = 0)
    {
        _memoryVoltageOffset = memoryVoltageOffset;
        return this;
    }

    public override NvidiaGpuOverclocking Build()
    {
        return new NvidiaGpuOverclocking
        {
            CoreClockLock = _coreClockLock,
            CoreClockOffset = _coreClockOffset,
            CoreVoltage = _coreVoltage,
            CoreVoltageOffset = _coreVoltageOffset,
            MemoryClockLock = _memoryClockLock,
            MemoryClockOffset = _memoryClockOffset,
            MemoryVoltage = _memoryVoltage,
            MemoryVoltageOffset = _memoryVoltageOffset,
            FanSpeed = _fanSpeed,
            PowerLimit = _powerLimit,
        };
    }
}
