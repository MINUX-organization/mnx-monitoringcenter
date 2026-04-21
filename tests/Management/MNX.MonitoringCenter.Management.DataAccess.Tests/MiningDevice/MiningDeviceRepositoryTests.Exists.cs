using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.MiningDevice;

public partial class MiningDeviceRepositoryTests
{
    [TestCaseSource(typeof(MiningDevicesTestCaseSource), nameof(MiningDevicesTestCaseSource.MiningDevices))]
    public async Task Exists_ValidNameAndUserId_ReturnsTrue(MiningDeviceInfo data)
    {
        // Arrange

        var deviceName = data.Name;
        var userId = MiningDevicesTestCaseSource.UserId;

        await PrepareDataBase(data);


        // Act

        var isExists = await _miningDeviceRepository
            .Exists(deviceName, userId, default);


        // Assert

        Assert.That(isExists, Is.True);
    }

    [TestCaseSource(typeof(MiningDevicesTestCaseSource), nameof(MiningDevicesTestCaseSource.MiningDevices))]
    public async Task Exists_InvalidNameAndUserId_ReturnsFalse(MiningDeviceInfo data)
    {
        // Arrange

        var deviceName = "SomeWrongDeviceName";
        var userId = Guid.NewGuid();

        await PrepareDataBase(data);


        // Act

        var isExists = await _miningDeviceRepository
            .Exists(deviceName, userId, default);


        // Assert

        Assert.That(isExists, Is.False);
    }

    [TestCaseSource(typeof(MiningDevicesTestCaseSource), nameof(MiningDevicesTestCaseSource.InactiveMiningDevices))]
    public async Task Exists_TryCheckExistendOfInactiveDevice_ReturnsFalse(MiningDeviceInfo data)
    {
        // Arrange

        var deviceName = data.Name;
        var userId = MiningDevicesTestCaseSource.UserId;

        await PrepareDataBase(data);


        // Act

        var isExists = await _miningDeviceRepository
            .Exists(deviceName, userId, default);


        // Assert

        Assert.That(isExists, Is.False);
    }
}
