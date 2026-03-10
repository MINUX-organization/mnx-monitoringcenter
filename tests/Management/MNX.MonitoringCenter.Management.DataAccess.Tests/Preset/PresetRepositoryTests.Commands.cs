using Microsoft.EntityFrameworkCore;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Preset;

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
        AssertPreset(data, checkingPreset);
    }

    [TestCaseSource(typeof(PresetsTestCaseSource), nameof(PresetsTestCaseSource.Preset))]
    public async Task Save_ExistingPreset_ShouldThrowAnDbUpdateException(Preset data)
    {
        var presetId = data.Id;
        var userId = PresetsTestCaseSource.UserId;

        await _presetRepository.Save(data);
        Context.ChangeTracker.Clear();


        // Act

        var exception = Assert.ThrowsAsync<DbUpdateException>(async () =>
            await _presetRepository.Save(data));


        // Assert

        Assert.That(exception, Is.Not.Null);
    }

    [TestCaseSource(typeof(PresetsTestCaseSource), nameof(PresetsTestCaseSource.Preset))]
    public async Task Update_ValidPreset_ShouldUpdateEntity(Preset data)
    {
        // Arrange

        var presetId = data.Id;
        var userId = PresetsTestCaseSource.UserId;

        await _presetRepository.Save(data);
        Context.ChangeTracker.Clear();


        // Act

        data.Name = "NewPresetName";
        data.IsVisible = false;

        await _presetRepository.Update(data);
        var checkingPreset = await _presetRepository.GetById(presetId, userId, default);


        // Assert

        Assert.That(checkingPreset, Is.Not.Null);
        AssertPreset(data, checkingPreset);
    }

    [TestCaseSource(typeof(PresetsTestCaseSource), nameof(PresetsTestCaseSource.Preset))]
    public void Update_NonexistentPreset_ShouldThrowAnPostgresException(Preset data)
    {
        // Arrange

        var presetId = data.Id;
        var userId = PresetsTestCaseSource.UserId;


        // Act

        var exception = Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
            await _presetRepository.Update(data));


        // Assert

        Assert.That(exception, Is.Not.Null);
    }

    [TestCaseSource(typeof(PresetsTestCaseSource), nameof(PresetsTestCaseSource.Preset))]
    public async Task Remove_ValidPreset_ShouldDeleteEntity(Preset data)
    {
        // Arrange

        var presetId = data.Id;
        var userId = PresetsTestCaseSource.UserId;

        await _presetRepository.Save(data);
        Context.ChangeTracker.Clear();


        // Act

        await _presetRepository.Remove(presetId, userId);
        var checkingPreset = await _presetRepository
            .GetById(presetId, userId, default);
    
        
        // Assert

        Assert.That(checkingPreset, Is.Null);
    }
}
