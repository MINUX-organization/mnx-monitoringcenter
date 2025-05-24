using FluentValidation;
using MNX.MonitoringCenter.Inventory.Contracts;

namespace MNX.MonitoringCenter.Inventory.UseCases.RigInventory.RigInventoryValidation.SoftwareInventoryValidation;

/// <summary>
/// Валидатор параметров сущности <see cref="MinerInventory"/>.
/// </summary>
public class MinerInventoryModelValidator : AbstractValidator<MinerInventory>
{
    public MinerInventoryModelValidator()
    {
        RuleFor(model => model.Name)
            .Must(value => !string.IsNullOrEmpty(value))
                .WithMessage($"{nameof(MinerInventory.Name)} cannot be null or empty");

        RuleFor(model => model.Version)
            .Must(value => !string.IsNullOrEmpty(value))
                .WithMessage($"{nameof(MinerInventory.Version)} cannot be null or empty");
    }
}
