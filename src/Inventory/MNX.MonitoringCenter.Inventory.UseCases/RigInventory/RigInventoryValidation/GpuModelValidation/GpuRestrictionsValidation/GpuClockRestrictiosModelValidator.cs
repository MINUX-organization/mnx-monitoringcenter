using FluentValidation;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;

namespace MNX.MonitoringCenter.Inventory.UseCases.RigInventory.RigInventoryValidation.GpuModelValidation.GpuRestrictionsValidation;

/// <summary>
/// Валидатор параметров сущности <see cref="GpuClockRestrictions"/>.
/// </summary>
public class GpuClockRestrictiosModelValidator : AbstractValidator<GpuClockRestrictions>
{
    public GpuClockRestrictiosModelValidator()
    {
        RuleFor(model => model.Core)
            .NotNull()
                .WithMessage($"{nameof(GpuClockRestrictions.Core)} is required")
            .SetValidator(new GpuChangingValueModelValidator());

        RuleFor(model => model.Memory)
            .NotNull()
                .WithMessage($"{nameof(GpuClockRestrictions.Memory)} is required")
            .SetValidator(new GpuChangingValueModelValidator());
    }
}
