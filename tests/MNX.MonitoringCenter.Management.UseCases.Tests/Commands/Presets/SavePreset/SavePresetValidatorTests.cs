using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;
using FluentValidation.TestHelper;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Presets.SavePreset;

[TestFixture]
public class SavePresetValidatorTests
{
    private SavePresetValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new SavePresetValidator();
    }

    [Test]
    public void SavePresetCommand_WhenGpuNameNotEmpty_ShouldNotErrors()
    {
        var command = GetCommand("GeForce RTX 4090");
        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.GpuName);
    }

    [Test]
    public void SavePresetCommand_WhenGpuNameEmpty_ShouldErrors()
    {
        var command = GetCommand(string.Empty);
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.GpuName)
            .WithErrorMessage("Название GPU не должно быть пустым");
    }

    [Test]
    public void SavePresetCommand_WhenModelNotNull_ShouldNotErrors()
    {
        var command = GetCommand("GeForce RTX 4090");
        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Model);
    }

    [Test]
    public void SavePresetCommand_WhenModelNull_ShouldErrors()
    {
        var command = new SavePresetCommand("GeForce RTX 4090", null!);
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Model)
              .WithErrorMessage("Данные для пресета обязательны");
    }

    [TestCaseSource(typeof(PresetCommandTestCase),
                    nameof(PresetCommandTestCase.CreateCorrectPresetModel))]
    public void SavePresetCommand_WhenPresetModelAreValid_ShouldNotErrors(PresetModel model)
    {
        var command = new SavePresetCommand("GeForce RTX 4090", model);
        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Model.CoreClock);

        result.ShouldNotHaveValidationErrorFor(x => x.Model.MemoryClock);

        result.ShouldNotHaveValidationErrorFor(x => x.Model.PowerLimit);

        result.ShouldNotHaveValidationErrorFor(x => x.Model.CriticalTemperature);

        result.ShouldNotHaveValidationErrorFor(x => x.Model.FanSpeed);
    }

    [TestCaseSource(typeof(PresetCommandTestCase),
                    nameof(PresetCommandTestCase.CreateIncorrectPresetModel))]
    public void SavePresetCommand_WhenPresetModelAreNotValid_ShouldErrors(PresetModel model)
    {
        var command = new SavePresetCommand("GeForce RTX 4090", model);
        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.Model);

        result.ShouldHaveValidationErrorFor(x => x.Model.CoreClock)
              .WithErrorMessage("Значение тактовой частоты ядра" +
                    " не должно выходить за диапазон [1000; 5000] Мгц");

        result.ShouldHaveValidationErrorFor(x => x.Model.MemoryClock)
              .WithErrorMessage("Значение тактовой частоты памяти" +
                    " не должно выходить за диапазон [1000; 5000] Мгц");

        result.ShouldHaveValidationErrorFor(x => x.Model.PowerLimit)
              .WithErrorMessage("Значение ограничения мощности не" +
                    " должно выходить за диапазон [100; 150] Ватт");

        result.ShouldHaveValidationErrorFor(x => x.Model.CriticalTemperature)
              .WithErrorMessage("Значение критической температуры" +
                    " не должно выходить за диапазон [0; 110] гадусов" +
                    " Цельсия");

        result.ShouldHaveValidationErrorFor(x => x.Model.FanSpeed)
              .WithErrorMessage("Значение скорости вентилятора не" +
                    " должно выходить за диапазон [0; 100] %");
    }

    private static SavePresetCommand GetCommand(string name)
    {
        var presetModel = CreatePresetModel();

        return new SavePresetCommand(name, presetModel);
    }

    private static PresetModel CreatePresetModel()
    {
        return new PresetModel(memoryClock: 1313,
                               coreClock: 2235,
                               powerLimit: 150,
                               criticalTemperature: 105,
                               fanSpeed: 99);
    }
}
