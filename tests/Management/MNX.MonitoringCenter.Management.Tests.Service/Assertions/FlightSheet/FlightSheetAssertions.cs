using MNX.MonitoringCenter.Management.Core.Mining.FlightSheet.Target;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;
using NUnit.Framework;

namespace MNX.MonitoringCenter.Management.Tests.Service.Assertions.FlightSheet;

using FlightSheet = Core.Mining.FlightSheet.FlightSheet;

public static partial class FlightSheetAssertions
{
    public static void ShouldBeEqualTo(this IEnumerable<FlightSheet?>? checking,
        IEnumerable<FlightSheet?>? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        var expectedList = expected!
            .OfType<FlightSheet>()
            .OrderBy(x => x!.Id)
            .ToList();
        var checkingList = checking!
            .OfType<FlightSheet>()
            .OrderBy(x => x!.Id)
            .ToList();

        Assert.That(checkingList, Has.Count.EqualTo(expectedList.Count));
        for (var i = 0; i < expectedList.Count; i++)
        {
            checkingList[i].ShouldBeEqualTo(expectedList[i]);
        }
    }

    public static void ShouldBeEqualTo(this FlightSheet? checking, FlightSheet? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        Assert.Multiple(() =>
        {
            Assert.That(checking!.Id, Is.EqualTo(expected!.Id));
            Assert.That(checking.OwnerId, Is.EqualTo(expected.OwnerId));
            Assert.That(checking.Name, Is.EqualTo(expected.Name));
            checking.Targets.ShouldBeEqualTo(expected.Targets);
        });
    }

    public static void ShouldBeEqualTo(this IEnumerable<FlightSheetTarget?>? checking,
        IEnumerable<FlightSheetTarget?>? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        var expectedList = expected!
            .OfType<FlightSheetTarget>()
            .OrderBy(x => x.Id)
            .ToList();
        var checkingList = checking!
            .OfType<FlightSheetTarget>()
            .OrderBy(x => x.Id)
            .ToList();

        Assert.That(checkingList, Has.Count.EqualTo(expectedList.Count));
        for (var i = 0; i < expectedList.Count; i++)
        {
            checkingList[i].ShouldBeEqualTo(expectedList[i]);
        }
    }

    public static void ShouldBeEqualTo(this FlightSheetTarget? checking, FlightSheetTarget? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        Assert.Multiple(() =>
        {
            Assert.That(checking!.Id, Is.EqualTo(expected!.Id));
            Assert.That(checking.FlightSheetId, Is.EqualTo(expected.FlightSheetId));
            Assert.That(checking.MinerId, Is.EqualTo(expected.MinerId));
            Assert.That(checking.DeviceType, Is.EqualTo(expected.DeviceType));

            checking.MiningConfig.ShouldBeEqualTo(expected.MiningConfig);
            checking.Miner.ShouldBeEqualTo(expected.Miner);
        });
    }

    public static void ShouldBeEqualTo(this IEnumerable<BaseMiningConfig?>? checking,
        IEnumerable<BaseMiningConfig?>? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        var expectedList = expected!
            .OfType<BaseMiningConfig>()
            .OrderBy(x => x.DeviceType)
            .ThenBy(x => x.AdditionalArguments)
            .ThenBy(x => x.ConfigFileContent)
            .ToList();
        var checkingList = checking!
            .OfType<BaseMiningConfig>()
            .OrderBy(x => x.DeviceType)
            .ThenBy(x => x.AdditionalArguments)
            .ThenBy(x => x.ConfigFileContent)
            .ToList();

        Assert.That(checkingList, Has.Count.EqualTo(expectedList.Count));
        for (var i = 0; i < expectedList.Count; i++)
        {
            checkingList[i].ShouldBeEqualTo(expectedList[i]);
        }
    }

    public static void ShouldBeEqualTo(this BaseMiningConfig? checking, BaseMiningConfig? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        Assert.Multiple(() =>
        {
            Assert.That(checking!.DeviceType, Is.EqualTo(expected!.DeviceType));
            Assert.That(checking.AdditionalArguments, Is.EqualTo(expected.AdditionalArguments));
            Assert.That(checking.ConfigFileContent, Is.EqualTo(expected.ConfigFileContent));
            checking.CoinConfigs.ShouldBeEqualTo(expected.CoinConfigs);
        });

        switch (expected, checking)
        {
            case (GpuMiningConfig e, GpuMiningConfig c):
                AssertGpuMiningConfig(e, c);
                break;

            case (CpuMiningConfig e, CpuMiningConfig c):
                AssertCpuMiningConfig(e, c);
                break;
        }
    }
}
