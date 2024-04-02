using FluentValidation.TestHelper;
using MNX.MonitoringCenter.Management.UseCases.Commands.Pools;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Pools;

public class PoolValidatorTests
{
    private static PoolModelValidator? _validator =
        new PoolModelValidator();

    public static void ValidatePoolModel_WhenPoolModelAreValid(PoolInputModel model)
    {
        var result = _validator.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(x => x.Port);
        result.ShouldNotHaveValidationErrorFor(x => x.Domain.Length);
    }

    public static void ValidatePoolModel_WhenPoolModelAreNotValid(PoolInputModel model)
    {
        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Port)
            .WithErrorMessage("The port must be in the range [0; 65535]");

        result.ShouldHaveValidationErrorFor(x => x.Domain.Length)
            .WithErrorMessage("The length of the domain should be in range of [1; 40] characters");
    }
}
