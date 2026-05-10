using FluentAssertions;
using MNX.MonitoringCenter.Management.Core.Mining;
using NUnit.Framework;

namespace MNX.MonitoringCenter.Management.Tests.Service.Assertions;

public static class CryptocurrencyAssertions
{
    public static void ShouldBeEqualTo(this IEnumerable<Cryptocurrency?>? checking,
        IEnumerable<Cryptocurrency?>? expected)
    {
        if (expected is null)
        {
            checking.Should().BeNull();
            return;
        }

        checking.Should().NotBeNull();
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
        if (expected is null)
        {
            checking.Should().BeNull();
            return;
        }

        checking.Should().NotBeNull();
        checking.Should().Satisfy<Cryptocurrency>(x =>
        {
            x.Id.Should().Be(expected.Id);
            x.OwnerId.Should().Be(expected.OwnerId);
            x.ShortName.Should().Be(expected.ShortName);
            x.FullName.Should().Be(expected.FullName);
            x.AlgorithmId.Should().Be(expected.AlgorithmId);
            x.Algorithm.ShouldBeEqualTo(expected.Algorithm);
        });
    }
}
