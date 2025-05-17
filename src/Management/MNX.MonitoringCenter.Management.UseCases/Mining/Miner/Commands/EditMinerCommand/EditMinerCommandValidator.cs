using FluentValidation;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.Models;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.EditMinerCommand;

/// <summary>
/// Валидатор параметров сущности <see cref="EditMinerCommand"/>.
/// </summary>
public class EditMinerCommandValidator : AbstractValidator<EditMinerCommand>
{
    public EditMinerCommandValidator()
    {
        RuleFor(x => x.Model)
            .NotNull()
            .WithMessage("Miner data is required!")
            .SetValidator(cmd => new MinerInputModelValidator());
    }
}