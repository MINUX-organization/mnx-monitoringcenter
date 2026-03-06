using MNX.MonitoringCenter.Management.Contracts.FlightSheet;
using MNX.MonitoringCenter.Management.Contracts.FlightSheet.MiningConfigs;
using MNX.MonitoringCenter.Management.Core.Mining.FlightSheet.Target;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Enums;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.MiningConfigInputModels;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.MiningConfigModels;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.MiningConfigs;
using MNX.MonitoringCenter.Management.UseCases.Mapping.FlightSheet;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Miner;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.EditFightSheet;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.Models;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.Models.MiningConfig;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner;
using Moq;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests;

[TestFixture]
public sealed class FlightSheetMapperTests
{
    private IMinerMapper _minerMapper;
    private Mock<IMiningConfigMapper> _miningConfigMapperMock;
    private IFlightSheetMapper _flightSheetMapper;

    [SetUp]
    public void SetUp()
    {
        _minerMapper = new MinerMapper();
        _miningConfigMapperMock = new Mock<IMiningConfigMapper>();
        _flightSheetMapper = new FlightSheetMapper(_minerMapper, _miningConfigMapperMock.Object);
    }

    [Test]
    public void MapToCoreEntity_ValidInput_ReturnsMappedFlightSheet()
    {
        // Arrange

        var userId = Guid.NewGuid();

        var gpuMiningConfigInputModel = CreateMiningConfigInput(MiningDeviceType.GPU);
        var cpuMiningConfigInputModel = CreateMiningConfigInput(MiningDeviceType.CPU);
        var flightSheetInputModel = CreateFlightSheetInputModel(
            (GpuMiningConfigInputModel)gpuMiningConfigInputModel,
            (CpuMiningConfigInputModel)cpuMiningConfigInputModel);

        var mappedGpuMiningConfig = CreateBaseMiningConfig(MiningDeviceType.GPU, gpuMiningConfigInputModel);
        var mappedCpuMiningConfig = CreateBaseMiningConfig(MiningDeviceType.CPU, cpuMiningConfigInputModel);

        _miningConfigMapperMock.Setup(x => x.MapToCoreEntity(gpuMiningConfigInputModel))
            .Returns(mappedGpuMiningConfig);
        _miningConfigMapperMock.Setup(x => x.MapToCoreEntity(cpuMiningConfigInputModel))
            .Returns(mappedCpuMiningConfig);


        // Act

        var mappedFlightSheet = _flightSheetMapper.MapToCoreEntity(flightSheetInputModel, userId);


        // Assert

        Assert.That(mappedFlightSheet, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(mappedFlightSheet.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(mappedFlightSheet.Name, Is.EqualTo(flightSheetInputModel.Name));
            Assert.That(mappedFlightSheet.OwnerId, Is.EqualTo(userId));
            CheckFlightSheetTargets(flightSheetInputModel.Targets, mappedFlightSheet.Targets);
        });

        _miningConfigMapperMock.Verify(x => 
            x.MapToCoreEntity(gpuMiningConfigInputModel), Times.Once);
        _miningConfigMapperMock.Verify(x => 
            x.MapToCoreEntity(cpuMiningConfigInputModel), Times.Once);
    }

    [Test]
    public void MapToCoreEntity_ValidEditCommand_ReturnMappedFlightSheet()
    {
        // Arrange

        var commandId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var gpuMiningConfigInputModel = CreateMiningConfigInput(MiningDeviceType.GPU);
        var cpuMiningConfigInputModel = CreateMiningConfigInput(MiningDeviceType.CPU);

        var flightSheetInputModel = CreateFlightSheetInputModel(
            (GpuMiningConfigInputModel)gpuMiningConfigInputModel,
            (CpuMiningConfigInputModel)cpuMiningConfigInputModel);

        var editFlightSheetCommand = new EditFlightSheetCommand(
            commandId,
            userId,
            flightSheetInputModel);

        var mappedGpuMiningConfig = CreateBaseMiningConfig(MiningDeviceType.GPU, gpuMiningConfigInputModel);
        var mappedCpuMiningConfig = CreateBaseMiningConfig(MiningDeviceType.CPU, cpuMiningConfigInputModel);

        _miningConfigMapperMock.Setup(x => x.MapToCoreEntity(gpuMiningConfigInputModel))
            .Returns(mappedGpuMiningConfig);
        _miningConfigMapperMock.Setup(x => x.MapToCoreEntity(cpuMiningConfigInputModel))
            .Returns(mappedCpuMiningConfig);


        // Act

        var mappedFlightSheet = _flightSheetMapper.MapToCoreEntity(editFlightSheetCommand);


        // Assert

        Assert.That(mappedFlightSheet, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(mappedFlightSheet.Id, Is.EqualTo(commandId));
            Assert.That(mappedFlightSheet.OwnerId, Is.EqualTo(userId));
            Assert.That(mappedFlightSheet.Name, Is.EqualTo(editFlightSheetCommand.Model.Name));
            CheckFlightSheetTargets(editFlightSheetCommand.Model.Targets, mappedFlightSheet.Targets);
        });

        _miningConfigMapperMock.Verify(x => 
            x.MapToCoreEntity(gpuMiningConfigInputModel), Times.Once);
        _miningConfigMapperMock.Verify(x => 
            x.MapToCoreEntity(cpuMiningConfigInputModel), Times.Once);
    }

    [Test]
    public void MapToModel_ValidCoreFlightSheet_ReturnMappedModel()
    {
        // Arrange

        var flightSheetId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var flightSheet = new FlightSheetBuilder()
            .WithId(flightSheetId)
            .WithOwnerId(userId)
            .WithTargets(() => CreateFlightSheetTargets(flightSheetId, userId))
            .Build();

        _miningConfigMapperMock.Setup(x => x.MapToModel(It.IsAny<GpuMiningConfig>()))
            .Returns(CreateBaseMiningConfig(MiningDeviceType.GPU));
        _miningConfigMapperMock.Setup(x => x.MapToModel(It.IsAny<CpuMiningConfig>()))
            .Returns(CreateBaseMiningConfig(MiningDeviceType.CPU));


        // Act

        var mappedFlightSheet = _flightSheetMapper.MapToModel(flightSheet);


        // Assert

        Assert.That(mappedFlightSheet, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(mappedFlightSheet.Id, Is.EqualTo(flightSheet.Id));
            Assert.That(mappedFlightSheet.Name, Is.EqualTo(flightSheet.Name));
            Assert.That(mappedFlightSheet.Targets, Has.Count.EqualTo(flightSheet.Targets.Count));
            CheckFlightSheetTargetModels(flightSheet.Targets, mappedFlightSheet.Targets);
        });

        _miningConfigMapperMock.Verify(x => 
            x.MapToModel(It.IsAny<GpuMiningConfig>()), Times.Once);
        _miningConfigMapperMock.Verify(x =>
            x.MapToModel(It.IsAny<CpuMiningConfig>()), Times.Once);
    }

    private static void CheckFlightSheetTargetModels(List<FlightSheetTarget> expected, List<FlightSheetTargetModel> checking)
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

    private static void CheckFlightSheetTargets(List<FlightSheetTargetInputModel> expected, List<FlightSheetTarget> checking)
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

    private static List<FlightSheetTarget> CreateFlightSheetTargets(Guid flightSheetId, Guid userId)
    {
        var minerId = Guid.NewGuid();
        return
        [
            new FlightSheetTargetBuilder()
                .WithFlightSheetId(flightSheetId)
                .WithMiner(miner =>
                    miner.WithId(minerId)
                         .WithOwner(userId)
                         .WithMiningMode(MiningModeEnum.Dual)
                         .WithSupportedDevices(DeviceTypeManufacturerCombination.NvidiaGpu)
                         .WithWalletWorkerTemplate("MyWalletWorkerTemplate")
                         .WithPoolTemplate("MyPoolTemplate"))
                .WithMiningConfig(() =>
                {
                    return new GpuMiningConfigBuilder()
                        .WithAdditionalArguments("Argument1, Argument2, Argument3")
                        .WithConfigFileContent("{\n\tconfigs:\n\t{\n\t\tConfig1,\n\t\tConfig2,\n\t\tConfig3\n\t}\n}")
                        .AddCoinConfig(coinConfig =>
                            coinConfig.WithPoolPassword("123456")
                                      .WithPool(pool =>
                                        pool.WithCryptocurrency(cryptocurrency =>
                                                cryptocurrency.WithAlgorithm(algorithm => algorithm.WithId(Guid.NewGuid()))))
                                      .WithWallet(wallet =>
                                        wallet.WithCryptocurrency(cryptocurrency =>
                                                cryptocurrency.WithAlgorithm(algorithm => algorithm.WithId(Guid.NewGuid())))))
                        .AddCoinConfig(coinConfig =>
                            coinConfig.WithPool(pool =>
                                pool.WithTls()
                                    .WithCryptocurrency(cryptocurrency =>
                                        cryptocurrency.WithAlgorithm(algorithm => algorithm.WithId(Guid.NewGuid())))))
                        .Build();
                })
                .Build(),
            new FlightSheetTargetBuilder()
                .WithFlightSheetId(flightSheetId)
                .WithMiner(miner =>
                    miner.WithId(minerId)
                         .WithOwner(userId)
                         .WithMiningMode(MiningModeEnum.Triple)
                         .WithSupportedDevices(DeviceTypeManufacturerCombination.NvidiaGpu)
                         .WithWalletWorkerTemplate("MyWalletWorkerTemplate")
                         .WithPoolTemplate("MyPoolTemplate"))
                .WithMiningConfig(() =>
                {
                    return new CpuMiningConfigBuilder()
                        .WithAdditionalArguments("Argument1, Argument2, Argument3")
                        .WithConfigFileContent("{\n\tconfigs:\n\t{\n\t\tConfig1,\n\t\tConfig2,\n\t\tConfig3,\n\t\tConfig4,\n\t\tConfig5\n\t}\n}")
                        .AddCoinConfig(coinConfig =>
                            coinConfig.WithPoolPassword("123456")
                                      .WithPool(pool =>
                                        pool.WithCryptocurrency(crypto =>
                                                crypto.WithAlgorithm(algorithm => algorithm.WithId(Guid.NewGuid()))))
                                      .WithWallet(wallet =>
                                        wallet.WithCryptocurrency(crypto =>
                                                crypto.WithAlgorithm(algorithm => algorithm.WithId(Guid.NewGuid())))))
                        .Build();
                })
                .Build()
        ];
    }

    private static FlightSheetInputModel CreateFlightSheetInputModel(
        GpuMiningConfigInputModel gpuMiningConfigInputModel, CpuMiningConfigInputModel cpuMiningConfigInputModel)
    {
        return new FlightSheetInputModelBuilder()
            .AddFlightSheetTarget(target =>
                target.WithMinerId(Guid.NewGuid())
                      .WithMiningConfig(() => gpuMiningConfigInputModel))
            .AddFlightSheetTarget(target =>
                target.WithMinerId(Guid.NewGuid())
                      .WithMiningConfig(() => cpuMiningConfigInputModel))
            .Build();
    }

    private static MiningConfigInputModel CreateMiningConfigInput(MiningDeviceType type)
    {
        switch (type)
        {
            case MiningDeviceType.GPU:
            {
                return new GpuMiningConfigInputModelBuilder()
                        .WithAdditionalArguments("Argument1, Argument2, Argument3")
                        .WithConfigFileContent("{\n\tconfigs:\n\t{\n\t\tConfig1,\n\t\tConfig2,\n\t\tConfig3\n\t}\n}")
                        .AddCoinConfig(coinConfig => coinConfig.WithPoolPassword("123456"))
                        .AddCoinConfig(coinConfig => coinConfig.WithPoolPassword("654321"))
                        .Build();
            }
            case MiningDeviceType.CPU:
            {
                return new CpuMiningConfigInputModelBuilder()
                        .WithAdditionalArguments("Argument1, Argument2, Argument3")
                        .WithConfigFileContent("{\n\tconfigs:\n\t{\n\t\tConfig1,\n\t\tConfig2,\n\t\tConfig3\n\t}\n}")
                        .WithHugePages(3)
                        .WithThreadsCount(8)
                        .AddCoinConfig(coinConfig => coinConfig.WithPoolPassword("123456"))
                        .AddCoinConfig(coinConfig => coinConfig.WithPoolPassword("654321"))
                        .Build();
            }
            default:
                throw new ArgumentException("Unknown device type");
        }
    }

    private static BaseMiningConfig CreateBaseMiningConfig(MiningDeviceType type, MiningConfigInputModel miningConfigInputModel)
    {
        switch (type)
        {
            case MiningDeviceType.GPU:
            {
                return new GpuMiningConfigBuilder()
                    .WithAdditionalArguments(miningConfigInputModel.AdditionalArguments)
                    .WithConfigFileContent(miningConfigInputModel.ConfigFileContent)
                    .AddCoinConfig(coinConfig =>
                        coinConfig.WithPool(pool => pool.WithId(miningConfigInputModel.CoinConfigs[0].PoolId))
                                  .WithPoolPassword(miningConfigInputModel.CoinConfigs[0].PoolPassword)
                                  .WithWallet(wallet => wallet.WithId(miningConfigInputModel.CoinConfigs[0].WalletId)))
                    .AddCoinConfig(coinConfig =>
                        coinConfig.WithPool(pool => pool.WithId(miningConfigInputModel.CoinConfigs[1].PoolId))
                                  .WithPoolPassword(miningConfigInputModel.CoinConfigs[1].PoolPassword)
                                  .WithWallet(wallet => wallet.WithId(miningConfigInputModel.CoinConfigs[1].WalletId)))
                    .Build();
            }
            case MiningDeviceType.CPU:
            {
                return new CpuMiningConfigBuilder()
                    .WithAdditionalArguments(miningConfigInputModel.AdditionalArguments)
                    .WithConfigFileContent(miningConfigInputModel.ConfigFileContent)
                    .WithHugePages(((CpuMiningConfigInputModel)miningConfigInputModel).HugePages.Value)
                    .WithThreads(((CpuMiningConfigInputModel)miningConfigInputModel).ThreadsCount.Value)
                    .AddCoinConfig(coinConfig =>
                        coinConfig.WithPool(pool => pool.WithId(miningConfigInputModel.CoinConfigs[0].PoolId))
                                  .WithPoolPassword(miningConfigInputModel.CoinConfigs[0].PoolPassword)
                                  .WithWallet(wallet => wallet.WithId(miningConfigInputModel.CoinConfigs[0].WalletId)))
                    .AddCoinConfig(coinConfig =>
                        coinConfig.WithPool(pool => pool.WithId(miningConfigInputModel.CoinConfigs[1].PoolId))
                                  .WithPoolPassword(miningConfigInputModel.CoinConfigs[1].PoolPassword)
                                  .WithWallet(wallet => wallet.WithId(miningConfigInputModel.CoinConfigs[1].WalletId)))
                    .Build();
            }
            default:
                throw new ArgumentException($"Invalid type of {nameof(MiningConfigInputModel)}");
        }
    }

    private static BaseMiningConfigModel CreateBaseMiningConfig(MiningDeviceType type)
    {
        switch (type)
        {
            case MiningDeviceType.GPU:
            {
                return new GpuMiningConfigModelBuilder()
                    .WithAdditionalArguments("Argument1, Argument2, Argument3, Argument4, Argument5")
                    .WithConfigFileContent("{\n\tconfigs:\n\t{\n\t\tConfig1,\n\t\tConfig2,\n\t\tConfig3\n\t}\n}")
                    .AddCoinConfig(coinConfig =>
                        coinConfig.WithWallet(wallet => wallet.WithCryptocurrencyId(Guid.NewGuid()))
                                  .WithPool(pool => pool.WithTls()))
                    .Build();
            }
            case MiningDeviceType.CPU:
            {
                return new CpuMiningConfigModelBuilder()
                        .WithAdditionalArguments("Argument1, Argument2, Argument3, Argument4, Argument5")
                        .WithConfigFileContent("{\n\tconfigs:\n\t{\n\t\tConfig1,\n\t\tConfig2,\n\t\tConfig3\n\t}\n}")
                        .WithThreadsCount(3)
                        .WithHugePages(5)
                        .AddCoinConfig(coinConfig =>
                            coinConfig.WithWallet(wallet => wallet.WithCryptocurrencyId(Guid.NewGuid()))
                                      .WithPool(pool => pool.WithTls()))
                        .Build();
            }
            default:
                throw new ArgumentException($"Invalid type of {nameof(BaseMiningConfigModel)}");
        }
    }
}
