using FluentAssertions;
using MNX.MonitoringCenter.Management.Contracts.FlightSheet;
using MNX.MonitoringCenter.Management.Core.Mining.FlightSheet.Target;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.EditFightSheet;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.Models;
using NUnit.Framework;

namespace MNX.MonitoringCenter.Management.Tests.Service.Assertions.FlightSheet;

using FlightSheet = Core.Mining.FlightSheet.FlightSheet;

public static partial class FlightSheetAssertions
{
    public static void ShouldBeEqualTo(this IEnumerable<FlightSheet?>? checking,
        IEnumerable<FlightSheet?>? expected)
    {
        if (expected is null)
        {
            checking.Should().BeNull();
            return;
        }

        checking.Should().NotBeNull();
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
        if (expected is null)
        {
            checking.Should().BeNull();
            return;
        }

        checking.Should().NotBeNull();
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
        if (expected is null)
        {
            checking.Should().BeNull();
            return;
        }

        checking.Should().NotBeNull();
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
        if (expected is null)
        {
            checking.Should().BeNull();
            return;
        }

        checking.Should().NotBeNull();
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
        if (expected is null)
        {
            checking.Should().BeNull();
            return;
        }

        checking.Should().NotBeNull();
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
        if (expected is null)
        {
            checking.Should().BeNull();
            return;
        }

        checking.Should().NotBeNull();
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

    public static void ShouldBeEqualTo(this FlightSheet? checking, FlightSheetInputModel? expected, Guid expectedUserId)
    {
        if (expected is null)
        {
            checking.Should().BeNull();
            return;
        }

        checking.Should().NotBeNull();
        Assert.Multiple(() =>
        {
            Assert.That(checking.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(checking.Name, Is.EqualTo(expected.Name));
            Assert.That(checking.OwnerId, Is.EqualTo(expectedUserId));
            expected.Targets.ShouldBeEqualTo(checking.Targets);
        });
    }

    public static void ShouldBeEqualTo(this FlightSheet? checking, EditFlightSheetCommand? expected)
    {
        if (expected is null)
        {
            checking.Should().BeNull();
            return;
        }

        checking.Should().NotBeNull();
        Assert.Multiple(() =>
        {
            Assert.That(checking.Id, Is.EqualTo(expected.Id));
            Assert.That(checking.OwnerId, Is.EqualTo(expected.UserId));
            Assert.That(checking.Name, Is.EqualTo(expected.Model.Name));
            expected.Model.Targets.ShouldBeEqualTo(checking.Targets);
        });
    }

    public static void ShouldBeEqualTo(this FlightSheetModel? checking, FlightSheet? expected)
    {
        if (expected is null)
        {
            checking.Should().BeNull();
            return;
        }

        checking.Should().NotBeNull();
        Assert.Multiple(() =>
        {
            Assert.That(checking.Id, Is.EqualTo(expected.Id));
            Assert.That(checking.Name, Is.EqualTo(expected.Name));
            Assert.That(checking.Targets, Has.Count.EqualTo(expected.Targets.Count));
            expected.Targets.ShouldBeEqualTo(checking.Targets);
        });
    }
}