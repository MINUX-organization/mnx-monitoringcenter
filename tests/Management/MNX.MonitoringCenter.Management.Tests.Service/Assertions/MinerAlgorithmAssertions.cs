using MNX.MonitoringCenter.Management.Core.Mining.Miner;
using NUnit.Framework;

namespace MNX.MonitoringCenter.Management.Tests.Service.Assertions;

public static class MinerAlgorithmAssertions
{
    public static void ShouldBeEqualTo(this IEnumerable<MinerAlgorithm?>? checking, IEnumerable<MinerAlgorithm?>? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        var expectedList = expected!
            .Where(x => x is not null)
            .OrderBy(x => (x!.MinerId, x.AlgorithmId, x.Name))
            .ToList();
        var checkingList = checking!
            .Where(x => x is not null)
            .OrderBy(x => (x!.MinerId, x.AlgorithmId, x.Name))
            .ToList();

        Assert.That(checking, Has.Count.EqualTo(expectedList.Count));
        for (var i = 0; i < expectedList.Count; i++)
        {
            checkingList[i].ShouldBeEqualTo(expectedList[i]);
        }
    }

    public static void ShouldBeEqualTo(this MinerAlgorithm? checking, MinerAlgorithm? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        Assert.Multiple(() =>
        {
            Assert.That(checking!.AlgorithmId, Is.EqualTo(expected!.AlgorithmId));
            Assert.That(checking.MinerId, Is.EqualTo(expected.MinerId));
            Assert.That(checking.Name, Is.EqualTo(expected.Name));
        });
    }
}
