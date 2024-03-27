using FluentValidation;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Pools;

public class PoolModelValidator : AbstractValidator<PoolInputModel>
{
    public PoolModelValidator()
    {
        RuleFor(x => x.Port)
            .Must(port => 0 <= port && port <= 65535)
            .WithMessage("The port must be in the range [0,65535]");

        RuleFor(x => x.Domain.Length)
            .NotEmpty()
            .Must(domain => domain <= 40)
            .WithMessage("The length of the domain should be in range of [1,40] characters.");
    }
}
