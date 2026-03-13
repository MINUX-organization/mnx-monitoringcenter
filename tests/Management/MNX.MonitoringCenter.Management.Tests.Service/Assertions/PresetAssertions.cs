using MNX.MonitoringCenter.Management.Tests.Service.Assertions.Overclocking;
using NUnit.Framework;

namespace MNX.MonitoringCenter.Management.Tests.Service.Assertions;

using Preset = Core.Overclocking.Preset;

public static class PresetAssertions
{
    public static void ShouldBeEqualTo(this IEnumerable<Preset?>? checking, IEnumerable<Preset?>? expected)
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

    public static void ShouldBeEqualTo(this Preset? checking, Preset? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        Assert.Multiple(() =>
        {
            Assert.That(checking!.Id, Is.EqualTo(expected!.Id));
            Assert.That(checking.Name, Is.EqualTo(expected.Name));
            Assert.That(checking.DeviceName, Is.EqualTo(expected.DeviceName));
            Assert.That(checking.IsVisible, Is.EqualTo(expected.IsVisible));
            Assert.That(checking.OwnerId, Is.EqualTo(expected.OwnerId));
            Assert.That(checking.OverclockingId, Is.EqualTo(expected.OverclockingId));
            checking.Overclocking.ShouldBeEquivalentTo(expected.Overclocking);
        });
    }
}
