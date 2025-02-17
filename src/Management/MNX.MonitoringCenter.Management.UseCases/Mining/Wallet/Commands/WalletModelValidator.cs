using FluentValidation;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Wallet.Commands;

/// <summary>
/// Валидатор модели кошелька.
/// </summary>
public class WalletModelValidator : AbstractValidator<WalletInputModel>
{
    public WalletModelValidator()
    {
        RuleFor(x => x.Name.Length)
            .Must(length => 1 <= length && length <= 30)
            .WithMessage("The length of the wallet name should be in range of [1; 30] characters");

        RuleFor(x => x.Address.Length)
            .Must(length => 1 <= length)
            .WithMessage("The length of the address should be in range of [1; ...] characters");
    }
}
