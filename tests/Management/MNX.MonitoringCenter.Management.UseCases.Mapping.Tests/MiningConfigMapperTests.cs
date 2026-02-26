using MNX.MonitoringCenter.Management.Contracts.FlightSheet.MiningConfigs;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.MiningConfigInputModels;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.MiningConfigs;
using MNX.MonitoringCenter.Management.UseCases.Mapping.MiningConfig;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Pool;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Wallet;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.Models.MiningConfig;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool;
using MNX.MonitoringCenter.Management.UseCases.Mining.Wallet;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests;
[TestFixture]
public sealed class MiningConfigMapperTests
{
    private IPoolMapper _poolMapper;
    private IWalletMapper _walletMapper;
    private IMiningConfigMapper _miningConfigMapper;

    [SetUp]
    public void SetUp()
    {
        _walletMapper = new WalletMapper();
        _poolMapper = new PoolMapper();
        _miningConfigMapper = new MiningConfigMapper(_poolMapper, _walletMapper);
    }

    [TestCaseSource(typeof(MiningConfigMappingTestData), nameof(MiningConfigMappingTestData.CpuMiningConfigInputModels))]
    public void MapToCoreEntity_ValidCpuMiningConfigInputModels_ReturnMiningConfigs(MiningConfigInputModel data)
    {
        // Arrange
        var cpuData = (CpuMiningConfigInputModel)data;
        var hugePages = cpuData.HugePages;
        var threadsCount = cpuData.ThreadsCount;


        // Act

        var mappedData = _miningConfigMapper.MapToCoreEntity(data);


        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(mappedData, Is.Not.Null);
            Assert.That(mappedData, Is.TypeOf<CpuMiningConfig>());

            var mappedCpuConfig = (CpuMiningConfig)mappedData;
            Assert.That(mappedCpuConfig.DeviceType, Is.EqualTo(MiningDeviceType.CPU));
            Assert.That(mappedCpuConfig.AdditionalArguments, Is.EqualTo(data.AdditionalArguments));
            Assert.That(mappedCpuConfig.ConfigFileContent, Is.EqualTo(data.ConfigFileContent));
            Assert.That(mappedCpuConfig.HugePages, Is.EqualTo(hugePages));
            Assert.That(mappedCpuConfig.ThreadsCount, Is.EqualTo(threadsCount));
            Assert.That(mappedCpuConfig.CoinConfigs, Has.Count.EqualTo(data.CoinConfigs.Count));
            CheckCoinConfigs(data.CoinConfigs, mappedCpuConfig.CoinConfigs);
        });
    }

    [TestCaseSource(typeof(MiningConfigMappingTestData), nameof(MiningConfigMappingTestData.GpuMiningConfigInputModels))]
    public void MapToCoreEntity_ValidGpuMiningConfigInputModels_ReturnMiningConfigs(MiningConfigInputModel data)
    {
        // Arrange
        // Act

        var mappedData = _miningConfigMapper.MapToCoreEntity(data);


        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(mappedData, Is.Not.Null);
            Assert.That(mappedData, Is.TypeOf<GpuMiningConfig>());

            var mappedGpuConfig = (GpuMiningConfig)mappedData;
            Assert.That(mappedGpuConfig.DeviceType, Is.EqualTo(MiningDeviceType.GPU));
            Assert.That(mappedGpuConfig.AdditionalArguments, Is.EqualTo(data.AdditionalArguments));
            Assert.That(mappedGpuConfig.ConfigFileContent, Is.EqualTo(data.ConfigFileContent));
            Assert.That(mappedGpuConfig.CoinConfigs, Has.Count.EqualTo(data.CoinConfigs.Count));
            CheckCoinConfigs(data.CoinConfigs, mappedGpuConfig.CoinConfigs);
        });
    }

    [TestCaseSource(typeof(MiningConfigMappingTestData), nameof(MiningConfigMappingTestData.CpuMiningConfigs))]
    public void MapToModel_ValidCpuMiningConfigs_ReturnCpuMiningConfigInputModels(BaseMiningConfig data)
    {
        // Arrange

        var cpuData = (CpuMiningConfig)data;

        // Act

        var mappedModel = _miningConfigMapper.MapToModel(data);


        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(mappedModel, Is.Not.Null);
            Assert.That(mappedModel, Is.TypeOf<CpuMiningConfigModel>());

            var cpuMappedModel = (CpuMiningConfigModel)mappedModel;
            Assert.That(cpuMappedModel.AdditionalArguments, Is.EqualTo(cpuData.AdditionalArguments));
            Assert.That(cpuMappedModel.ConfigFileContent, Is.EqualTo(cpuData.ConfigFileContent));
            Assert.That(cpuMappedModel.HugePages, Is.EqualTo(cpuData.HugePages));
            Assert.That(cpuMappedModel.ThreadsCount, Is.EqualTo(cpuData.ThreadsCount));
            CheckCoinConfigModels(cpuData.CoinConfigs, cpuMappedModel.CoinConfigs);
        });
    }

    [TestCaseSource(typeof(MiningConfigMappingTestData), nameof(MiningConfigMappingTestData.GpuMiningConfigs))]
    public void MapToModel_ValidGpuMiningConfigs_ReturnGpuMiningConfigInputModels(BaseMiningConfig data)
    {
        // Arrange
        // Act

        var mappedModel = _miningConfigMapper.MapToModel(data);


        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(mappedModel, Is.Not.Null);
            Assert.That(mappedModel, Is.TypeOf<GpuMiningConfigModel>());

            var gpuMappedModel = (GpuMiningConfigModel)mappedModel;
            Assert.That(gpuMappedModel.AdditionalArguments, Is.EqualTo(data.AdditionalArguments));
            Assert.That(gpuMappedModel.ConfigFileContent, Is.EqualTo(data.ConfigFileContent));
            CheckCoinConfigModels(data.CoinConfigs, gpuMappedModel.CoinConfigs);
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

    private class MiningConfigMappingTestData
    {
        public static IEnumerable<MiningConfigInputModel> CpuMiningConfigInputModels
        {
            get
            {
                var poolId = Guid.NewGuid();
                var walletId = Guid.NewGuid();

                yield return new CpuMiningConfigInputModelBuilder()
                    .WithAdditionalArguments("Argument1, Argument2, Argument3")
                    .WithConfigFileContent("{\n\tcontent:\n\t{\n\t\tParam1,\n\t\tParam2,\n\t\tParam3\n\t}\n}")
                    .WithHugePages(3)
                    .WithThreadsCount(8)
                    .AddCoinConfig(coinConfig =>
                        coinConfig.WithPoolPassword("123456")
                                  .WithPoolId(poolId)
                                  .WithWalletId(walletId))
                    .AddCoinConfig(coinConfig =>
                        coinConfig.WithPoolId(poolId)
                                  .WithWalletId(walletId))
                    .AddCoinConfig(coinConfig =>
                        coinConfig.WithPoolPassword("qwertyui")
                                  .WithPoolId(poolId)
                                  .WithWalletId(walletId))
                    .Build();
                yield return new CpuMiningConfigInputModelBuilder()
                    .WithAdditionalArguments("Argument43")
                    .WithHugePages(4)
                    .WithThreadsCount(6)
                    .AddCoinConfig(coinConfig =>
                        coinConfig.WithPoolPassword("x")
                                  .WithPoolId(poolId)
                                  .WithWalletId(walletId))
                    .Build();
                yield return new CpuMiningConfigInputModelBuilder()
                    .WithConfigFileContent("{\n\tcontent:\n\t{\n\t\tParam1,\n\t\tParam2,\n\t\tParam3,\n\t\tParam4,\n\t\tParam5\n\t}\n}")
                    .Build();
            }
        }

        public static IEnumerable<MiningConfigInputModel> GpuMiningConfigInputModels
        {
            get
            {
                var poolId = Guid.NewGuid();
                var walletId = Guid.NewGuid();

                yield return new GpuMiningConfigInputModelBuilder()
                    .WithConfigFileContent("{\n\tcontent:\n\t{\n\t\tParam1,\n\t\tParam2,\n\t\tParam3,\n\t\tParam4,\n\t\tParam5\n\t}\n}")
                    .Build();
                yield return new GpuMiningConfigInputModelBuilder()
                    .WithAdditionalArguments("Argument1, Argument2, Argument3")
                    .WithConfigFileContent("{\n\tcontent:\n\t{\n\t\tParam1,\n\t\tParam2,\n\t\tParam3\n\t}\n}")
                    .AddCoinConfig(coinConfig =>
                        coinConfig.WithPoolPassword("123456")
                                  .WithPoolId(poolId)
                                  .WithWalletId(walletId))
                    .AddCoinConfig(coinConfig =>
                        coinConfig.WithPoolId(poolId)
                                  .WithWalletId(walletId))
                    .AddCoinConfig(coinConfig =>
                        coinConfig.WithPoolPassword("qwertyui")
                                  .WithPoolId(poolId)
                                  .WithWalletId(walletId))
                    .Build();
                yield return new GpuMiningConfigInputModelBuilder()
                    .WithAdditionalArguments("Argument43")
                    .AddCoinConfig(coinConfig =>
                        coinConfig.WithPoolId(poolId)
                                  .WithWalletId(walletId)
                                  .WithPoolPassword("x"))
                    .Build();
            }
        }

        public static IEnumerable<BaseMiningConfig> CpuMiningConfigs
        {
            get
            {
                var ownerId = Guid.NewGuid();
                var cryptocurrencyId = Guid.NewGuid();
                var algorithmId = Guid.NewGuid();

                yield return new CpuMiningConfigBuilder()
                    .WithAdditionalArguments("Argument7")
                    .WithConfigFileContent("{\n\tcontent:\n\t{\n\t\tParam1,\n\t\tParam2,\n\t\tParam3,\n\t\tParam4,\n\t\tParam5\n\t}\n}")
                    .WithThreads(5)
                    .WithHugePages(3)
                    .AddCoinConfig(coinConfig =>
                        coinConfig.WithPool(pool =>
                            pool.WithTls()
                                .WithOwner(ownerId)
                                .WithCryptocurrency(crypto =>
                                    crypto.WithId(cryptocurrencyId)
                                          .WithOwner(ownerId)
                                          .WithAlgorithm(algo =>
                                            algo.WithId(algorithmId)
                                                .WithOwner(ownerId))))
                                  .WithWallet(wallet =>
                                    wallet.WithOwnerId(ownerId)
                                          .WithCryptocurrency(crypto =>
                                            crypto.WithId(cryptocurrencyId)
                                                  .WithOwner(ownerId)
                                                  .WithAlgorithm(algo =>
                                                    algo.WithId(algorithmId)
                                                        .WithOwner(ownerId))))
                                  .WithPoolPassword("123456"))
                    .AddCoinConfig(coinConfig =>
                        coinConfig.WithPool(pool =>
                            pool.WithOwner(ownerId)
                                .WithTls()
                                .WithCryptocurrency(crypto =>
                                    crypto.WithId(cryptocurrencyId)
                                          .WithOwner(ownerId)
                                          .WithAlgorithm(algo =>
                                            algo.WithId(algorithmId)
                                                .WithOwner(ownerId))))
                                  .WithWallet(wallet =>
                                    wallet.WithOwnerId(ownerId)
                                          .WithCryptocurrency(crypto =>
                                            crypto.WithId(cryptocurrencyId)
                                                  .WithOwner(ownerId)
                                                  .WithAlgorithm(algo =>
                                                    algo.WithId(algorithmId)
                                                        .WithOwner(ownerId)))))
                    .AddCoinConfig(coinConfig =>
                        coinConfig.WithPool(pool =>
                            pool.WithTls()
                                .WithOwner(ownerId)
                                .WithCryptocurrency(crypto =>
                                    crypto.WithId(cryptocurrencyId)
                                          .WithOwner(ownerId)
                                          .WithAlgorithm(algo =>
                                            algo.WithId(algorithmId)
                                                .WithOwner(ownerId))))
                                  .WithWallet(wallet =>
                            wallet.WithOwnerId(ownerId)
                                  .WithCryptocurrency(crypto =>
                                    crypto.WithId(cryptocurrencyId)
                                          .WithOwner(ownerId)
                                          .WithAlgorithm(algo =>
                                            algo.WithId(algorithmId)
                                                .WithOwner(ownerId))))
                                  .WithPoolPassword("qwertyui"))
                    .Build();
                yield return new CpuMiningConfigBuilder()
                    .WithAdditionalArguments("Argument4, Argument5, Argument6")
                    .WithConfigFileContent("{\n\tcontent:\n\t{\n\t\tParam1,\n\t\tParam2,\n\t\tParam3\n\t}\n}")
                    .WithThreads(9)
                    .AddCoinConfig(coinConfig =>
                        coinConfig.WithWallet(wallet =>
                            wallet.WithOwnerId(ownerId)
                                  .WithCryptocurrency(crypto =>
                                    crypto.WithId(cryptocurrencyId)
                                          .WithOwner(ownerId)
                                          .WithAlgorithm(algo =>
                                             algo.WithId(algorithmId)
                                                 .WithOwner(ownerId))))
                                  .WithPool(pool =>
                                    pool.WithTls()
                                        .WithOwner(ownerId)
                                        .WithCryptocurrency(crypto =>
                                            crypto.WithId(cryptocurrencyId)
                                                  .WithOwner(ownerId)
                                                  .WithAlgorithm(algo =>
                                                    algo.WithId(algorithmId)
                                                        .WithOwner(ownerId))))
                                  .WithPoolPassword("123456"))
                    .Build();
                yield return new CpuMiningConfigBuilder()
                    .WithAdditionalArguments("Argument1, Argument2, Argument3")
                    .Build();
            }
        }

        public static IEnumerable<BaseMiningConfig> GpuMiningConfigs
        {
            get
            {
                var ownerId = Guid.NewGuid();
                var cryptocurrencyId = Guid.NewGuid();
                var algorithmId = Guid.NewGuid();

                yield return new GpuMiningConfigBuilder()
                    .WithAdditionalArguments("Argument1, Argument2, Argument3, Argument4")
                    .WithConfigFileContent("{\n\tcontent:\n\t{\n\t\tParam1,\n\t\tParam2,\n\t\tParam3,\n\t\tParam4,\n\t\tParam5\n\t}\n}")
                    .AddCoinConfig(coinConfig =>
                        coinConfig.WithPool(pool =>
                            pool.WithTls()
                                .WithOwner(ownerId)
                                .WithCryptocurrency(crypto =>
                                    crypto.WithId(cryptocurrencyId)
                                          .WithOwner(ownerId)
                                          .WithAlgorithm(algo =>
                                            algo.WithId(algorithmId)
                                                .WithOwner(ownerId))))
                                  .WithWallet(wallet =>
                            wallet.WithOwnerId(ownerId)
                                  .WithCryptocurrency(crypto =>
                                    crypto.WithId(cryptocurrencyId)
                                          .WithOwner(ownerId)
                                          .WithAlgorithm(algo =>
                                            algo.WithId(algorithmId)
                                                .WithOwner(ownerId))))
                                  .WithPoolPassword("123456"))
                    .AddCoinConfig(coinConfig =>
                        coinConfig.WithPool(pool =>
                            pool.WithTls()
                                .WithOwner(ownerId)
                                .WithCryptocurrency(crypto =>
                                    crypto.WithId(cryptocurrencyId)
                                          .WithOwner(ownerId)
                                          .WithAlgorithm(algo =>
                                            algo.WithId(algorithmId)
                                                .WithOwner(ownerId))))
                                .WithWallet(wallet =>
                            wallet.WithOwnerId(ownerId)
                                  .WithCryptocurrency(crypto =>
                                    crypto.WithId(cryptocurrencyId)
                                          .WithOwner(ownerId)
                                          .WithAlgorithm(algo =>
                                            algo.WithId(algorithmId)
                                                .WithOwner(ownerId))))
                                  .WithPoolPassword("qwertyui"))
                    .Build();
                yield return new GpuMiningConfigBuilder()
                    .WithAdditionalArguments("Argument1, Argument2, Argument3, Argument4")
                    .WithConfigFileContent("{\n\tcontent:\n\t{\n\t\tParam1,\n\t\tParam2,\n\t\tParam3,\n\t\tParam4,\n\t\tParam5\n\t}\n}")
                    .AddCoinConfig(coinConfig =>
                        coinConfig.WithWallet(wallet =>
                            wallet.WithOwnerId(ownerId)
                                  .WithCryptocurrency(crypto =>
                                    crypto.WithId(cryptocurrencyId)
                                          .WithOwner(ownerId)
                                          .WithAlgorithm(algo =>
                                            algo.WithId(algorithmId)
                                                .WithOwner(ownerId))))
                                  .WithPool(pool =>
                            pool.WithTls()
                                .WithOwner(ownerId)
                                .WithCryptocurrency(crypto =>
                                    crypto.WithId(cryptocurrencyId)
                                          .WithOwner(ownerId)
                                          .WithAlgorithm(algo =>
                                            algo.WithId(algorithmId)
                                                .WithOwner(ownerId)))))
                    .Build();
                yield return new GpuMiningConfigBuilder().Build();
            }
        }
    }
}
