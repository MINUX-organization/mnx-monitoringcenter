using FluentValidation;
using MNX.MonitoringCenter.Management.UseCases.Commands.Wallets;

namespace MNX.MonitoringCenter.Management.UseCases.Wallet.Commands.EditWallet;

/// <summary>
/// Валидатор для команды редактирования кошелька.
/// </summary>
public class EditWalletValidator : AbstractValidator<EditWalletCommand>
{
    public EditWalletValidator()
    {
        RuleFor(x => x.Model)
            .NotNull()
            .WithMessage("Wallet data is required")
            .SetValidator(x => new WalletModelValidator());
    }
}
