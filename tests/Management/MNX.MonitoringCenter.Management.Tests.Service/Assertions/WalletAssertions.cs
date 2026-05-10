using FluentAssertions;
using NUnit.Framework;

namespace MNX.MonitoringCenter.Management.Tests.Service.Assertions;

using Wallet = Core.Mining.Wallet;

public static class WalletAssertions
{
    public static void ShouldBeEqualTo(this IEnumerable<Wallet?>? checking, IEnumerable<Wallet?>? expected)
    {
        if (expected is null)
        {
            checking.Should().BeNull();
            return;
        }

        checking.Should().NotBeNull();
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
        if (expected is null)
        {
            checking.Should().BeNull();
            return;
        }

        checking.Should().NotBeNull();
        checking.Should().Satisfy<Wallet>(x =>
        {
            x.Id.Should().Be(expected.Id);
            x.OwnerId.Should().Be(expected.OwnerId);
            x.Name.Should().Be(expected.Name);
            x.Address.Should().Be(expected.Address);
            x.CryptocurrencyId.Should().Be(expected.CryptocurrencyId);
            x.Cryptocurrency.ShouldBeEqualTo(expected.Cryptocurrency);
        });
    }
}
