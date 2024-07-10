using FluentValidation.TestHelper;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;

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
            .WithErrorMessage("The GPU name must not be empty");
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
        var savePresetInputModel = new SavePresetInputModel(string.Empty, null!, null!);
        var command = new SavePresetCommand(TestHelper.UserId, savePresetInputModel);
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.SavePresetModel.Overclocking)
              .WithErrorMessage("Preset data is required");
    }

    [TestCaseSource(typeof(PresetCommandTestCase),
                    nameof(PresetCommandTestCase.CreateCorrectOverclockingModel))]
    public void SavePresetCommand_WhenPresetModelAreValid_ShouldNotErrors(OverclockingInputModel model)
    {      
        PresetValidatorTests.
            ValidateOverclockongModel_WhenOverclockingModelAreValid(model);
    }

    [TestCaseSource(typeof(PresetCommandTestCase),
                    nameof(PresetCommandTestCase.CreateIncorrectOverclockingModel))]
    public void SavePresetCommand_WhenPresetModelAreNotValid_ShouldErrors(OverclockingInputModel model)
    {
        PresetValidatorTests.
            ValidateOverclockingModel_WhenOverclockingModelAreNotValid(model);
    }

    private static SavePresetCommand GetCommand(string gpuName)
    {
        var presetModel = new SavePresetInputModel("example", gpuName , null!);

        return new SavePresetCommand(TestHelper.UserId, presetModel);
    }
}
