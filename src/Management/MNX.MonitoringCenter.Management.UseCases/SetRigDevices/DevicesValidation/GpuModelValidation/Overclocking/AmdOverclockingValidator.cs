using FluentValidation;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.SetRigDevices.DevicesValidation.GpuModelValidation.Overclocking;

/// <summary>
/// Валидатор параметров сущности <see cref="AmdGpuOverclocking"/>.
/// </summary>
public class AmdOverclockingValidator : AbstractValidator<AmdGpuOverclocking>
{
    ///
    public AmdOverclockingValidator()
    {
        RuleFor(model => model.CoreClockLock)
            .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(AmdGpuOverclocking.CoreClockLock)} cannot be negative");

        RuleFor(model => model.CoreClockState)
            .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(AmdGpuOverclocking.CoreClockState)} cannot be negative");

        RuleFor(model => model.MemoryClockLock)
            .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(AmdGpuOverclocking.MemoryClockLock)} cannot be negative");

        RuleFor(model => model.MemoryClockState)
            .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(AmdGpuOverclocking.MemoryClockState)} cannot be negative");

        RuleFor(model => model.CoreVoltage)
            .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(AmdGpuOverclocking.CoreVoltage)} cannot be negative");

        RuleFor(model => model.CoreVoltageOffset)
            .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(AmdGpuOverclocking.CoreVoltageOffset)} cannot be negative");

        RuleFor(model => model.MemoryVoltage)
            .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(AmdGpuOverclocking.MemoryVoltage)} cannot be negative");

        RuleFor(model => model.SocFrequency)
            .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(AmdGpuOverclocking.SocFrequency)} cannot be negative");

        RuleFor(model => model.SocVoltage)
            .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(AmdGpuOverclocking.SocVoltage)} cannot be negative");
    }
}
