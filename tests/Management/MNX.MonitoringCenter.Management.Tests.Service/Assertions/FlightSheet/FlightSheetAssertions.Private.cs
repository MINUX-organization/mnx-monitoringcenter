using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;
using NUnit.Framework;

namespace MNX.MonitoringCenter.Management.Tests.Service.Assertions.FlightSheet;

public static partial class FlightSheetAssertions
{
    private static void AssertCpuMiningConfig(CpuMiningConfig expected, CpuMiningConfig checking)
    {
        Assert.Multiple(() =>
        {
            Assert.That(checking.ThreadsCount, Is.EqualTo(expected.ThreadsCount));
            Assert.That(checking.HugePages, Is.EqualTo(expected.HugePages));
        });
    }

    private static void AssertGpuMiningConfig(GpuMiningConfig expected, GpuMiningConfig checking)
    {
        // Дополнительных полей нет.
    }

    private static void ShouldBeEqualTo(this IEnumerable<MiningCoinConfig?>? checking,
        IEnumerable<MiningCoinConfig?>? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        var expectedList = expected!.Where(x => x is not null)
            .OrderBy(x => x!.Id)
            .ToList();
        var checkingList = checking!.Where(x => x is not null)
            .OrderBy(x => x!.Id)
            .ToList();

        Assert.That(checkingList, Has.Count.EqualTo(expectedList.Count));
        for (var i = 0; i < expectedList.Count; i++)
        {
            checkingList[i].ShouldBeEqualTo(expectedList[i]);
        }
    }

    private static void ShouldBeEqualTo(this MiningCoinConfig? checking, MiningCoinConfig? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        Assert.Multiple(() =>
        {
            Assert.That(checking!.Id, Is.EqualTo(expected!.Id));
            Assert.That(checking.PoolId, Is.EqualTo(expected.PoolId));
            Assert.That(checking.WalletId, Is.EqualTo(expected.WalletId));
            Assert.That(checking.PoolPassword, Is.EqualTo(expected.PoolPassword));
            checking.Pool.ShouldBeEqualTo(expected.Pool);
            checking.Wallet.ShouldBeEqualTo(expected.Wallet);
        });
    }
}
