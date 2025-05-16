using FluentValidation;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;

namespace MNX.MonitoringCenter.Inventory.UseCases.RigInventory.RigInventoryValidation.GpuModelValidation;

/// <summary>
/// Валидатор параметров сущности <see cref="GpuChangingValue"/>
/// </summary>
public class GpuChangingValueModelValidator : AbstractValidator<GpuChangingValue>
{
    public GpuChangingValueModelValidator()
    {
        RuleFor(model => model.Lock)
            .NotNull()
                .WithMessage($"{nameof(GpuChangingValue.Lock)} is required")
            .SetValidator(new RangeValueModelValidator());

        RuleFor(model => model.Offset)
            .NotNull()
                .WithMessage($"{nameof(GpuChangingValue.Offset)} is required")
            .SetValidator(new RangeValueModelValidator());
    }
}