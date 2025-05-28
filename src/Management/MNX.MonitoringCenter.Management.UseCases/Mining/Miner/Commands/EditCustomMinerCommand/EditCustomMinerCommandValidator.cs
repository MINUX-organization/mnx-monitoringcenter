using FluentValidation;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.Models;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.EditMinerCommand;

/// <summary>
/// Валидатор параметров сущности <see cref="EditCustomMinerCommand"/>.
/// </summary>
public class EditCustomMinerCommandValidator : AbstractValidator<EditCustomMinerCommand>
{
    public EditCustomMinerCommandValidator()
    {
        RuleFor(x => x.Model)
            .NotNull()
            .WithMessage("Miner data is required!")
            .SetValidator(cmd => new MinerInputModelValidator());
    }
}