using FluentValidation.TestHelper;
using MNX.MonitoringCenter.Management.UseCases.Commands.Pools;
using MNX.MonitoringCenter.Management.UseCases.Commands.Pools.UpdatePool;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Pools.UpdatePool;

[TestFixture]
public class UpdatePoolValidatorTests
{
    private UpdatePoolValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new UpdatePoolValidator();
    }

    [Test]
    public void UpdatePoolCommand_WhenModelNotNull_ShouldNotErrors()
    {
        var command = GetCommand(CreatePoolModel());
        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Model);
    }

    [Test]
    public void UpdatePoolCommand_WhenModelNull_ShouldErrors()
    {
        var command = GetCommand(null!);
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Model)
            .WithErrorMessage("Pool data is required");
    }

    [TestCaseSource(typeof(PoolCommandTestCase),
                    nameof(PoolCommandTestCase.CreateCorrectPoolModel))]
    public void UpdatePoolCommand_WhenPoolModelAreValid_ShouldNotErrors(PoolInputModel model)
    {
        var command = new UpdatePoolCommand(Guid.NewGuid(), model, TestHelper.UserId);

        PoolValidatorTests
            .ValidatePoolModel_WhenPoolModelAreValid(command.Model);
    }

    [TestCaseSource(typeof(PoolCommandTestCase),
                    nameof(PoolCommandTestCase.CreateIncorrectPoolModel))]
    public void UpdatePoolCommand_WhenPoolModelAreNotValid_ShouldErrors(PoolInputModel model)
    {
        var command = new UpdatePoolCommand(Guid.NewGuid(), model, TestHelper.UserId);

        PoolValidatorTests
            .ValidatePoolModel_WhenPoolModelAreNotValid(command.Model);
    }

    private static UpdatePoolCommand GetCommand(PoolInputModel model)
    {
        return new UpdatePoolCommand(Guid.NewGuid(), model, TestHelper.UserId);
    }

    private static PoolInputModel CreatePoolModel()
    {
        return new PoolInputModel("domain", 1233, Guid.NewGuid());
    }
}
