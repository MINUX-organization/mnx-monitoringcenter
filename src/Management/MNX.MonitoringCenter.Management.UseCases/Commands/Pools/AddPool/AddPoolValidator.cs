using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Pools.AddPool;

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
