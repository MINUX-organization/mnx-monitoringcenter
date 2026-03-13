using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice;
using NUnit.Framework;

namespace MNX.MonitoringCenter.Management.Tests.Service.Assertions;

public static class MiningDeviceInfoAssertions
{
    public static void ShouldBeEqualTo(this IEnumerable<MiningDeviceInfo?>? checking,
        IEnumerable<MiningDeviceInfo?>? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        var expectedList = expected!
            .Where(x => x is not null)
            .OrderBy(x => x!.Id)
            .ToList();
        var checkingList = checking!
            .Where(x => x is not null)
            .OrderBy(x => x!.Id)
            .ToList();

        Assert.That(checkingList, Has.Count.EqualTo(expectedList.Count));
        for (var i = 0; i < expectedList.Count; i++)
        {
            checkingList[i].ShouldBeEqualTo(expectedList[i]);
        }
    }

    public static void ShouldBeEqualTo(this MiningDeviceInfo? checking, MiningDeviceInfo? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        Assert.Multiple(() =>
        {
            Assert.That(checking!.Id, Is.EqualTo(expected!.Id));
            Assert.That(checking.OwnerId, Is.EqualTo(expected.OwnerId));
            Assert.That(checking.Type, Is.EqualTo(expected.Type));
            Assert.That(checking.Manufacturer, Is.EqualTo(expected.Manufacturer));
            Assert.That(checking.Model, Is.EqualTo(expected.Model));
            Assert.That(checking.RigId, Is.EqualTo(expected.RigId));
            Assert.That(checking.LifeCycleStatus, Is.EqualTo(expected.LifeCycleStatus));
            Assert.That(checking.FlightSheetId, Is.EqualTo(expected.FlightSheetId));
            Assert.That(checking.PresetId, Is.EqualTo(expected.PresetId));
            Assert.That(checking.FlightSheetConfirmationState, Is.EqualTo(expected.FlightSheetConfirmationState));
        });
    }
}
