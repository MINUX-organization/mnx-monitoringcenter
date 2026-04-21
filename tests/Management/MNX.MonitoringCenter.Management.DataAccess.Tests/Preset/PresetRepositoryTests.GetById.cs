namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Preset;

using MNX.MonitoringCenter.Management.Tests.Service.Assertions;
using Preset = Core.Overclocking.Preset;

public partial class PresetRepositoryTests
{
    [TestCaseSource(typeof(PresetsTestCaseSource), nameof(PresetsTestCaseSource.Preset))]
    public async Task GetById_ValidIdAndUserId_ReturnsEntity(Preset data)
    {
        // Arrange

        var presetId = data.Id;
        var userId = PresetsTestCaseSource.UserId;

        await _presetRepository.Save(data);


        // Act

        var checkingPreset = await _presetRepository
            .GetById(presetId, userId, default);
        

        // Assert

        Assert.That(checkingPreset, Is.Not.Null);
        checkingPreset.ShouldBeEqualTo(data);
    }

    [TestCaseSource(typeof(PresetsTestCaseSource), nameof(PresetsTestCaseSource.Preset))]
    public async Task GetById_InvalidIdAndUserId_ReturnsNull(Preset data)
    {
        // Arrange

        var presetId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        await _presetRepository.Save(data);


        // Act

        var checkingPreset = await _presetRepository
            .GetById(presetId, userId, default);


        // Assert

        Assert.That(checkingPreset, Is.Null);
    }
}
