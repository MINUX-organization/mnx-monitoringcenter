using FluentValidation;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm.Commands.AddAlgorithmCommand;

/// <summary>
/// Валидатор команды <see cref="AddAlgorithmCommand"/>.
/// </summary>
public class AddAlgorithmValidator : AbstractValidator<AddAlgorithmCommand>
{
    public AddAlgorithmValidator()
    {
        RuleFor(command => command.Model)
            .NotNull()
                .WithMessage("Algorithm data is required")
            .SetValidator(new AlgorithmModelValidator());
    }
}
