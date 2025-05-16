using FluentValidation;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.NetworkAdapter;
using MNX.MonitoringCenter.Inventory.UseCases.RigInventory.RigInventoryValidation;
using MNX.MonitoringCenter.Inventory.UseCases.RigInventory.RigInventoryValidation.CpuModelValidation;
using MNX.MonitoringCenter.Inventory.UseCases.RigInventory.RigInventoryValidation.GpuModelValidation;
using MNX.MonitoringCenter.Inventory.UseCases.RigInventory.RigInventoryValidation.DriveModelValidation;
using MNX.MonitoringCenter.Inventory.UseCases.RigInventory.RigInventoryValidation.NetworkAdapterValidation;

namespace MNX.MonitoringCenter.Inventory.UseCases.RigInventory;

/// <summary>
/// Валидатор команды сохранения инвентаризации рига.
/// </summary>
public class SaveRigInventoryCommandValidator : AbstractValidator<SaveRigInventoryCommand>
{
    public SaveRigInventoryCommandValidator(IRigRepository rigRepository)
    {
        RuleFor(x => x.Message)
            .NotNull()
            .WithMessage(x => "Inventory message is required")
            .DependentRules(() =>
            {
                RuleFor(x => x.Message.RigId)
                    .NotEmpty()
                    .WithMessage(x => "Rig id is required")
                    .DependentRules(() =>
                    {
                        RuleFor(x => x.Message.RigId)
                            .MustAsync(async (rigId, cancellationToken)
                                => await rigRepository.Exists(rigId, cancellationToken))
                            .WithMessage("Rig not found");
                    });

                RuleFor(x => x.Message.RigOwnerId)
                    .NotEmpty()
                    .WithMessage(x => "Rig owner id is required");

                RuleFor(x => x.Message.Inventory)
                    .NotNull()
                    .WithMessage(x => "Inventory is required")
                    .DependentRules(() =>
                    {
                        RuleFor(x => x.Message.Inventory.Cpus)
                            .NotEmpty()
                            .WithMessage(x => "Cpus inventory is required")
                            .ForEach(x => x.SetValidator(new CpuModelValidator()));

                        RuleFor(x => x.Message.Inventory.Drives)
                            .NotEmpty()
                            .WithMessage(x => "Drives inventory is required")
                            .ForEach(x => x.SetValidator(new DriveModelValidator()));

                        RuleFor(x => x.Message.Inventory.Gpus)
                            .NotEmpty()
                            .WithMessage(x => "Gpus inventory is required")
                            .ForEach(x => x.SetValidator(new GpuModelValidator()));

                        RuleFor(x => x.Message.Inventory.NetworkAdapters)
                            .NotEmpty()
                            .WithMessage(x => "Network adapters inventory is required")
                            .Must(x => x.Any(adapter =>
                                !string.IsNullOrEmpty(adapter.GlobalIP) &&
                                !string.IsNullOrEmpty(adapter.LocalIP)))
                            .WithMessage($"At least one {nameof(NetworkAdapter)} must have whole IPs (local and global)")
                            .ForEach(x => x.SetValidator(new NetworkAdaperModelValidator()));

                        RuleFor(x => x.Message.Inventory.Motherboard)
                            .NotNull()
                            .WithMessage(x => "Motherboard inventory is required");

                        RuleFor(x => x.Message.Inventory.Software)
                            .NotNull()
                            .WithMessage(x => "Software inventory is required");

                        RuleFor(x => x.Message.Inventory)
                            .SetValidator(new RigInventoryModelValidator());
                    });

            });
    }
}
