using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
