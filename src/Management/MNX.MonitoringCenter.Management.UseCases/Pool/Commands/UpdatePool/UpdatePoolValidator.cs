using FluentValidation;

namespace MNX.MonitoringCenter.Management.UseCases.Pool.Commands.UpdatePool;

public class UpdatePoolValidator : AbstractValidator<UpdatePoolCommand>
{
    public UpdatePoolValidator()
    {
        RuleFor(x => x.Model)
            .NotNull()
            .WithMessage("Pool data is required")
            .SetValidator(x => new PoolModelValidator());
    }
}
