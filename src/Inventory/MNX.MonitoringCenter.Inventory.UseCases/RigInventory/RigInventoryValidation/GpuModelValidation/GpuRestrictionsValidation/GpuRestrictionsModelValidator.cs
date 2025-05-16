using FluentValidation;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;

namespace MNX.MonitoringCenter.Inventory.UseCases.RigInventory.RigInventoryValidation.GpuModelValidation.GpuRestrictionsValidation;

/// <summary>
/// Валидатор параметров для сущности <see cref="GpuRestrictions"/>.
/// </summary>
public class GpuRestrictionsModelValidator : AbstractValidator<GpuRestrictions>
{
    public GpuRestrictionsModelValidator()
    {
        RuleFor(model => model.Power)
            .NotNull()
                .WithMessage($"{nameof(GpuRestrictions.Power)} is required")
            .SetValidator(new RangeValueModelValidator());

        RuleFor(model => model.FanSpeed)
            .NotNull()
                .WithMessage($"{nameof(GpuRestrictions.FanSpeed)} is required")
            .SetValidator(new RangeValueModelValidator());

        RuleFor(model => model.Temperature)
            .NotNull()
                .WithMessage($"{nameof(GpuRestrictions.Temperature)} is required")
            .SetValidator(new GpuTemperatureRestrictionsModelValidator());

        RuleFor(model => model.Voltage)
            .NotNull()
                .WithMessage($"{nameof(GpuRestrictions.Voltage)} is required")
            .SetValidator(new GpuVoltageRestrictionsModelValidator());

        RuleFor(model => model.Clock)
            .NotNull()
                .WithMessage($"{nameof(GpuRestrictions.Clock)} is required")
            .SetValidator(new GpuClockRestrictiosModelValidator());
    }
}

/// <summary>
/// Валидатор параметров для сущности <see cref="GpuTemperatureRestrictions"/>.
/// </summary>
public class GpuTemperatureRestrictionsModelValidator : AbstractValidator<GpuTemperatureRestrictions>
{
    public GpuTemperatureRestrictionsModelValidator()
    {
        RuleFor(model => model.Core)
            .NotNull()
                .WithMessage($"{nameof(GpuTemperatureRestrictions.Core)} is required")
            .SetValidator(new RangeValueModelValidator());

        RuleFor(model => model.Memory)
            .NotNull()
                .WithMessage($"{nameof(GpuTemperatureRestrictions.Memory)} is required")
            .SetValidator(new RangeValueModelValidator());
    }
}