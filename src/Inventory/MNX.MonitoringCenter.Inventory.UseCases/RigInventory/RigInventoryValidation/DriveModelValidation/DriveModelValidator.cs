using FluentValidation;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Drive;

namespace MNX.MonitoringCenter.Inventory.UseCases.RigInventory.RigInventoryValidation.DriveModelValidation;

/// <summary>
/// Валидатор параметров сущности <see cref="Drive"/>.
/// </summary>
public class DriveModelValidator : AbstractValidator<Drive>
{
    public DriveModelValidator()
    {
        RuleFor(model => model.Information)
            .NotNull()
                .WithMessage($"{nameof(Drive.Information)} is required");

        RuleFor(model => model.Information.Capacity)
            .GreaterThan(0)
                .WithMessage($"{nameof(Drive.Information.Capacity)} cannot be zero or negative");
    }
}