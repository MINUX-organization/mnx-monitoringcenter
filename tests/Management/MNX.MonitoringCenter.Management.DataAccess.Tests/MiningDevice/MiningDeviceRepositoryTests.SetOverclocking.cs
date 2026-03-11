using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.Overclockings;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.Overclockings.Fans;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.MiningDevice;

public partial class MiningDeviceRepositoryTests
{
    [TestCaseSource(typeof(MiningDevicesTestCaseSource), nameof(MiningDevicesTestCaseSource.MiningDevicesWithOverclockings))]
    public async Task SetOverclocking_ValidOverclockingAndDevice_ShouldSetPreset(MiningDeviceInfo data)
    {
        // Arrange

        var deviceId = data.Id;
        var overclocking = new NvidiaGpuOverclockingBuilder()
            .WithFanOverclocking(() => new FanOverclockingWithTargetSpeedBuilder().Build())
            .Build();

        await PrepareDataBase(data);


        // Act

        await _miningDeviceRepository
            .SetOverclocking(data, overclocking, default);
        var checkingDevice = await _miningDeviceRepository
            .GetById(deviceId, default);


        // Assert

        Assert.That(checkingDevice, Is.Not.Null);
        Assert.That(checkingDevice.Preset.OverclockingId, Is.EqualTo(overclocking.Id));
    }
}
