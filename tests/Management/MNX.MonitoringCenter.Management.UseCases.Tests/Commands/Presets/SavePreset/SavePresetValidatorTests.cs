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

        result.ShouldNotHaveValidationErrorFor(x => x.SavePresetModel.GpuName);
    }

    [Test]
    public void SavePresetCommand_WhenGpuNameEmpty_ShouldErrors()
    {
        var command = GetCommand(string.Empty);
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.SavePresetModel.GpuName)
            .WithErrorMessage("Название GPU не должно быть пустым");
    }

    [Test]
    public void SavePresetCommand_WhenModelNotNull_ShouldNotErrors()
    {
        var command = GetCommand("GeForce RTX 4090");
        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.SavePresetModel);
    }

    [Test]
    public void SavePresetCommand_WhenModelNull_ShouldErrors()
    {
        var savePresetInputModel = new SavePresetInputModel(string.Empty, null!);
        var command = new SavePresetCommand(TestHelper.UserId, savePresetInputModel);
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.SavePresetModel.PresetModel)
              .WithErrorMessage("Данные для пресета обязательны");
    }

    [TestCaseSource(typeof(PresetCommandTestCase),
                    nameof(PresetCommandTestCase.CreateCorrectSavePresetModel))]
    public void SavePresetCommand_WhenPresetModelAreValid_ShouldNotErrors(SavePresetInputModel model)
    {
        var command = new SavePresetCommand(TestHelper.UserId, model);

        PresetValidatorTests.
            ValidatePresetModel_WhenPresetModelAreValid(command.SavePresetModel.PresetModel);
    }

    [TestCaseSource(typeof(PresetCommandTestCase),
                    nameof(PresetCommandTestCase.CreateIncorrectSavePresetModel))]
    public void SavePresetCommand_WhenPresetModelAreNotValid_ShouldErrors(SavePresetInputModel model)
    {
        var command = new SavePresetCommand(TestHelper.UserId, model);

        PresetValidatorTests.
            ValidatePresetModel_WhenPresetModelAreNotValid(command.SavePresetModel.PresetModel);
    }

    private static SavePresetCommand GetCommand(string name)
    {
        var presetModel = CreateSavePresetInputModel(name);

        return new SavePresetCommand(TestHelper.UserId, presetModel);
    }

    private static SavePresetInputModel CreateSavePresetInputModel(string name)
    {
        var presetInputModel = new PresetInputModel(memoryClock: 1313,
                               coreClock: 2235,
                               powerLimit: 150,
                               criticalTemperature: 105,
                               fanSpeed: 99);

        return new SavePresetInputModel(name, presetInputModel);
    }
}
