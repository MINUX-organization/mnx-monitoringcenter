using MNX.MonitoringCenter.Management.Contracts.FlightSheet.MiningConfigs;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;
using MNX.MonitoringCenter.Management.Tests.Service.Assertions;
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
        // Act

        var mappedData = _miningConfigMapper.MapToCoreEntity(data);


        // Assert

        Assert.That(mappedData, Is.Not.Null);
        Assert.That(mappedData, Is.TypeOf<CpuMiningConfig>());
        var mappedCpuConfig = (CpuMiningConfig)mappedData;
        mappedCpuConfig.ShouldBeEqualTo(data);
    }

    [TestCaseSource(typeof(MiningConfigMappingTestData), nameof(MiningConfigMappingTestData.GpuMiningConfigInputModels))]
    public void MapToCoreEntity_ValidGpuMiningConfigInputModels_ReturnMiningConfigs(MiningConfigInputModel data)
    {
        // Arrange
        // Act

        var mappedData = _miningConfigMapper.MapToCoreEntity(data);


        // Assert

        Assert.That(mappedData, Is.Not.Null);
        Assert.That(mappedData, Is.TypeOf<GpuMiningConfig>());
        var mappedGpuConfig = (GpuMiningConfig)mappedData;
        mappedGpuConfig.ShouldBeEqualTo(data);
    }

    [TestCaseSource(typeof(MiningConfigMappingTestData), nameof(MiningConfigMappingTestData.CpuMiningConfigs))]
    public void MapToModel_ValidCpuMiningConfigs_ReturnCpuMiningConfigInputModels(BaseMiningConfig data)
    {
        // Arrange
        // Act

        var mappedModel = _miningConfigMapper.MapToModel(data);


        // Assert

        Assert.That(mappedModel, Is.Not.Null);
        Assert.That(mappedModel, Is.TypeOf<CpuMiningConfigModel>());
        var cpuMappedModel = (CpuMiningConfigModel)mappedModel;
        cpuMappedModel.ShouldBeEqualTo(data);
    }

    [TestCaseSource(typeof(MiningConfigMappingTestData), nameof(MiningConfigMappingTestData.GpuMiningConfigs))]
    public void MapToModel_ValidGpuMiningConfigs_ReturnGpuMiningConfigInputModels(BaseMiningConfig data)
    {
        // Arrange
        // Act

        var mappedModel = _miningConfigMapper.MapToModel(data);


        // Assert

        Assert.That(mappedModel, Is.Not.Null);
        Assert.That(mappedModel, Is.TypeOf<GpuMiningConfigModel>());
        var gpuMappedModel = (GpuMiningConfigModel)mappedModel;
        gpuMappedModel.ShouldBeEqualTo(data);
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
