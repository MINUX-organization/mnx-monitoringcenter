using FluentValidation.TestHelper;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Presets;

public static class PresetValidatorTests
{
    private static OverclockingModelValidator? _validator = 
        new OverclockingModelValidator();

    public static void ValidateOverclockongModel_WhenOverclockingModelAreValid(OverclockingInputModel model)
    {
        var result = _validator.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(x => x.CoreClockLock);
        result.ShouldNotHaveValidationErrorFor(x => x.CoreClockOffset);
        result.ShouldNotHaveValidationErrorFor(x => x.CoreVoltage);
        result.ShouldNotHaveValidationErrorFor(x => x.CoreVoltageOffset);
        result.ShouldNotHaveValidationErrorFor(x => x.MemoryClockLock);
        result.ShouldNotHaveValidationErrorFor(x => x.MemoryClockOffset);
        result.ShouldNotHaveValidationErrorFor(x => x.MemoryVoltage);
        result.ShouldNotHaveValidationErrorFor(x => x.MemoryVoltageOffset);
        result.ShouldNotHaveValidationErrorFor(x => x.CriticalTemperature);
        result.ShouldNotHaveValidationErrorFor(x => x.PowerLimit);
        result.ShouldNotHaveValidationErrorFor(x => x.FanSpeed);
    }

    public static void ValidateOverclockingModel_WhenOverclockingModelAreNotValid(OverclockingInputModel model)
    {
        var result = _validator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.CoreClockLock)
            .WithErrorMessage("The value of the core clock lock must not exceed the range [1000; 3000]");

        result.ShouldHaveValidationErrorFor(x => x.CoreClockOffset)
            .WithErrorMessage("The value of the core clock offset must not exceed the range [100; 300]");

        result.ShouldHaveValidationErrorFor(x => x.CoreVoltage)
            .WithErrorMessage("The value of the core voltage must not exceed the range [1000; 2000]");

        result.ShouldHaveValidationErrorFor(x => x.CoreVoltageOffset)
            .WithErrorMessage("The value of the core voltage offset must not exceed the range [-50; 50]");

        result.ShouldHaveValidationErrorFor(x => x.MemoryClockLock)
            .WithErrorMessage("The value of the memory clock lock must not exceed the range [1000; 3000]");

        result.ShouldHaveValidationErrorFor(x => x.MemoryClockOffset)
            .WithErrorMessage("The value of the memory clock offset must not exceed the range [50; 150]");

        result.ShouldHaveValidationErrorFor(x => x.MemoryVoltage)
            .WithErrorMessage("The value of the memory voltage must not exceed the range [900; 1100]");

        result.ShouldHaveValidationErrorFor(x => x.MemoryVoltageOffset)
            .WithErrorMessage("The value of the memory voltage offset must not exceed the range [-50; 50]");

        result.ShouldHaveValidationErrorFor(x => x.CriticalTemperature)
            .WithErrorMessage("The value of the critical temperature must not exceed the range [80; 100]");

        result.ShouldHaveValidationErrorFor(x => x.PowerLimit)
            .WithErrorMessage("The value of the power limit must not exceed the range [100; 350]");

        result.ShouldHaveValidationErrorFor(x => x.FanSpeed)
            .WithErrorMessage("The value of the fan speed must not exceed the range [500; 3000]");
    }
}
