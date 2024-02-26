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

        PresetValidatorTests.
            ValidatePresetModel_WhenPresetModelAreValid(command.Model);
    }

    [TestCaseSource(typeof(PresetCommandTestCase),
                    nameof(PresetCommandTestCase.CreateIncorrectPresetModel))]
    public void SavePresetCommand_WhenPresetModelAreNotValid_ShouldErrors(PresetModel model)
    {
        var command = new SavePresetCommand("GeForce RTX 4090", model);

        PresetValidatorTests.
            ValidatePresetModel_WhenPresetModelAreNotValid(command.Model);
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
