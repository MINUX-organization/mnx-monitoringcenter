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


        // Act

        var checkingDevices = await _miningDeviceRepository
            .GetAvailable(specification).ToListAsync();


        // Assert

        Assert.That(checkingDevices, Is.Not.Null);
        Assert.That(checkingDevices, Has.Count.EqualTo(expectedCount));
        AssertDevices([.. query], checkingDevices);
        AssertFlightSheets([.. query.Select(x => x.FlightSheet)],
                           [..checkingDevices.Select(x => x.FlightSheet)]);
        AssertPresets([.. query.Select(x => x.Preset)],
                      [.. checkingDevices.Select(x => x.Preset)]);
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

        Assert.That(checkingDevices, Is.Not.Null);
        Assert.That(checkingDevices, Has.Count.EqualTo(expectedCount));
        AssertDevices([.. query], checkingDevices);
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
