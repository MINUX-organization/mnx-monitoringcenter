namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Preset;

using MNX.MonitoringCenter.Management.Tests.Service.Assertions;
using Preset = Core.Overclocking.Preset;

public partial class PresetRepositoryTests
{
    [TestCaseSource(typeof(PresetsTestCaseSource), nameof(PresetsTestCaseSource.Preset))]
    public async Task Save_ValidPreset_ShouldAddEntity(Preset data)
    {
        // Arrange

        var presetId = data.Id;
        var userId = PresetsTestCaseSource.UserId;


        // Act

        await _presetRepository.Save(data);
        var checkingPreset = await _presetRepository
            .GetById(presetId, userId, default);


        // Assert

        Assert.That(checkingPreset, Is.Not.Null);
        checkingPreset.ShouldBeEqualTo(data);
    }

    [TestCaseSource(typeof(PresetsTestCaseSource), nameof(PresetsTestCaseSource.Preset))]
    public async Task Update_ValidPreset_ShouldUpdateEntity(Preset data)
    {
        // Arrange

        var presetId = data.Id;
        var userId = PresetsTestCaseSource.UserId;

        await _presetRepository.Save(data);
        ClearChangeTracker();


        // Act

        data.Name = "NewPresetName";
        data.IsVisible = false;

        await _presetRepository.Update(data);
        var checkingPreset = await _presetRepository.GetById(presetId, userId, default);


        // Assert

        Assert.That(checkingPreset, Is.Not.Null);
        checkingPreset.ShouldBeEqualTo(data);
    }

    [TestCaseSource(typeof(PresetsTestCaseSource), nameof(PresetsTestCaseSource.Preset))]
    public async Task Remove_ValidPreset_ShouldDeleteEntity(Preset data)
    {
        // Arrange

        var presetId = data.Id;
        var userId = PresetsTestCaseSource.UserId;

        await _presetRepository.Save(data);
        ClearChangeTracker();


        // Act

        await _presetRepository.Remove(presetId, userId);
        var checkingPreset = await _presetRepository
            .GetById(presetId, userId, default);
    
        
        // Assert

        Assert.That(checkingPreset, Is.Null);
    }
}
