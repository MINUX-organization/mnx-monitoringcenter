using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu;

namespace MNX.MonitoringCenter.Management.UseCases.Overclocking.Validation;

/// <summary>
/// Валидатор разгона видеокарты параметра Amd.
/// </summary>
public class AmdGpuOverclockingValidator : OverclockingValidatorBase<AmdGpuOverclocking>
{
    ///
    public AmdGpuOverclockingValidator() { } // нужен для инициализации пайплайна валидации.

    public AmdGpuOverclockingValidator(GpuRestrictions restrictions)
    {
        var rest = (AmdGpuRestrictions)restrictions;

        AddRangeValidatorRule(x => x.PowerLimit,
            nameof(AmdGpuOverclocking.PowerLimit), rest.Power.Minimal, rest.Power.Maximal);

        AddRangeValidatorRule(x => x.FanSpeed,
            nameof(AmdGpuOverclocking.FanSpeed), rest.FanSpeed.Minimal, rest.FanSpeed.Maximal);

        AddRangeValidatorRule(x => x.CoreClockLock,
            nameof(AmdGpuOverclocking.CoreClockLock), rest.ClockCoreLock.Minimal, rest.ClockCoreLock.Maximal);

        AddRangeValidatorRule(x => x.CoreClockState,
            nameof(AmdGpuOverclocking.CoreClockState), rest.ClockCoreState.Minimal, rest.ClockCoreState.Maximal);

        AddRangeValidatorRule(x => x.MemoryClockLock,
            nameof(AmdGpuOverclocking.MemoryClockLock), rest.ClockMemoryLock.Minimal, rest.ClockMemoryLock.Maximal);

        AddRangeValidatorRule(x => x.MemoryClockState,
            nameof(AmdGpuOverclocking.MemoryClockState), rest.ClockMemoryState.Minimal, rest.ClockMemoryState.Maximal);

        AddRangeValidatorRule(x => x.CoreVoltage,
            nameof(AmdGpuOverclocking.CoreVoltage), rest.VoltageCoreLock.Minimal, rest.VoltageCoreLock.Maximal);

        AddRangeValidatorRule(x => x.CoreVoltageOffset,
            nameof(AmdGpuOverclocking.CoreVoltageOffset), rest.VoltageCoreOffset.Minimal, rest.VoltageCoreOffset.Maximal);

        AddRangeValidatorRule(x => x.MemoryVoltage,
            nameof(AmdGpuOverclocking.MemoryVoltage), rest.VoltageMemoryLock.Minimal, rest.VoltageMemoryLock.Maximal);

        AddRangeValidatorRule(x => x.MemoryControllerVoltage,
            nameof(AmdGpuOverclocking.MemoryControllerVoltage), rest.VoltageMemoryController.Minimal, rest.VoltageMemoryController.Maximal);

        AddRangeValidatorRule(x => x.SocFrequency,
            nameof(AmdGpuOverclocking.SocFrequency), rest.SocFrequency.Minimal, rest.SocFrequency.Maximal);

        AddRangeValidatorRule(x => x.SocVoltage,
            nameof(AmdGpuOverclocking.SocVoltage), rest.SocVoltage.Minimal, rest.SocVoltage.Maximal);
    }
}
