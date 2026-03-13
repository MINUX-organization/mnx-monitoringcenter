using MNX.MonitoringCenter.Management.Core.Mining;
using NUnit.Framework;

namespace MNX.MonitoringCenter.Management.Tests.Service.Assertions;

public static class PoolAssertions
{
    public static void ShouldBeEqualTo(this IEnumerable<Pool?>? checking, IEnumerable<Pool?>? expected)
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

    public static void ShouldBeEqualTo(this Pool? checking, Pool? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        Assert.Multiple(() =>
        {
            Assert.That(checking!.Id, Is.EqualTo(expected!.Id));
            Assert.That(checking.OwnerId, Is.EqualTo(expected.OwnerId));
            Assert.That(checking.Domain, Is.EqualTo(expected.Domain));
            Assert.That(checking.Port, Is.EqualTo(expected.Port));
            Assert.That(checking.Tls, Is.EqualTo(expected.Tls));
            Assert.That(checking.CryptocurrencyId, Is.EqualTo(expected.CryptocurrencyId));
            checking.Cryptocurrency.ShouldBeEqualTo(expected.Cryptocurrency);
        });
    }
}
