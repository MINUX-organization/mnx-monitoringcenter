using FluentValidation;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Pools.UpdatePool;

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
