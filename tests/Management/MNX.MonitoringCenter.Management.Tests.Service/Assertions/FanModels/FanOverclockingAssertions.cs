using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu.Fan;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;
using NUnit.Framework;

namespace MNX.MonitoringCenter.Management.Tests.Service.Assertions.FanModels;

public static class FanOverclockingAssertions
{
    public static void ShouldBeEqualTo(this IFanOverclocking checking, FanOverclockingWithTargetTemperatureModel expected, Guid expectedId)
    {
        Assert.That(checking.Id, Is.EqualTo(expectedId));
        Assert.That(checking.Type, Is.EqualTo(expected.FanOverclockingType));
    }

    public static void ShouldBeEqualTo(this FanOverclockingWithTargetTemperature checking, FanOverclockingWithTargetTemperatureModel expected)
    {
        Assert.That(checking.MaxTargetSpeed, Is.EqualTo(expected.MaxTargetSpeed));
        Assert.That(checking.MinTargetSpeed, Is.EqualTo(expected.MinTargetSpeed));
        Assert.That(checking.TargetCoreTemperature, Is.EqualTo(expected.TargetCoreTemperature));
        Assert.That(checking.TargetMemoryTemperature, Is.EqualTo(expected.TargetMemoryTemperature));
    }

    public static void ShouldBeEqualTo(this FanOverclockingWithTargetTemperatureModel checking, FanOverclockingWithTargetTemperature expected)
    {
        Assert.That(checking.MaxTargetSpeed, Is.EqualTo(expected.MaxTargetSpeed));
        Assert.That(checking.MinTargetSpeed, Is.EqualTo(expected.MinTargetSpeed));
        Assert.That(checking.TargetCoreTemperature, Is.EqualTo(expected.TargetCoreTemperature));
        Assert.That(checking.TargetMemoryTemperature, Is.EqualTo(expected.TargetMemoryTemperature));
    }
}
