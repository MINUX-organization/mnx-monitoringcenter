using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking.Cpu;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;
using NUnit.Framework;

namespace MNX.MonitoringCenter.Management.Tests.Service.Assertions.Overclocking;

public static partial class OverclockingAssertions
{
    private static void AssertNvidiaGpu(NvidiaGpuOverclocking expected, NvidiaGpuOverclocking checking)
    {
        Assert.Multiple(() =>
        {
            Assert.That(checking.CoreClockLock, Is.EqualTo(expected.CoreClockLock));
            Assert.That(checking.CoreClockOffset, Is.EqualTo(expected.CoreClockOffset));
            Assert.That(checking.CoreVoltage, Is.EqualTo(expected.CoreVoltage));
            Assert.That(checking.CoreVoltageOffset, Is.EqualTo(expected.CoreVoltageOffset));
            Assert.That(checking.MemoryClockLock, Is.EqualTo(expected.MemoryClockLock));
            Assert.That(checking.MemoryClockOffset, Is.EqualTo(expected.MemoryClockOffset));
            Assert.That(checking.MemoryVoltage, Is.EqualTo(expected.MemoryVoltage));
            Assert.That(checking.MemoryVoltageOffset, Is.EqualTo(expected.MemoryVoltageOffset));
            Assert.That(checking.PowerLimit, Is.EqualTo(expected.PowerLimit));
            AssertFan(expected.FanOverclocking, checking.FanOverclocking);
        });
    }

    static void AssertAmdGpu(AmdGpuOverclocking expected, AmdGpuOverclocking checking)
    {
        Assert.Multiple(() =>
        {
            Assert.That(checking.CoreClockLock, Is.EqualTo(expected.CoreClockLock));
            Assert.That(checking.CoreClockState, Is.EqualTo(expected.CoreClockState));
            Assert.That(checking.CoreVoltage, Is.EqualTo(expected.CoreVoltage));
            Assert.That(checking.CoreVoltageOffset, Is.EqualTo(expected.CoreVoltageOffset));
            Assert.That(checking.MemoryClockLock, Is.EqualTo(expected.MemoryClockLock));
            Assert.That(checking.MemoryClockState, Is.EqualTo(expected.MemoryClockState));
            Assert.That(checking.MemoryVoltage, Is.EqualTo(expected.MemoryVoltage));
            Assert.That(checking.MemoryControllerVoltage, Is.EqualTo(expected.MemoryControllerVoltage));
            Assert.That(checking.MemoryTweak, Is.EqualTo(expected.MemoryTweak));
            Assert.That(checking.EnhancedOverclock, Is.EqualTo(expected.EnhancedOverclock));
            Assert.That(checking.AlternativeDownVoltage, Is.EqualTo(expected.AlternativeDownVoltage));
            Assert.That(checking.SocFrequency, Is.EqualTo(expected.SocFrequency));
            Assert.That(checking.SocVoltage, Is.EqualTo(expected.SocVoltage));
            Assert.That(checking.PowerLimit, Is.EqualTo(expected.PowerLimit));
            AssertFan(expected.FanOverclocking, checking.FanOverclocking);
        });
    }

    private static void AssertIntelGpu(IntelGpuOverclocking expected, IntelGpuOverclocking checking)
    {
        // Нет дополнительных свойств.
    }

    private static void AssertCpu(CpuOverclocking expected, CpuOverclocking checking)
    {
        Assert.Multiple(() =>
        {
            Assert.That(checking.CoreClockLock, Is.EqualTo(expected.CoreClockLock));
            Assert.That(checking.CoreVoltage, Is.EqualTo(expected.CoreVoltage));
        });
    }

    private static void AssertFan(IFanOverclocking? expected, IFanOverclocking? checking)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        Assert.Multiple(() =>
        {
            Assert.That(checking!.Id, Is.EqualTo(expected!.Id));
            Assert.That(checking.Type, Is.EqualTo(expected.Type));
        });

        switch (expected, checking)
        {
            case (FanOverclockingWithTargetSpeed e, FanOverclockingWithTargetSpeed c):
                AssertTargetSpeed(e, c);
                break;
            case (FanOverclockingWithTargetTemperature e, FanOverclockingWithTargetTemperature c):
                AssertTargetTemperature(e, c);
                break;
            case (FanOverclockingWithLinearDependence e, FanOverclockingWithLinearDependence c):
                AssertLinearDependence(e, c);
                break;
        }
    }

    private static void AssertTargetSpeed(
        FanOverclockingWithTargetSpeed expected, FanOverclockingWithTargetSpeed checking)
    {
        Assert.That(checking.TargetSpeed, Is.EqualTo(expected.TargetSpeed));
    }

    private static void AssertTargetTemperature(
        FanOverclockingWithTargetTemperature expected, FanOverclockingWithTargetTemperature checking)
    {
        Assert.Multiple(() =>
        {
            Assert.That(checking.MaxTargetSpeed, Is.EqualTo(expected.MaxTargetSpeed));
            Assert.That(checking.MinTargetSpeed, Is.EqualTo(expected.MinTargetSpeed));
            Assert.That(checking.TargetCoreTemperature, Is.EqualTo(expected.TargetCoreTemperature));
            Assert.That(checking.TargetMemoryTemperature, Is.EqualTo(expected.TargetMemoryTemperature));
        });
    }

    private static void AssertLinearDependence(
        FanOverclockingWithLinearDependence expected, FanOverclockingWithLinearDependence checking)
    {
        Assert.That(checking.TargetPoints, Has.Length.EqualTo(expected.TargetPoints.Length));
        for (var i = 0; i < expected.TargetPoints.Length; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(checking.TargetPoints[i].PointIndex,
                    Is.EqualTo(expected.TargetPoints[i].PointIndex));

                Assert.That(checking.TargetPoints[i].TemperatureValueTarget,
                    Is.EqualTo(expected.TargetPoints[i].TemperatureValueTarget));

                Assert.That(checking.TargetPoints[i].FanSpeedValueTarget,
                    Is.EqualTo(expected.TargetPoints[i].FanSpeedValueTarget));
            });
        }
    }
}
