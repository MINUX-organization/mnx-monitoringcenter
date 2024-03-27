using FluentValidation;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Crypto;

/// <summary>
/// Валидатор модели криптовалюты.
/// </summary>
public class CryptocurrencyModelValidator : AbstractValidator<CryptocurrencyInputModel>
{
    public CryptocurrencyModelValidator()
    {
        RuleFor(x => x.FullName.Length)
            .NotEmpty()
            .Must(fullName => 2 <= fullName && fullName <= 40)
            .WithMessage("The length of the cryptocurrency name should be in the range of [2,40] characters.");

        RuleFor(x => x.ShortName.Length)
            .NotEmpty()
            .Must(shortName => 2 <= shortName && shortName <= 10)
            .WithMessage("The length of the cryptocurrency short name should be in the range of [2,10] characters.");
    }
}
