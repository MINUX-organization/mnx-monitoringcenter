using MNX.MonitoringCenter.Management.Tests.Service.Assertions;
using MNX.MonitoringCenter.Management.UseCases;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Preset;

using Preset = Core.Overclocking.Preset;

public partial class PresetRepositoryTests
{
    [TestCaseSource(typeof(PresetsTestCaseSource), nameof(PresetsTestCaseSource.PresetsWithNvidiaGpuOverclocking))]
    public async Task GetAllAvailable_ValidUserId_ReturnsAvailableEntities(List<Preset> data)
    {
        // Arrange

        var userId = PresetsTestCaseSource.UserId;
        var specification = new Specification(userId);
        var query = data.Where(x => x.OwnerId == userId &&
                                    x.IsVisible == true);
        var expectedCount = query.Count();

        await PrepareDataBase(data);


        // Act

        var checkingPresets = await _presetRepository
            .GetAllAvailable(null, specification).ToListAsync();


        // Assert

        Assert.That(checkingPresets, Is.Not.Null);
        Assert.That(checkingPresets, Has.Count.EqualTo(expectedCount));
        checkingPresets.ShouldBeEqualTo(query);
    }

    [TestCaseSource(typeof(PresetsTestCaseSource), nameof(PresetsTestCaseSource.PresetsWithNvidiaGpuOverclocking))]
    public async Task GetAllAvailable_ValidUserIdAndGpuName_ReturnsAvailableEntities(List<Preset> data)
    {
        // Arrange

        var userId = PresetsTestCaseSource.UserId;
        var gpuName = PresetsTestCaseSource.NvidiaGpuName;
        var specification = new Specification(userId);
        var query = data.Where(x => x.OwnerId == userId &&
                                    x.DeviceName == gpuName &&
                                    x.IsVisible == true);
        var expectedCount = query.Count();

        await PrepareDataBase(data);


        // Act

        var checkingPresets = await _presetRepository
            .GetAllAvailable(gpuName, specification).ToListAsync();


        // Assert

        Assert.That(checkingPresets, Is.Not.Null);
        Assert.That(checkingPresets, Has.Count.EqualTo(expectedCount));
        checkingPresets.ShouldBeEqualTo(query);
    }

    [TestCaseSource(typeof(PresetsTestCaseSource), nameof(PresetsTestCaseSource.PresetsWithNvidiaGpuOverclocking))]
    public async Task GetAllAvailable_InvalidUserId_ReturnsEmptyList(List<Preset> data)
    {
        // Arrange

        var specification = new Specification(Guid.NewGuid());

        await PrepareDataBase(data);


        // Act

        var checkingPresets = await _presetRepository
            .GetAllAvailable(null, specification).ToListAsync();


        // Assert

        Assert.That(checkingPresets, Is.Not.Null);
        Assert.That(checkingPresets, Is.Empty);
    }
}
