using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.Tests.Service.Assertions;
using MNX.MonitoringCenter.Management.Tests.Service.Assertions.Overclocking;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.MiningDevice;

public partial class MiningDeviceRepositoryTests
{
    [TestCaseSource(typeof(MiningDevicesTestCaseSource), nameof(MiningDevicesTestCaseSource.MiningDevices))]
    public async Task GetById_ValidId_ReturnsEntity(MiningDeviceInfo data)
    {
        // Arrange

        var deviceId = data.Id;

        await PrepareDataBase(data);


        // Act

        var checkingDevice = await _miningDeviceRepository
            .GetById(deviceId, default);


        // Assert

        checkingDevice.ShouldBeEqualTo(data);
    }

    [TestCaseSource(typeof(MiningDevicesTestCaseSource), nameof(MiningDevicesTestCaseSource.MiningDevices))]
    public async Task GetById_InvalidId_ReturnsNull(MiningDeviceInfo data)
    {
        // Arrange

        var deviceId = Guid.NewGuid();

        await PrepareDataBase(data);


        // Act

        var exception = Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _miningDeviceRepository .GetById(deviceId, default));


        // Assert

        Assert.That(exception, Is.Not.Null);
    }

    [TestCaseSource(typeof(MiningDevicesTestCaseSource), nameof(MiningDevicesTestCaseSource.MiningDevicesWithOnlineStatus))]
    public async Task GetActiveDeviceById_ValidIdAndUserId_ReturnsEntity(MiningDeviceInfo data)
    {
        // Arrange

        var deviceId = data.Id;
        var userId = MiningDevicesTestCaseSource.UserId;

        await PrepareDataBase(data);


        // Act

        var checkingDevice = await _miningDeviceRepository
            .GetActiveDeviceById(deviceId, userId, default);


        // Assert

        checkingDevice.ShouldBeEqualTo(data);
    }

    [TestCaseSource(typeof(MiningDevicesTestCaseSource), nameof(MiningDevicesTestCaseSource.MiningDevicesWithOnlineStatus))]
    public async Task GetActiveDeviceById_InvalidIdAndUserId_ReturnsNull(MiningDeviceInfo data)
    {
        // Arrange

        var deviceId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        await PrepareDataBase(data);


        // Act

        var checkingDevice = await _miningDeviceRepository
            .GetActiveDeviceById(deviceId, userId, default);


        // Assert

        Assert.That(checkingDevice, Is.Null);
    }

    [TestCaseSource(typeof(MiningDevicesTestCaseSource), nameof(MiningDevicesTestCaseSource.MiningDevicesWithOverclockings))]
    public async Task GetOverclocking_ValidDeviceIdAndUserId_ReturnsEntity(MiningDeviceInfo data)
    {
        // Arrange

        var deviceId = data.Id;
        var userId = MiningDevicesTestCaseSource.UserId;
        var overclocking = data.Preset!.Overclocking;

        await PrepareDataBase(data);


        // Act

        var checkingOverclocking = await _miningDeviceRepository
            .GetOverclocking(deviceId, userId);


        // Assert

        Assert.That(checkingOverclocking, Is.Not.Null);
        checkingOverclocking.ShouldBeEquivalentTo(overclocking);
    }

    [TestCaseSource(typeof(MiningDevicesTestCaseSource), nameof(MiningDevicesTestCaseSource.MiningDevicesWithOverclockings))]
    public async Task GetOverclocking_InvalidDeviceIdAndUserId_ReturnsNull(MiningDeviceInfo data)
    {
        // Arrange

        var deviceId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        await PrepareDataBase(data);


        // Act

        var checkingOverclocking = await _miningDeviceRepository
            .GetOverclocking(deviceId, userId);


        // Assert

        Assert.That(checkingOverclocking, Is.Null);
    }
}
