using FluentValidation;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.NetworkAdapter;

namespace MNX.MonitoringCenter.Inventory.UseCases.RigInventory.RigInventoryValidation.NetworkAdapterValidation;

/// <summary>
/// Валидатор параметров сущности <see cref="NetworkAdapter"/>
/// </summary>
public class NetworkAdaperModelValidator : AbstractValidator<NetworkAdapter>
{
    public NetworkAdaperModelValidator()
    {
        RuleFor(model => model.Information)
            .NotNull()
                .WithMessage($"{nameof(NetworkAdapter.Information)} is required");

        RuleFor(model => model.Information.LogicalName)
            .Must(value => !string.IsNullOrEmpty(value))
                .WithMessage($"{nameof(NetworkAdapter.Information.LogicalName)} is required");

        RuleFor(model => model.Information.Mac)
            .Must(value => !string.IsNullOrWhiteSpace(value))
                .WithMessage($"{nameof(NetworkAdapter.Information.Mac)} is required");
    }
}