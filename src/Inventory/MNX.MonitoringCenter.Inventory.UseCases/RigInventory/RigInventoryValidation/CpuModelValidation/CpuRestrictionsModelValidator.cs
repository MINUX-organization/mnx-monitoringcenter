using FluentValidation;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Cpu;

namespace MNX.MonitoringCenter.Inventory.UseCases.RigInventory.RigInventoryValidation.CpuModelValidation;

/// <summary>
/// Валидатор параметров сущности <see cref="CpuRestrictions"/>.
/// </summary>
public class CpuRestrictionsModelValidator : AbstractValidator<CpuRestrictions>
{
    public CpuRestrictionsModelValidator()
    {
        RuleFor(model => model.Power)
            .NotNull()
                .WithMessage("Power is required")
            .SetValidator(new RangeValueModelValidator());

        RuleFor(model => model.FanSpeed)
            .NotNull()
                .WithMessage("FanSpeed is required")
            .SetValidator(new RangeValueModelValidator());

        RuleFor(model => model.Temperature)
            .NotNull()
                .WithMessage("Temperature is required")
            .SetValidator(new RangeValueModelValidator());

        RuleFor(model => model.Clock)
            .NotNull()
                .WithMessage("Clock is required")
            .SetValidator(new RangeValueModelValidator());
    }
}
