using MNX.MonitoringCenter.Management.Tests.Service.Assertions;
using MNX.MonitoringCenter.Management.Tests.Service.Assertions.FlightSheet;
using MNX.MonitoringCenter.Management.UseCases;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.MiningDevice;

using MiningDeviceInfo = Core.Mining.MiningDevice.MiningDeviceInfo;

public partial class MiningDeviceRepositoryTests
{
    [TestCaseSource(typeof(MiningDevicesTestCaseSource), nameof(MiningDevicesTestCaseSource.MiningDeviceListsWithVisiblePreset))]
    public async Task GetAvailable_ValidUserId_ReturnsEntitiesList(List<MiningDeviceInfo> data)
    {
        // Arrange

        var specification = new Specification(MiningDevicesTestCaseSource.UserId);
        var query = data.Where(x => x.OwnerId == specification.UserId);
        var expectedCount = query.Count();

        await PrepareDataBase(data);

        foreach (var item in data)
            item.Preset.Overclocking = null;


        // Act

        var checkingDevices = await _miningDeviceRepository
            .GetAvailable(specification).ToListAsync();


        // Assert

        checkingDevices.ShouldBeEqualTo(query);
        checkingDevices.Select(x => x.FlightSheet)
            .ShouldBeEqualTo(query.Select(x => x.FlightSheet));
        checkingDevices.Select(x => x.Preset)
            .ShouldBeEqualTo(query.Select(x => x.Preset));
    }

    [TestCaseSource(typeof(MiningDevicesTestCaseSource), nameof(MiningDevicesTestCaseSource.MiningDeviceListsWithVisiblePreset))]
    public async Task GetAvailable_InvalidUserId_ReturnsEmptyList(List<MiningDeviceInfo> data)
    {
        // Arrange

        var specification = new Specification(Guid.NewGuid());

        await PrepareDataBase(data);


        // Act

        var checkingDevices = await _miningDeviceRepository
            .GetAvailable(specification).ToListAsync();


        // Assert

        Assert.That(checkingDevices, Is.Not.Null);
        Assert.That(checkingDevices, Is.Empty);
    }

    [TestCaseSource(typeof(MiningDevicesTestCaseSource), nameof(MiningDevicesTestCaseSource.GpuDevicesWithUnionVisiblePreset))]
    public async Task GetAvailableByPresetId_ValidPresetIdAndUserId_ReturnsEntitiesList(List<MiningDeviceInfo> data)
    {
        // Arrange

        var presetId = MiningDevicesTestCaseSource.PresetId;
        var userId = MiningDevicesTestCaseSource.UserId;
        var query = data.Where(x => x.OwnerId == userId &&
                                    x.PresetId == presetId);
        var expectedCount = query.Count();

        await PrepareDataBase(data);


        // Act

        var checkingDevices = await _miningDeviceRepository
            .GetAvailableByPresetId(presetId, userId);


        // Assert

        checkingDevices.ShouldBeEqualTo(query);
    }

    [TestCaseSource(typeof(MiningDevicesTestCaseSource), nameof(MiningDevicesTestCaseSource.GpuDevicesWithUnionVisiblePreset))]
    public async Task GetAvailableByPresetId_InvalidPresetIdAndUserId_ReturnsEmptyList(List<MiningDeviceInfo> data)
    {
        // Arrange

        var presetId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        await PrepareDataBase(data);


        // Act

        var checkingDevices = await _miningDeviceRepository
            .GetAvailableByPresetId(presetId, userId);


        // Assert

        Assert.That(checkingDevices, Is.Not.Null);
        Assert.That(checkingDevices, Is.Empty);
    }
}
