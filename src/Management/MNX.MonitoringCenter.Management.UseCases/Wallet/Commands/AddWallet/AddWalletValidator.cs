using FluentValidation;
using MNX.MonitoringCenter.Management.UseCases.Commands.Wallets;

namespace MNX.MonitoringCenter.Management.UseCases.Wallet.Commands.AddWallet;

/// <summary>
/// Валидатор для команды добавления кошелька.
/// </summary>
public class AddWalletValidator : AbstractValidator<AddWalletCommand>
{
    public AddWalletValidator()
    {
        RuleFor(x => x.Model)
            .NotNull()
            .WithMessage("Wallet data is required")
            .SetValidator(x => new WalletModelValidator());
    }
}
