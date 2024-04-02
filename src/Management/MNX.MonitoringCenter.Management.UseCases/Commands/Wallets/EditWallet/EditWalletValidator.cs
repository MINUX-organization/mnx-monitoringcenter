using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Wallets.EditWallet;

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
