using FluentValidation.TestHelper;
using MNX.MonitoringCenter.Management.UseCases.Wallet.Commands;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Wallets;

public class WalletValidatorTests
{
    private static WalletModelValidator _validator =
        new WalletModelValidator();

    public static void ValidateWalletModel_WhenModelAreValid(WalletInputModel model)
    {
        var result = _validator.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(x => x.Name.Length);
        result.ShouldNotHaveValidationErrorFor(x => x.Address.Length);
    }

    public static void ValidateWalletModel_WhenModelAreNotValid(WalletInputModel model)
    {
        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Name.Length)
            .WithErrorMessage("The length of the wallet name should be in range of [1; 30] characters");

        result.ShouldHaveValidationErrorFor(x => x.Address.Length)
            .WithErrorMessage("The length of the address should be in range of [1; 60] characters");
    }
}
