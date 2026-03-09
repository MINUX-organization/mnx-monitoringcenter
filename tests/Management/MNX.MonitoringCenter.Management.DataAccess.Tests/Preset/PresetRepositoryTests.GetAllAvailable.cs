using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;
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
        var query = data.Where(x => x.OwnerId == userId);
        var expectedCount = query.Count();

        await PrepareDataBase(data);


        // Act

        var checkingPresets = await _presetRepository
            .GetAllAvailable(null, specification).ToListAsync();


        // Assert

        Assert.That(checkingPresets, Is.Not.Null);
        Assert.That(checkingPresets, Has.Count.EqualTo(expectedCount));
        AssertPresetsWithNvidiaGpuOverclocking(data, checkingPresets);
    }

    [TestCaseSource(typeof(PresetsTestCaseSource), nameof(PresetsTestCaseSource.PresetsWithNvidiaGpuOverclocking))]
    public async Task GetAllAvailable_ValidUserIdAndGpuName_ReturnsAvailableEntities(List<Preset> data)
    {
        // Arrange

        var userId = PresetsTestCaseSource.UserId;
        var gpuName = PresetsTestCaseSource.NvidiaGpuName;
        var specification = new Specification(userId);
        var query = data.Where(x => x.OwnerId == userId &&
                                    x.DeviceName == gpuName);
        var expectedCount = query.Count();

        await PrepareDataBase(data);


        // Act

        var checkingPresets = await _presetRepository
            .GetAllAvailable(gpuName, specification).ToListAsync();


        // Assert

        Assert.That(checkingPresets, Is.Not.Null);
        Assert.That(checkingPresets, Has.Count.EqualTo(expectedCount));
        AssertPresetsWithNvidiaGpuOverclocking(query.ToList(), checkingPresets);
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

    private static void AssertPresetsWithNvidiaGpuOverclocking(List<Preset> expected, List<Preset> checking)
    {
        expected = expected.OrderBy(x => x.Name).ToList();
        checking = checking.OrderBy(x => x.Name).ToList();

        for (var i = 0; i < expected.Count; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(checking[i].Id, Is.EqualTo(expected[i].Id));
                Assert.That(checking[i].Name, Is.EqualTo(expected[i].Name));
                Assert.That(checking[i].DeviceName, Is.EqualTo(expected[i].DeviceName));
                Assert.That(checking[i].IsVisible, Is.EqualTo(expected[i].IsVisible));
                Assert.That(checking[i].OwnerId, Is.EqualTo(expected[i].OwnerId));
                Assert.That(checking[i].OverclockingId, Is.EqualTo(expected[i].OverclockingId));

                Assert.That(checking[i].Overclocking, Is.Not.Null);
                Assert.That(checking[i].Overclocking, Is.TypeOf<NvidiaGpuOverclocking>());
                var checkingOverclocking = (NvidiaGpuOverclocking)checking[i].Overclocking!;
                var expectedOverclocking = (NvidiaGpuOverclocking)expected[i].Overclocking!;
                AssertOverclockings(expectedOverclocking, checkingOverclocking);
            });
        }

        void AssertOverclockings(NvidiaGpuOverclocking expected, NvidiaGpuOverclocking checking)
        {
            Assert.Multiple(() =>
            {
                Assert.That(checking.Id, Is.EqualTo(expected.Id));
                Assert.That(checking.TargetDeviceType, Is.EqualTo(expected.TargetDeviceType));
                Assert.That(checking.PowerLimit, Is.EqualTo(expected.PowerLimit));
                Assert.That(checking.CoreClockLock, Is.EqualTo(expected.CoreClockLock));
                Assert.That(checking.CoreClockOffset, Is.EqualTo(expected.CoreClockOffset));
                Assert.That(checking.CoreVoltage, Is.EqualTo(expected.CoreVoltage));
                Assert.That(checking.CoreVoltageOffset, Is.EqualTo(expected.CoreVoltageOffset));
                Assert.That(checking.MemoryClockLock, Is.EqualTo(expected.MemoryClockLock));
                Assert.That(checking.MemoryClockOffset, Is.EqualTo(expected.MemoryClockOffset));
                Assert.That(checking.MemoryVoltage, Is.EqualTo(expected.MemoryVoltage));
                Assert.That(checking.MemoryVoltageOffset, Is.EqualTo(expected.MemoryVoltageOffset));

                Assert.That(checking.FanOverclocking, Is.Not.Null);
                Assert.That(checking.FanOverclocking, Is.TypeOf<FanOverclockingWithTargetSpeed>());
                var expectedFanOverclocking = (FanOverclockingWithTargetSpeed)expected.FanOverclocking;
                var checkingFanOverclocking = (FanOverclockingWithTargetSpeed)checking.FanOverclocking;
                AssertFanOverclockings(expectedFanOverclocking, checkingFanOverclocking);
            });

            void AssertFanOverclockings(FanOverclockingWithTargetSpeed expected, FanOverclockingWithTargetSpeed checking)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(checking.Id, Is.EqualTo(expected.Id));
                    Assert.That(checking.Type, Is.EqualTo(expected.Type));
                    Assert.That(checking.TargetSpeed, Is.EqualTo(expected.TargetSpeed));
                });
            }
        }
    }

    private async Task PrepareDataBase(List<Preset> data)
    {
        foreach (var preset in data)
        {
            await _presetRepository.Save(preset);
        }
    }
}
