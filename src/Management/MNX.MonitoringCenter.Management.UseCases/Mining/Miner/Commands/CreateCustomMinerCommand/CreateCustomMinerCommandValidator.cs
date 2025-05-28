using FluentValidation;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.Models;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Commands.CreateCustomMinerCommand;

/// <summary>
/// Валидатор параметров сущности <see cref="CreateCustomMinerCommand"/>.
/// </summary>
public class CreateCustomMinerCommandValidator : AbstractValidator<CreateCustomMinerCommand>
{
    public CreateCustomMinerCommandValidator()
    {
        RuleFor(x => x.Model)
            .NotNull()
            .WithMessage("Miner data is required!")
            .SetValidator(cmd => new MinerInputModelValidator());
    }
}