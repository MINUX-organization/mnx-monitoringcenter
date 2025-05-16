using FluentValidation;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu;

namespace MNX.MonitoringCenter.Inventory.UseCases.RigInventory.RigInventoryValidation.GpuModelValidation.GpuOverclockingValidation;

/// <summary>
/// Валидатор параметров сущности <see cref="GpuOverclocking"/>
/// </summary>
public class GpuOverclockingModelValidator : AbstractValidator<GpuOverclocking>
{
    public GpuOverclockingModelValidator()
    {
        RuleFor(model => model.PowerLimit)
            .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(GpuOverclocking.PowerLimit)} cannot be negative");

        RuleFor(model => model.FanSpeed)
            .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(GpuOverclocking.FanSpeed)} cannot be negative");

        RuleFor(model => model.CoreClockLock)
            .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(GpuOverclocking.CoreClockLock)} cannot be negative");

        RuleFor(model => model.CoreClockOffset)
            .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(GpuOverclocking.CoreClockOffset)} cannot be negative");

        RuleFor(model => model.MemoryClockLock)
            .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(GpuOverclocking.MemoryClockLock)} cannot be negative");

        RuleFor(model => model.MemoryClockOffset)
            .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(GpuOverclocking.MemoryClockOffset)} cannot be negative");

        RuleFor(model => model.CoreVoltage)
            .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(GpuOverclocking.CoreVoltage)} cannot be negative");

        RuleFor(model => model.CoreVoltageOffset)
            .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(GpuOverclocking.CoreVoltageOffset)} cannot be negative");

        RuleFor(model => model.MemoryVoltage)
            .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(GpuOverclocking.MemoryVoltage)} cannot be negative");

        RuleFor(model => model.MemoryVoltageOffset)
            .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(GpuOverclocking.MemoryVoltageOffset)} cannot be negative");
    }
}
