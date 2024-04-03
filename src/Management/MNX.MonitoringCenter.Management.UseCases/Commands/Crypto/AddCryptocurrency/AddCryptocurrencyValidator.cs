using FluentValidation;

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
