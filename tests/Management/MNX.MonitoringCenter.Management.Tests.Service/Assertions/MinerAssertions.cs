using FluentAssertions;
using MNX.MonitoringCenter.Management.Core.Mining.Miner;
using NUnit.Framework;

namespace MNX.MonitoringCenter.Management.Tests.Service.Assertions;

public static class MinerAssertions
{
    public static void ShouldBeEqualTo(this IEnumerable<Miner?>? checking, IEnumerable<Miner?>? expected)
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

    public static void ShouldBeEqualTo(this Miner? checking, Miner? expected)
    {
        if (expected is null)
        {
            checking.Should().BeNull();
            return;
        }

        checking.Should().NotBeNull();
        Assert.Multiple(() =>
        {
            Assert.That(checking!.Id, Is.EqualTo(expected!.Id));
            Assert.That(checking.Type, Is.EqualTo(expected.Type));
            Assert.That(checking.OwnerId, Is.EqualTo(expected.OwnerId));
            Assert.That(checking.Name, Is.EqualTo(expected.Name));
            Assert.That(checking.Version, Is.EqualTo(expected.Version));
            Assert.That(checking.InstallationUrl, Is.EqualTo(expected.InstallationUrl));
            Assert.That(checking.MiningMode, Is.EqualTo(expected.MiningMode));
            Assert.That(checking.PoolTemplate, Is.EqualTo(expected.PoolTemplate));
            Assert.That(checking.WalletWorkerTemplate, Is.EqualTo(expected.WalletWorkerTemplate));
            Assert.That(checking.SupportedDevices, Is.EqualTo(expected.SupportedDevices));
            checking.SupportedAlgorithms.ShouldBeEqualTo(expected.SupportedAlgorithms);
        });
    }
}
