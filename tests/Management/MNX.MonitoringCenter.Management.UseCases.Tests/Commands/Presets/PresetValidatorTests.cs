using FluentValidation.TestHelper;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Presets;

public static class PresetValidatorTests
{
    private static PresetModelValidator? _validator = 
        new PresetModelValidator();

    public static void ValidatePresetModel_WhenPresetModelAreValid(PresetInputModel model)
    {
        var result = _validator.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(x => x.CoreClock);
        result.ShouldNotHaveValidationErrorFor(x => x.MemoryClock);
        result.ShouldNotHaveValidationErrorFor(x => x.PowerLimit);
        result.ShouldNotHaveValidationErrorFor(x => x.CriticalTemperature);
        result.ShouldNotHaveValidationErrorFor(x => x.FanSpeed);
    }

    public static void ValidatePresetModel_WhenPresetModelAreNotValid(PresetInputModel model)
    {
        var result = _validator.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(x => x);

        result.ShouldHaveValidationErrorFor(x => x.CoreClock)
              .WithErrorMessage("The value of the core clock frequency must not exceed the range [1000; 5000] MHz");

        result.ShouldHaveValidationErrorFor(x => x.MemoryClock)
              .WithErrorMessage("The memory clock frequency value must not exceed the range [1000; 5000] MHz");

        result.ShouldHaveValidationErrorFor(x => x.PowerLimit)
              .WithErrorMessage("The power limitation value must not exceed the range [100; 150] Watts");

        result.ShouldHaveValidationErrorFor(x => x.CriticalTemperature)
              .WithErrorMessage("The value of the critical temperature must not exceed the range [0; 110] gradus Celsius");

        result.ShouldHaveValidationErrorFor(x => x.FanSpeed)
              .WithErrorMessage("The fan speed value must not exceed the range [0; 100] %");
    }
}
