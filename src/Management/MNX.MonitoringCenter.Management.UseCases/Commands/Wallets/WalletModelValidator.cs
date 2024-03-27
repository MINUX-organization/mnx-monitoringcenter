using FluentValidation;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Wallets;

/// <summary>
/// Валидатор модели кошелька.
/// </summary>
public class WalletModelValidator : AbstractValidator<WalletInputModel>
{
    public WalletModelValidator()
    {
        RuleFor(x => x.Name.Length)
            .NotEmpty()
            .Must(name => name <= 30)
            .WithMessage("The length of the wallet name should be in range of [1; 30] characters");

        RuleFor(x => x.Address.Length)
            .NotEmpty()
            .Must(address => address <= 60)
            .WithMessage("The length of the address should be in range of [1; 60] characters");
    }
}
