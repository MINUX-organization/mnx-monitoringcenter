using FluentValidation.TestHelper;
using MNX.MonitoringCenter.Management.UseCases.Commands.Crypto;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Crypto;

public static class CryptocurrencyValidatorTests
{
    private static CryptocurrencyModelValidator? _validator =
        new CryptocurrencyModelValidator();

    public static void ValidateCryptocurrencyModel_WhenCryptocurrencyModelAreValid(CryptocurrencyInputModel model)
    {
        var result = _validator.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(x => x.ShortName.Length);
        result.ShouldNotHaveValidationErrorFor(x => x.FullName.Length);
    }

    public static void ValidateCryptocurrencyModel_WhenCryptocurrencyModelAreNotValid(CryptocurrencyInputModel model)
    {
        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.ShortName.Length)
            .WithErrorMessage("The length of the cryptocurrency short name should be in the range of [2; 10] characters");

        result.ShouldHaveValidationErrorFor(x => x.FullName.Length)
            .WithErrorMessage("The length of the cryptocurrency name should be in the range of [2; 40] characters");
    }
}
