using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Crypto.AddCryptocurrency;

/// <summary>
/// Валидатор для команды добавления криптовалюты
/// </summary>
public class AddCryptocurrencyValidator : AbstractValidator<AddCryptocurrencyCommand>
{
    public AddCryptocurrencyValidator()
    {
        RuleFor(x => x.Model)
            .NotNull()
            .WithMessage("Cryptocurrency data is required")
            .SetValidator(x => new CryptocurrencyModelValidator());
    }
}
