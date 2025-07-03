using FluentValidation;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Overclocking;

namespace MNX.MonitoringCenter.Inventory.UseCases.RigInventory.RigInventoryValidation.GpuModelValidation.Overclocking;

/// <summary>
/// Валидатор параметров сущности <see cref="NvidiaGpuOverclocking"/>.
/// </summary>
public class NvidiaOverclockingValidator : AbstractValidator<NvidiaGpuOverclocking>
{
    ///
    public NvidiaOverclockingValidator()
    {
        RuleFor(model => model.CoreClockLock)
            .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(NvidiaGpuOverclocking.CoreClockLock)} cannot be negative");

        RuleFor(model => model.CoreClockOffset)
            .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(NvidiaGpuOverclocking.CoreClockOffset)} cannot be negative");

        RuleFor(model => model.MemoryClockLock)
            .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(NvidiaGpuOverclocking.MemoryClockLock)} cannot be negative");

        RuleFor(model => model.MemoryClockOffset)
            .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(NvidiaGpuOverclocking.MemoryClockOffset)} cannot be negative");

        RuleFor(model => model.CoreVoltage)
            .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(NvidiaGpuOverclocking.CoreVoltage)} cannot be negative");

        RuleFor(model => model.CoreVoltageOffset)
            .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(NvidiaGpuOverclocking.CoreVoltageOffset)} cannot be negative");

        RuleFor(model => model.MemoryVoltage)
            .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(NvidiaGpuOverclocking.MemoryVoltage)} cannot be negative");

        RuleFor(model => model.MemoryVoltageOffset)
            .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(NvidiaGpuOverclocking.MemoryVoltageOffset)} cannot be negative");
    }
}
