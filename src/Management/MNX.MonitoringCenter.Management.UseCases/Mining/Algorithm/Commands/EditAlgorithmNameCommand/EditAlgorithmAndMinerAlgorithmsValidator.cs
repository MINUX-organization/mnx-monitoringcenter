using FluentValidation;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm.Commands.EditAlgorithmNameCommand;

/// <summary>
/// Валидатор команды <see cref="EditAlgorithmAndMinerAlgorithmsCommand"/>.
/// </summary>
public class EditAlgorithmAndMinerAlgorithmsValidator 
    : AbstractValidator<EditAlgorithmAndMinerAlgorithmsCommand>
{
    public EditAlgorithmAndMinerAlgorithmsValidator()
    {
        RuleFor(command => command.Model)
            .NotNull()
                .WithMessage("Algorithm data is required")
            .SetValidator(new AlgorithmModelValidator());
    }
}
