using MNX.MonitoringCenter.Management.Contracts.FlightSheet.MiningConfigs;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.Models.MiningConfig;
using NUnit.Framework;

namespace MNX.MonitoringCenter.Management.Tests.Service.Assertions;

public static class MiningConfigAssertions
{
    public static void ShouldBeEqualTo(this CpuMiningConfig checking, MiningConfigInputModel expected)
    {
        var cpuInputModel = (CpuMiningConfigInputModel)expected;

        Assert.Multiple(() =>
        {
            Assert.That(checking.DeviceType, Is.EqualTo(MiningDeviceType.CPU));
            Assert.That(checking.AdditionalArguments, Is.EqualTo(expected.AdditionalArguments));
            Assert.That(checking.ConfigFileContent, Is.EqualTo(expected.ConfigFileContent));
            Assert.That(checking.HugePages, Is.EqualTo(cpuInputModel.HugePages));
            Assert.That(checking.ThreadsCount, Is.EqualTo(cpuInputModel.ThreadsCount));
            Assert.That(checking.CoinConfigs, Has.Count.EqualTo(expected.CoinConfigs.Count));
            CheckCoinConfigs(expected.CoinConfigs, checking.CoinConfigs);
        });
    }

    public static void ShouldBeEqualTo(this GpuMiningConfig checking, MiningConfigInputModel expected)
    {
        Assert.Multiple(() =>
        {
            Assert.That(checking.DeviceType, Is.EqualTo(MiningDeviceType.GPU));
            Assert.That(checking.AdditionalArguments, Is.EqualTo(expected.AdditionalArguments));
            Assert.That(checking.ConfigFileContent, Is.EqualTo(expected.ConfigFileContent));
            Assert.That(checking.CoinConfigs, Has.Count.EqualTo(expected.CoinConfigs.Count));
            CheckCoinConfigs(expected.CoinConfigs, checking.CoinConfigs);
        });
    }

    public static void ShouldBeEqualTo(this CpuMiningConfigModel checking, BaseMiningConfig expected)
    {
        var cpuData = (CpuMiningConfig)expected;

        Assert.Multiple(() =>
        {
            Assert.That(checking.AdditionalArguments, Is.EqualTo(cpuData.AdditionalArguments));
            Assert.That(checking.ConfigFileContent, Is.EqualTo(cpuData.ConfigFileContent));
            Assert.That(checking.HugePages, Is.EqualTo(cpuData.HugePages));
            Assert.That(checking.ThreadsCount, Is.EqualTo(cpuData.ThreadsCount));
            CheckCoinConfigModels(cpuData.CoinConfigs, checking.CoinConfigs);
        });
    }

    public static void ShouldBeEqualTo(this GpuMiningConfigModel checking, BaseMiningConfig expected)
    {
        Assert.Multiple(() =>
        {
            Assert.That(checking.AdditionalArguments, Is.EqualTo(expected.AdditionalArguments));
            Assert.That(checking.ConfigFileContent, Is.EqualTo(expected.ConfigFileContent));
            CheckCoinConfigModels(expected.CoinConfigs, checking.CoinConfigs);
        });
    }

    private static void CheckCoinConfigs(List<MiningCoinConfigInputModel> expected, List<MiningCoinConfig> checking)
    {
        for (int i = 0; i < expected.Count; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(checking[i].Id, Is.Not.EqualTo(Guid.Empty));
                Assert.That(checking[i].PoolId, Is.EqualTo(expected[i].PoolId));
                Assert.That(checking[i].Pool, Is.Null);
                Assert.That(checking[i].PoolPassword, Is.EqualTo(expected[i].PoolPassword));
                Assert.That(checking[i].WalletId, Is.EqualTo(expected[i].WalletId));
                Assert.That(checking[i].Wallet, Is.Null);
            });
        }
    }

    private static void CheckCoinConfigModels(List<MiningCoinConfig> expected, List<MiningCoinConfigModel> checking)
    {
        for (int i = 0; i < expected.Count; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(checking[i].Pool, Is.Not.Null);
                Assert.Multiple(() =>
                {
                    Assert.That(checking[i].Pool.Id, Is.EqualTo(expected[i].Pool.Id));
                    Assert.That(checking[i].Pool.Domain, Is.EqualTo(expected[i].Pool.Domain));
                    Assert.That(checking[i].Pool.Port, Is.EqualTo(expected[i].Pool.Port));
                    Assert.That(checking[i].Pool.Tls, Is.EqualTo(expected[i].Pool.Tls));
                    Assert.That(checking[i].Pool.CryptocurrencyId, Is.EqualTo(expected[i].Pool.CryptocurrencyId));
                    Assert.That(checking[i].Pool.Cryptocurrency, Is.EqualTo(expected[i].Pool.Cryptocurrency.FullName));
                });

                Assert.That(checking[i].Wallet, Is.Not.Null);
                Assert.Multiple(() =>
                {
                    Assert.That(checking[i].Wallet.Id, Is.EqualTo(expected[i].Wallet.Id));
                    Assert.That(checking[i].Wallet.Name, Is.EqualTo(expected[i].Wallet.Name));
                    Assert.That(checking[i].Wallet.Address, Is.EqualTo(expected[i].Wallet.Address));
                    Assert.That(checking[i].Wallet.CryptocurrencyId, Is.EqualTo(expected[i].Wallet.CryptocurrencyId));
                    Assert.That(checking[i].Wallet.Cryptocurrency, Is.EqualTo(expected[i].Wallet.Cryptocurrency.FullName));
                });

                Assert.That(checking[i].PoolPassword, Is.EqualTo(expected[i].PoolPassword));
            });
        }
    }
}
