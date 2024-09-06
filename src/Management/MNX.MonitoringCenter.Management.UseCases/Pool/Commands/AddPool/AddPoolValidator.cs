using FluentValidation;

namespace MNX.MonitoringCenter.Management.UseCases.Pool.Commands.AddPool;

/// <summary>
/// Валидатор для команды добавления пула.
/// </summary>
public class AddPoolValidator : AbstractValidator<AddPoolCommand>
{
    public AddPoolValidator()
    {
        RuleFor(x => x.Model)
            .NotNull()
            .WithMessage("Pool data is required")
            .SetValidator(x => new PoolModelValidator());
    }
}
