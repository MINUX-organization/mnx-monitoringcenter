using MNX.MonitoringCenter.Management.Agent.Commands.Mining.ApplySettings.Models;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet;
using NUnit.Framework;

namespace MNX.MonitoringCenter.Management.Tests.Service.Assertions;

public static class WorkerSettingsAssertions
{
    public static void ShouldBeEqual(this List<WorkerSettings>? checking, List<DeviceFLightSheet>? expected)
    {
        if (!AssertionHelper.AssertNullConsistency(expected, checking)) return;

        var expectedList = expected!
            .Where(x => x is not null)
            .OrderBy(x => x!.Device.Id)
            .ToList();
        var checkingList = checking!
            .Where(x => x is not null)
            .OrderBy(x => x!.WorkerId)
            .ToList();

        Assert.That(checkingList, Has.Count.EqualTo(expectedList.Count));
        for (int i = 0; i < expected.Count; i++)
        {
            var expectedFlightSheet = expected[i].FlightSheet;
            var checkingSettingsModel = checking[i].SettingsModel;

            Assert.That(checking[i], Is.Not.Null);
            Assert.That(checkingSettingsModel, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(checking[i].WorkerId, Is.EqualTo(expected[i].Device.Id));

                Assert.That(checkingSettingsModel.MinerName, Is.EqualTo(expectedFlightSheet.Miner.Name));
                Assert.That(checkingSettingsModel.MinerVersion, Is.EqualTo(expectedFlightSheet.Miner.Version));
                Assert.That(checkingSettingsModel.AdditionalArguments, Is.EqualTo(expectedFlightSheet.MiningConfig.AdditionalArguments));
                Assert.That(checkingSettingsModel.ConfigFileContent, Is.EqualTo(expectedFlightSheet.MiningConfig.ConfigFileContent));

                Assert.That(checkingSettingsModel.CoinConfigs, Is.Not.Null);
                AssertCoinConfigs(expectedFlightSheet.MiningConfig.CoinConfigs, checkingSettingsModel.CoinConfigs);
            });
        }
    }
    private static void AssertCoinConfigs(List<MiningCoinConfig> expected, List<MiningCoinConfigModel> checking)
    {
        Assert.That(checking, Has.Count.EqualTo(expected.Count));

        for (int i = 0; i < expected.Count; i++)
        {
            Assert.That(checking[i], Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(checking[i].WalletAddress, Is.EqualTo(expected[i].Wallet.Address));
                Assert.That(checking[i].AlgorithmName, Is.EqualTo(expected[i].Wallet.Cryptocurrency.Algorithm.Name));
                Assert.That(checking[i].PoolHost, Is.EqualTo(expected[i].Pool.Domain));
                Assert.That(checking[i].PoolPort, Is.EqualTo(expected[i].Pool.Port));
                Assert.That(checking[i].Tls, Is.EqualTo(expected[i].Pool.Tls));
                Assert.That(checking[i].PoolPassword, Is.EqualTo(expected[i].PoolPassword));
            });
        }
    }

}
