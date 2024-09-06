using FluentValidation;

namespace MNX.MonitoringCenter.Inventory.UseCases;

/// <summary>
/// Валидатор команды сохранения инвентаризации.
/// </summary>
public class SaveInventoryCommandValidator : AbstractValidator<SaveInventoryCommand>
{
    public SaveInventoryCommandValidator()
    {
        RuleFor(x => x.Message)
            .NotNull()
            .WithMessage(x => "Inventory message is required")
            .DependentRules(() =>
            {
                RuleFor(x => x.Message.RigId)
                    .NotEmpty()
                    .WithMessage(x => "Rig id is required");

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
