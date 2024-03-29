using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Wallets.AddWallet;

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
