using MNX.MonitoringCenter.Management.Contracts.FlightSheet;
using MNX.MonitoringCenter.Management.Core.Mining.FlightSheet.Target;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.Models;
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

    private static void ShouldBeEqualTo(this List<FlightSheetTargetInputModel> expected, List<FlightSheetTarget> checking)
    {
        for (int i = 0; i < expected.Count; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(checking[i].Id, Is.Not.EqualTo(Guid.Empty));
                Assert.That(checking[i].DeviceType, Is.EqualTo(expected[i].MiningConfig.DeviceType));
                Assert.That(checking[i].MinerId, Is.EqualTo(expected[i].MinerId));
                Assert.That(checking[i].Miner, Is.Null);
                Assert.That(checking[i].FlightSheetId, Is.Not.EqualTo(Guid.Empty));
                Assert.That(checking[i].MiningConfig, Is.Not.Null);
                Assert.That(checking[i].MiningConfig.DeviceType, Is.EqualTo(expected[i].MiningConfig.DeviceType));
            });
        }
    }

    private static void ShouldBeEqualTo(this List<FlightSheetTarget> expected, List<FlightSheetTargetModel> checking)
    {
        for (int i = 0; i < expected.Count; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(checking[i].Miner.Id, Is.EqualTo(expected[i].MinerId));
                Assert.That(checking[i].Miner.Name, Is.EqualTo(expected[i].Miner.Name));
                Assert.That(checking[i].Miner.Version, Is.EqualTo(expected[i].Miner.Version));
                Assert.That(checking[i].Miner.SupportedDevices, Is.EqualTo(expected[i].Miner.SupportedDevices));
                Assert.That(checking[i].Miner.OwnerId, Is.EqualTo(expected[i].Miner.OwnerId));
                Assert.That(checking[i].Miner.InstallationUrl, Is.EqualTo(expected[i].Miner.InstallationUrl));
                Assert.That(checking[i].Miner.PoolTemplate, Is.EqualTo(expected[i].Miner.PoolTemplate));
                Assert.That(checking[i].Miner.WalletWorkerTemplate, Is.EqualTo(expected[i].Miner.WalletWorkerTemplate));
                Assert.That(checking[i].Miner.MiningMode, Is.EqualTo(expected[i].Miner.MiningMode));
                Assert.That(checking[i].Miner.SupportedAlgorithms, Has.Count.EqualTo(expected[i].Miner.SupportedAlgorithms.Count));
                Assert.That(checking[i].MiningConfig, Is.Not.Null);
            });
        }
    }
}
