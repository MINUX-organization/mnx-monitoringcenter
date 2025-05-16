using FluentValidation;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Cpu;

namespace MNX.MonitoringCenter.Inventory.UseCases.RigInventory.RigInventoryValidation.CpuModelValidation;

/// <summary>
/// Валидатор параметров сущности <see cref="CpuOverclocking"/>.
/// </summary>
public class CpuOverclockingModelValidator : AbstractValidator<CpuOverclocking>
{
    public CpuOverclockingModelValidator()
    {
        RuleFor(model => model.CoreClockLock)
            .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(CpuOverclocking.CoreClockLock)} cannot be negative");

        RuleFor(model => model.CoreVoltage)
            .GreaterThanOrEqualTo(0)
                .WithMessage($"{nameof(CpuOverclocking.CoreVoltage)} cannot be negative");
    }
}
