using MNX.MonitoringCenter.Management.Core.Mining;
using NUnit.Framework;

namespace MNX.MonitoringCenter.Management.Tests.Service.Assertions;

public static class CryptocurrencyAssertions
{
    public static void ShouldBeEqualTo(this IEnumerable<Cryptocurrency?>? checking,
        IEnumerable<Cryptocurrency?>? expected)
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

        Assert.That(checking, Has.Count.EqualTo(expectedList.Count));
        for (var i = 0; i < expectedList.Count; i++)
        {
            checkingList[i].ShouldBeEqualTo(expectedList[i]);
        }
    }

    public static void ShouldBeEqualTo(this Cryptocurrency? checking, Cryptocurrency? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        Assert.Multiple(() =>
        {
            Assert.That(checking!.Id, Is.EqualTo(expected!.Id));
            Assert.That(checking.OwnerId, Is.EqualTo(expected.OwnerId));
            Assert.That(checking.FullName, Is.EqualTo(expected.FullName));
            Assert.That(checking.ShortName, Is.EqualTo(expected.ShortName));
            Assert.That(checking.AlgorithmId, Is.EqualTo(expected.AlgorithmId));
            checking.Algorithm.ShouldBeEqualTo(expected.Algorithm);
        });
    }
}
