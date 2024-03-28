using FluentValidation.TestHelper;
using MNX.MonitoringCenter.Management.UseCases.Commands.Wallets;
using MNX.MonitoringCenter.Management.UseCases.Commands.Wallets.EditWallet;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Wallets.EditWallet;

[TestFixture]
public class EditWalletValidatorTests
{
    private EditWalletValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new EditWalletValidator();
    }

    [Test]
    public void EditWalletCommand_WhenModelNotNull_ShouldNotErrors()
    {
        var command = GetCommand(CreateWalletModel());
        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Model);
    }

    [Test]
    public void EditWalletCommand_WhenModelNull_ShouldErrors()
    {
        var command = GetCommand(null!);
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Model)
            .WithErrorMessage("Wallet data is required");
    }

    [TestCaseSource(typeof(WalletCommandTestCase),
                    nameof(WalletCommandTestCase.CreateCorrectWalletModel))]
    public void EditWalletCommand_WhenModelAreValid_ShouldNotErrors(WalletInputModel model)
    {
        var command = new EditWalletCommand(Guid.NewGuid(), model, TestHelper.UserId);

        WalletValidatorTests
            .ValidateWalletModel_WhenModelAreValid(command.Model);
    }

    [TestCaseSource(typeof(WalletCommandTestCase),
                    nameof(WalletCommandTestCase.CreateIncorrectWalletModel))]
    public void EditWalletCommand_WhenModelAreNotValid_ShouldErrors(WalletInputModel model)
    {
        var command = new EditWalletCommand(Guid.NewGuid(), model, TestHelper.UserId);

        WalletValidatorTests
            .ValidateWalletModel_WhenModelAreNotValid(command.Model);
    }

    private static EditWalletCommand GetCommand(WalletInputModel model)
    {
        return new EditWalletCommand(Guid.NewGuid(), model, TestHelper.UserId);
    }

    private static WalletInputModel CreateWalletModel()
    {
        return new WalletInputModel("name", "address", Guid.NewGuid());
    }
}
