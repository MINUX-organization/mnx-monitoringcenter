using NUnit.Framework;

namespace MNX.MonitoringCenter.Management.Tests.Service.Assertions;

using Algorithm = Core.Mining.Algorithm;

public static class AlgorithmAssertions
{
    public static void ShouldBeEqualTo(this IEnumerable<Algorithm?> checking, IEnumerable<Algorithm?> expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        var expectedList = expected
            .Where(x => x is not null)
            .OrderBy(x => x!.Id)
            .ToList();
        var checkingList = checking
            .Where(x => x is not null)
            .OrderBy(x => x!.Id)
            .ToList();

        Assert.That(checking, Has.Count.EqualTo(expectedList.Count));
        for (var i = 0; i < expectedList.Count; i++)
        {
            checkingList[i].ShouldBeEqualTo(expectedList[i]);
        }
    }

    public static void ShouldBeEqualTo(this Algorithm? checking, Algorithm? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        Assert.Multiple(() =>
        {
            Assert.That(checking!.Id, Is.EqualTo(expected!.Id));
            Assert.That(checking.Name, Is.EqualTo(checking.Name));
            Assert.That(checking.OwnerId, Is.EqualTo(checking.OwnerId));
        });
    }
}
