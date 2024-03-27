using FluentValidation.TestHelper;
using MNX.MonitoringCenter.Management.UseCases.Commands.Crypto;
using MNX.MonitoringCenter.Management.UseCases.Commands.Crypto.AddCryptocurrency;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Crypto.AddCryptocurrency;

[TestFixture]
public class AddCryptocurrencyValidatorTests
{
    private AddCryptocurrencyValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new AddCryptocurrencyValidator();
    }

    [Test]
    public void AddCryptocurrencyCommand_WhenModelNotNull_ShouldNotErrors()
    {
        var command = GetCommand();
        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Model);
    }

    [Test]
    public void AddCryptocurrencyCommand_WhenModelNull_ShouldErrors()
    {
        var command = new AddCryptocurrencyCommand(null!, TestHelper.UserId);
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Model)
            .WithErrorMessage("Данные для криптовалюты обязательны");
    }

    [TestCaseSource(typeof(CryptocurrencyCommandTestCase),
                    nameof(CryptocurrencyCommandTestCase.CreateCorrectInputModel))]
    public void AddCryptocurrencyCommand_WhenModelAreValid_ShouldNotErrors(CryptocurrencyInputModel model)
    {
        var command = new AddCryptocurrencyCommand(model, TestHelper.UserId);

        CryptocurrencyValidatorTests.
            ValidateCryptocurrencyModel_WhenCryptocurrencyModelAreValid(command.Model);
    }

    [TestCaseSource(typeof(CryptocurrencyCommandTestCase),
                    nameof(CryptocurrencyCommandTestCase.CreateIncorrectInputModel))]
    public void AddCryptocurrencyCommand_WhenModelAreNotValid_ShouldErrors(CryptocurrencyInputModel model)
    {
        var command = new AddCryptocurrencyCommand(model, TestHelper.UserId);

        CryptocurrencyValidatorTests
            .ValidateCryptocurrencyModel_WhenCryptocurrencyModelAreNotValid(command.Model);
    }

    private static AddCryptocurrencyCommand GetCommand()
    {
        var cryptocurrencyModel = CreateCryptocurrencyInputModel();

        return new AddCryptocurrencyCommand(cryptocurrencyModel, TestHelper.UserId);
    }

    private static CryptocurrencyInputModel CreateCryptocurrencyInputModel()
    {
        return new CryptocurrencyInputModel("SOL", "Solana", "HrenZnaet");
    }
}
