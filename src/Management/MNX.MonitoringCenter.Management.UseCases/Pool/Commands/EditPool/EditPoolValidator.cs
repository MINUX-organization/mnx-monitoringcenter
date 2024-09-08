using FluentValidation;

namespace MNX.MonitoringCenter.Management.UseCases.Pool.Commands.EditPool;

/// <summary>
/// Валидатор команды редактирования пула.
/// </summary>
public class EditPoolValidator : AbstractValidator<EditPoolCommand>
{
    public EditPoolValidator()
    {
        RuleFor(x => x.Model)
            .NotNull()
            .WithMessage("Pool data is required")
            .SetValidator(x => new PoolModelValidator());
    }
}
