using MNX.MonitoringCenter.Management.Tests.Service.Assertions;
using MNX.MonitoringCenter.Management.UseCases;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Preset;

using Preset = Core.Overclocking.Preset;

public partial class PresetRepositoryTests
{
    [TestCaseSource(typeof(PresetsTestCaseSource), nameof(PresetsTestCaseSource.PresetsSeparatedByDeviceNameGroups))]
    public async Task GetGroupedList_ValidUserId_ReturnsListOfGroups(List<Preset> data)
    {
        // Arrange

        var userId = PresetsTestCaseSource.UserId;
        var specification = new Specification(userId);
        var query = data.GroupBy(x => x.DeviceName);
        var expectedCount = query.Count();

        await PrepareDataBase(data);


        // Act

        var checkingPresetGroups = await _presetRepository
            .GetGroupedList(x => x.DeviceName, specification).ToListAsync();


        // Assert

        Assert.That(checkingPresetGroups, Is.Not.Null);
        Assert.That(checkingPresetGroups, Has.Count.EqualTo(expectedCount));

        var checkingPresets = checkingPresetGroups.SelectMany(x => x).ToList();
        checkingPresets.ShouldBeEqualTo(data);
    }
}
