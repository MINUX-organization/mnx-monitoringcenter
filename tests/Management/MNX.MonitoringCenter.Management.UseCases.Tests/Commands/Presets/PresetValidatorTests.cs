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
              .WithErrorMessage("Значение тактовой частоты ядра" +
                    " не должно выходить за диапазон [1000; 5000] Мгц");

        result.ShouldHaveValidationErrorFor(x => x.MemoryClock)
              .WithErrorMessage("Значение тактовой частоты памяти" +
                    " не должно выходить за диапазон [1000; 5000] Мгц");

        result.ShouldHaveValidationErrorFor(x => x.PowerLimit)
              .WithErrorMessage("Значение ограничения мощности не" +
                    " должно выходить за диапазон [100; 150] Ватт");

        result.ShouldHaveValidationErrorFor(x => x.CriticalTemperature)
              .WithErrorMessage("Значение критической температуры" +
                    " не должно выходить за диапазон [0; 110] гадусов" +
                    " Цельсия");

        result.ShouldHaveValidationErrorFor(x => x.FanSpeed)
              .WithErrorMessage("Значение скорости вентилятора не" +
                    " должно выходить за диапазон [0; 100] %");
    }
}
