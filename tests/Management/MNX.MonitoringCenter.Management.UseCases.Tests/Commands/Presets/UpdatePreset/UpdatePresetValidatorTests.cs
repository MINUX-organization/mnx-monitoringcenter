using MNX.MonitoringCenter.Management.UseCases.Commands.Presets;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.UpdatePreset;
using FluentValidation.TestHelper;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Presets.UpdatePreset;

[TestFixture]
public class UpdatePresetValidatorTests
{
    private UpdatePresetValidator _validator;

    [SetUp]
    public void Setup()
    {
        _validator = new UpdatePresetValidator();
    }

    [Test]
    public void SavePresetCommand_WhenModelNotNull_ShouldNotErrors()
    {
        var command = GetCommand(CreatePresetModel(false));
        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.SavePresetModel);
    }

    [Test]
    public void SavePresetCommand_WhenModelNull_ShouldErrors()
    {
        var command = GetCommand(CreatePresetModel(true));
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.SavePresetModel.Overclocking)
              .WithErrorMessage("Preset data is required");
    }

    [TestCaseSource(typeof(PresetCommandTestCase),
                    nameof(PresetCommandTestCase.CreateCorrectOverclockingModel))]
    public void UpdatePresetCommand_WhenPresetModelAreValid_ShouldNotErrors(OverclockingInputModel model)
    {
        var command = new UpdatePresetCommand(Guid.NewGuid(), new SavePresetInputModel("Test", "Test", model), 
                                              TestHelper.UserId);

        PresetValidatorTests.
            ValidateOverclockongModel_WhenOverclockingModelAreValid(command.SavePresetModel.Overclocking);
    }

    [TestCaseSource(typeof(PresetCommandTestCase),
                    nameof(PresetCommandTestCase.CreateIncorrectOverclockingModel))]
    public void UpdatePresetCommand_WhenPresetModelAreNotValid_ShouldErrors(OverclockingInputModel model)
    {
        var command = new UpdatePresetCommand(Guid.NewGuid(), new SavePresetInputModel("Test", "Test", model), 
                                              TestHelper.UserId);

        PresetValidatorTests.
            ValidateOverclockingModel_WhenOverclockingModelAreNotValid(command.SavePresetModel.Overclocking);
    }

    private static UpdatePresetCommand GetCommand(SavePresetInputModel presetModel)
    {
        Guid id = Guid.Parse("4d0b4812-6d2e-4d38-85c5-ac2c7871e000");
        
        return new UpdatePresetCommand(id, presetModel, 
                                      TestHelper.UserId);
    }

    private static SavePresetInputModel CreatePresetModel(bool overclockingIsNull)
    {
        if (overclockingIsNull)
        {
            return new SavePresetInputModel("Test", "Test", null!);
        }
        else
        {
            return new SavePresetInputModel("Test", "Test",
                   new OverclockingInputModel(2000, 200, 1500, 0, 2000, 100, 1000, 0, 90, 250, 2000));
        }       
    }
}
