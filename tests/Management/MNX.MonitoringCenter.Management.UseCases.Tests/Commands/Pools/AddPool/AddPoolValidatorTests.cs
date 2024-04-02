using FluentValidation.TestHelper;
using MNX.MonitoringCenter.Management.UseCases.Commands.Pools;
using MNX.MonitoringCenter.Management.UseCases.Commands.Pools.AddPool;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Pools.AddPool;

[TestFixture]
public class AddPoolValidatorTests
{
    private AddPoolValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new AddPoolValidator();
    }

    [Test]
    public void AddPoolCommand_WhenModelNotNull_ShouldNotErrors()
    {
        var command = GetCommand();
        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Model);
    }

    [Test]
    public void AddPoolCommand_WhenModelNull_ShouldErrors()
    {
        var command = new AddPoolCommand(null!, TestHelper.UserId);
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Model)
            .WithErrorMessage("Pool data is required");
    }

    [TestCaseSource(typeof(PoolCommandTestCase),
                    nameof(PoolCommandTestCase.CreateCorrectPoolModel))]
    public void AddPoolCommand_WhenPoolModelAreValid_ShouldNotErrors(PoolInputModel model)
    {
        var command = new AddPoolCommand(model, TestHelper.UserId);

        PoolValidatorTests
            .ValidatePoolModel_WhenPoolModelAreValid(command.Model);
    }

    [TestCaseSource(typeof(PoolCommandTestCase),
                    nameof(PoolCommandTestCase.CreateIncorrectPoolModel))]
    public void AddPoolCommand_WhenPoolModelAreNotValid_ShouldErrors(PoolInputModel model)
    {
        var command = new AddPoolCommand(model, TestHelper.UserId);

        PoolValidatorTests
            .ValidatePoolModel_WhenPoolModelAreNotValid(command.Model);
    }

    private static AddPoolCommand GetCommand()
    {
        var poolModel = CreatePoolInputModel();

        return new AddPoolCommand(poolModel, TestHelper.UserId);
    }

    private static PoolInputModel CreatePoolInputModel()
    {
        return new PoolInputModel("domain", 123321, Guid.NewGuid());
    }
}
