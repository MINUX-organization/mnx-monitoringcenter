namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Preset;

using Preset = Core.Overclocking.Preset;

public partial class PresetRepositoryTests
{
    [TestCaseSource(typeof(PresetsTestCaseSource), nameof(PresetsTestCaseSource.Preset))]
    public async Task Exists_ValidUserIdAndName_ReturnTrue(Preset data)
    {
        // Arrange

        var presetName = data.Name;
        var userId = PresetsTestCaseSource.UserId;

        await _presetRepository.Save(data);

        
        // Act

        var isExists = await _presetRepository
            .Exists(userId, presetName, default);


        // Assert

        Assert.That(isExists, Is.True);
    }

    [TestCaseSource(typeof(PresetsTestCaseSource), nameof(PresetsTestCaseSource.Preset))]
    public async Task Exists_InvalidUserIdAndName_ReturnsFalse(Preset data)
    {
        // Arrange

        var presetName = "SomeWrongPresetName";
        var userId = Guid.NewGuid();

        await _presetRepository.Save(data);


        // Act

        var isExists = await _presetRepository
            .Exists(userId, presetName, default);


        // Assert

        Assert.That(isExists, Is.False);
    }
}
