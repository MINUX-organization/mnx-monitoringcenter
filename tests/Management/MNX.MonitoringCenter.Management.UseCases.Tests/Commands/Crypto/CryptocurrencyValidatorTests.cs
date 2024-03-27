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

        result.ShouldNotHaveValidationErrorFor(x => x.ShortName);
        result.ShouldNotHaveValidationErrorFor(x => x.FullName);
    }

    public static void ValidateCryptocurrencyModel_WhenCryptocurrencyModelAreNotValid(CryptocurrencyInputModel model)
    {
        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x);

        result.ShouldHaveValidationErrorFor(x => x.ShortName)
            .WithErrorMessage("Длина короткого имени не должна выходить за диапазон [2; 10] символов");

        result.ShouldHaveValidationErrorFor(x => x.FullName)
            .WithErrorMessage("Длина полного имени не должна выходить за диапазон [2, 40] символов");
    }
}
