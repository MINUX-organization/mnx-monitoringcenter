using FluentValidation;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rig;

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

                RuleFor(x => x.Message.Inventory)
                    .NotNull()
                    .WithMessage(x => "Inventory is required")
                    .DependentRules(() =>
                    {
                        RuleFor(x => x.Message.Inventory.Cpus)
                            .NotEmpty()
                            .WithMessage(x => "Cpus inventory is required");

                        RuleFor(x => x.Message.Inventory.Drives)
                            .NotEmpty()
                            .WithMessage(x => "Drives inventory is required");

                        RuleFor(x => x.Message.Inventory.Gpus)
                            .NotEmpty()
                            .WithMessage(x => "Gpus inventory is required");

                        RuleFor(x => x.Message.Inventory.NetworkAdapters)
                            .NotEmpty()
                            .WithMessage(x => "Network adapters inventory is required");

                        RuleFor(x => x.Message.Inventory.Motherboard)
                            .NotNull()
                            .WithMessage(x => "Motherboard inventory is required");

                        RuleFor(x => x.Message.Inventory.Software)
                            .NotNull()
                            .WithMessage(x => "Software inventory is required");
                    });
            });
    }
}
