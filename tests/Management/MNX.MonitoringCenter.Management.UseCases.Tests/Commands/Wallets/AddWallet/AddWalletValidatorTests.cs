using FluentValidation.TestHelper;
using MNX.MonitoringCenter.Management.UseCases.Commands.Wallets;
using MNX.MonitoringCenter.Management.UseCases.Commands.Wallets.AddWallet;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Wallets.AddWallet;

[TestFixture]
public class AddWalletValidatorTests
{
    private AddWalletValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new AddWalletValidator();
    }

    [Test]
    public void AddWalletCommand_WhenModelNotNull_ShouldNotErrors()
    {
        var command = GetCommand();
        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Model);
    }

    [Test]
    public void AddWalletCommand_WhenModelNull_ShouldErrors()
    {
        var command = new AddWalletCommand(null!, TestHelper.UserId);
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Model)
            .WithErrorMessage("Wallet data is required");
    }

    [TestCaseSource(typeof(WalletCommandTestCase),
                    nameof(WalletCommandTestCase.CreateCorrectWalletModel))]
    public void AddWalletCommand_WhenWalletModelAreValid_ShouldNotErrors(WalletInputModel model)
    {
        var command = new AddWalletCommand(model, TestHelper.UserId);

        WalletValidatorTests
            .ValidateWalletModel_WhenModelAreValid(command.Model);
    }

    [TestCaseSource(typeof(WalletCommandTestCase),
                    nameof(WalletCommandTestCase.CreateIncorrectWalletModel))]
    public void AddWalletCommand_WhenModelAreNotValid_ShouldErrors(WalletInputModel model)
    {
        var command = new AddWalletCommand(model, TestHelper.UserId);

        WalletValidatorTests
            .ValidateWalletModel_WhenModelAreNotValid(command.Model);
    }

    private static AddWalletCommand GetCommand()
    {
        var walletModel = CreateWalletInputModel();

        return new AddWalletCommand(walletModel, TestHelper.UserId);
    }

    private static WalletInputModel CreateWalletInputModel()
    {
        return new WalletInputModel("name", "address", Guid.NewGuid());
    }
}
