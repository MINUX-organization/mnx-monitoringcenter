using NUnit.Framework;

namespace MNX.MonitoringCenter.Management.Tests.Service.Assertions;

using Wallet = Core.Mining.Wallet;

public static class WalletAssertions
{
    public static void ShouldBeEqualTo(this IEnumerable<Wallet?>? checking, IEnumerable<Wallet?>? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        var expectedList = expected!.Where(x => x is not null)
            .OrderBy(x => x!.Id)
            .ToList();
        var checkingList = checking!.Where(x => x is not null)
            .OrderBy(x => x!.Id)
            .ToList();

        Assert.That(checking, Has.Count.EqualTo(expectedList.Count));
        for (var i = 0; i < expectedList.Count; i++)
        {
            checkingList[i].ShouldBeEqualTo(expectedList[i]);
        }
    }

    public static void ShouldBeEqualTo(this Wallet? checking, Wallet? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        Assert.Multiple(() =>
        {
            Assert.That(checking!.Id, Is.EqualTo(expected!.Id));
            Assert.That(checking.OwnerId, Is.EqualTo(expected.OwnerId));
            Assert.That(checking.Name, Is.EqualTo(expected.Name));
            Assert.That(checking.Address, Is.EqualTo(expected.Address));
            Assert.That(checking.CryptocurrencyId, Is.EqualTo(expected.CryptocurrencyId));
            checking.Cryptocurrency.ShouldBeEqualTo(expected.Cryptocurrency);
        });
    }
}
