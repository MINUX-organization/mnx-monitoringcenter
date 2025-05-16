using FluentValidation;
using MNX.MonitoringCenter.Inventory.Contracts.Devices;

namespace MNX.MonitoringCenter.Inventory.UseCases.RigInventory.RigInventoryValidation;

/// <summary>
/// Валидатор параметров сущности <see cref="RangeValue"/>.
/// </summary>
public class RangeValueModelValidator : AbstractValidator<RangeValue>
{
    public RangeValueModelValidator()
    {
        RuleFor(model => model.Minimal)
            .LessThanOrEqualTo(x => x.Maximal)
                .WithMessage($"{nameof(RangeValue.Minimal)} cannot be greater than maximal");

        RuleFor(model => model.Maximal)
            .GreaterThanOrEqualTo(x => x.Minimal)
                .WithMessage($"{nameof(RangeValue.Maximal)} cannot be less than minimal");
    }
}
