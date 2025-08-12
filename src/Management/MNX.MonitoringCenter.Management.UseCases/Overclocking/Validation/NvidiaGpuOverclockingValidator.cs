using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu;

namespace MNX.MonitoringCenter.Management.UseCases.Overclocking.Validation;

/// <summary>
/// Валидатор разгона видеокарты модели Nvidia.
/// </summary>
public class NvidiaGpuOverclockingValidator : OverclockingValidatorBase<NvidiaGpuOverclocking>
{
    ///
    public NvidiaGpuOverclockingValidator() { } // нужен для инициализации пайплайна валидации.

    ///
    public NvidiaGpuOverclockingValidator(GpuRestrictions restrictions)
    {
        var rest = (NvidiaGpuRestrictions)restrictions;

        AddRangeValidatorRule(x => x.PowerLimit,
            nameof(NvidiaGpuOverclocking.PowerLimit), rest.Power.Minimal, rest.Power.Maximal);

        // TODO: Добавить SetValidator для параметра FanSpeed.

        AddRangeValidatorRule(x => x.CoreClockLock,
            nameof(NvidiaGpuOverclocking.CoreClockLock), rest.ClockCoreLock.Minimal, rest.ClockCoreLock.Maximal);

        AddRangeValidatorRule(x => x.CoreClockOffset,
            nameof(NvidiaGpuOverclocking.CoreClockOffset), rest.ClockCoreOffset.Minimal, rest.ClockCoreOffset.Maximal);

        AddRangeValidatorRule(x => x.MemoryClockLock,
            nameof(NvidiaGpuOverclocking.MemoryClockLock), rest.ClockMemoryLock.Minimal, rest.ClockMemoryLock.Maximal);

        AddRangeValidatorRule(x => x.MemoryClockOffset,
            nameof(NvidiaGpuOverclocking.MemoryClockOffset), rest.ClockMemoryOffset.Minimal, rest.ClockMemoryOffset.Maximal);

        AddRangeValidatorRule(x => x.CoreVoltage,
            nameof(NvidiaGpuOverclocking.CoreVoltage), rest.VoltageCoreLock.Minimal, rest.VoltageCoreLock.Maximal);

        AddRangeValidatorRule(x => x.CoreVoltageOffset,
            nameof(NvidiaGpuOverclocking.CoreVoltageOffset), rest.VoltageCoreOffset.Minimal, rest.VoltageCoreOffset.Maximal);

        AddRangeValidatorRule(x => x.MemoryVoltage,
            nameof(NvidiaGpuOverclocking.MemoryVoltage), rest.VoltageMemoryLock.Minimal, rest.VoltageMemoryLock.Maximal);

        AddRangeValidatorRule(x => x.MemoryVoltageOffset,
            nameof(NvidiaGpuOverclocking.MemoryVoltageOffset), rest.VoltageMemoryOffset.Minimal, rest.VoltageMemoryOffset.Maximal);
    }
}
