using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking.Enums;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Preset;

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
        AssertPreset(data, checkingPreset);
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

    private void AssertPreset(Preset expected, Preset checking)
    {
        Assert.Multiple(() =>
        {
            Assert.That(checking.Id, Is.EqualTo(expected.Id));
            Assert.That(checking.Name, Is.EqualTo(expected.Name));
            Assert.That(checking.DeviceName, Is.EqualTo(expected.DeviceName));
            Assert.That(checking.IsVisible, Is.EqualTo(expected.IsVisible));
            Assert.That(checking.OwnerId, Is.EqualTo(expected.OwnerId));
            Assert.That(checking.OverclockingId, Is.EqualTo(expected.OverclockingId));

            Assert.That(checking.Overclocking, Is.Not.Null);
            switch (checking.Overclocking!.TargetDeviceType)
            {
                case OverclockingTargetDeviceType.NvidiaGPU:
                {
                    AssertNvidiaGpuOverclocking(
                        (NvidiaGpuOverclocking)expected.Overclocking!,
                        (NvidiaGpuOverclocking)checking.Overclocking);
                    break;
                }
                case OverclockingTargetDeviceType.AmdGPU:
                {
                    AssertAmdGpuOverclocking(
                        (AmdGpuOverclocking)expected.Overclocking!,
                        (AmdGpuOverclocking)checking.Overclocking);
                    break;
                }
                case OverclockingTargetDeviceType.IntelGPU:
                {
                    AssertIntelGpuOverclocking(
                        (IntelGpuOverclocking)expected.Overclocking!,
                        (IntelGpuOverclocking)checking.Overclocking);
                    break;
                }
                case OverclockingTargetDeviceType.CPU:
                {
                    throw new NotImplementedException(
                        "Use of overclocking in presets doesn't implemented yet");
                }
            }
        });

        void AssertNvidiaGpuOverclocking(NvidiaGpuOverclocking expected, NvidiaGpuOverclocking checking)
        {
            Assert.Multiple(() =>
            {
                Assert.That(checking.Id, Is.EqualTo(expected.Id));
                Assert.That(checking.PowerLimit, Is.EqualTo(expected.PowerLimit));
                Assert.That(checking.CoreClockLock, Is.EqualTo(expected.CoreClockLock));
                Assert.That(checking.CoreClockOffset, Is.EqualTo(expected.CoreClockOffset));
                Assert.That(checking.CoreVoltage, Is.EqualTo(expected.CoreVoltage));
                Assert.That(checking.CoreVoltageOffset, Is.EqualTo(expected.CoreVoltageOffset));
                Assert.That(checking.MemoryClockLock, Is.EqualTo(expected.MemoryClockLock));
                Assert.That(checking.MemoryClockOffset, Is.EqualTo(expected.MemoryClockOffset));
                Assert.That(checking.MemoryVoltage, Is.EqualTo(expected.MemoryVoltage));
                Assert.That(checking.MemoryVoltageOffset, Is.EqualTo(expected.MemoryVoltageOffset));
                AssertFanOverclocking(expected.FanOverclocking, checking.FanOverclocking);
            });
        }

        void AssertAmdGpuOverclocking(AmdGpuOverclocking expected, AmdGpuOverclocking checking)
        {
            Assert.Multiple(() =>
            {
                Assert.That(checking.Id, Is.EqualTo(expected.Id));
                Assert.That(checking.CoreClockLock, Is.EqualTo(expected.CoreClockLock));
                Assert.That(checking.CoreClockState, Is.EqualTo(expected.CoreClockState));
                Assert.That(checking.CoreVoltage, Is.EqualTo(expected.CoreVoltage));
                Assert.That(checking.CoreVoltageOffset, Is.EqualTo(expected.CoreVoltageOffset));
                Assert.That(checking.MemoryClockLock, Is.EqualTo(expected.MemoryClockLock));
                Assert.That(checking.MemoryClockState, Is.EqualTo(expected.MemoryClockState));
                Assert.That(checking.MemoryControllerVoltage, Is.EqualTo(expected.MemoryControllerVoltage));
                Assert.That(checking.MemoryTweak, Is.EqualTo(expected.MemoryTweak));
                Assert.That(checking.MemoryVoltage, Is.EqualTo(expected.MemoryVoltage));
                Assert.That(checking.SocFrequency, Is.EqualTo(expected.SocFrequency));
                Assert.That(checking.SocVoltage, Is.EqualTo(expected.SocVoltage));
                Assert.That(checking.AlternativeDownVoltage, Is.EqualTo(expected.AlternativeDownVoltage));
                Assert.That(checking.EnhancedOverclock, Is.EqualTo(expected.EnhancedOverclock));
                Assert.That(checking.PowerLimit, Is.EqualTo(expected.PowerLimit));
                AssertFanOverclocking(expected.FanOverclocking, checking.FanOverclocking);
            });
        }

        void AssertIntelGpuOverclocking(IntelGpuOverclocking expected, IntelGpuOverclocking checking)
        {
            Assert.That(checking.Id, Is.EqualTo(expected.Id));
        }

        void AssertFanOverclocking(IFanOverclocking expected, IFanOverclocking checking)
        {
            switch (checking.Type)
            {
                case FanOverclockingType.TargetSpeed:
                {
                    AssertTargetSpeed(
                        (FanOverclockingWithTargetSpeed)expected,
                        (FanOverclockingWithTargetSpeed)checking);
                    break;
                }
                case FanOverclockingType.TargetTemperature:
                {
                    AssertTargetTemperature(
                        (FanOverclockingWithTargetTemperature)expected,
                        (FanOverclockingWithTargetTemperature)checking);
                    break;
                }
                case FanOverclockingType.LinearDependence:
                {
                    AssertLinearDependence(
                        (FanOverclockingWithLinearDependence)expected,
                        (FanOverclockingWithLinearDependence)checking);
                    break;
                }
            }

            void AssertTargetSpeed(FanOverclockingWithTargetSpeed expected, FanOverclockingWithTargetSpeed checking)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(checking.Id, Is.EqualTo(expected.Id));
                    Assert.That(checking.TargetSpeed, Is.EqualTo(expected.TargetSpeed));
                });
            }

            void AssertTargetTemperature(FanOverclockingWithTargetTemperature expected, FanOverclockingWithTargetTemperature checking)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(checking.Id, Is.EqualTo(expected.Id));
                    Assert.That(checking.TargetCoreTemperature, Is.EqualTo(expected.TargetCoreTemperature));
                    Assert.That(checking.TargetMemoryTemperature, Is.EqualTo(expected.TargetMemoryTemperature));
                    Assert.That(checking.MaxTargetSpeed, Is.EqualTo(expected.MaxTargetSpeed));
                    Assert.That(checking.MinTargetSpeed, Is.EqualTo(expected.MinTargetSpeed));
                });
            }

            void AssertLinearDependence(FanOverclockingWithLinearDependence expected, FanOverclockingWithLinearDependence checking)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(checking.Id, Is.EqualTo(expected.Id));
                    for (var i = 0; i < expected.TargetPoints.Length; i++)
                    {
                        Assert.That(checking.TargetPoints[i].PointIndex, Is.EqualTo(expected.TargetPoints[i].PointIndex));
                        Assert.That(checking.TargetPoints[i].FanSpeedValueTarget, Is.EqualTo(expected.TargetPoints[i].FanSpeedValueTarget));
                        Assert.That(checking.TargetPoints[i].TemperatureValueTarget, Is.EqualTo(expected.TargetPoints[i].TemperatureValueTarget));
                    }
                });
            }
        }
    }
}
