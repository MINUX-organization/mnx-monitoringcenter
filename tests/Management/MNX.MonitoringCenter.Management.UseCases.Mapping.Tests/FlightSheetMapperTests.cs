using MNX.MonitoringCenter.Management.Contracts.FlightSheet;
using MNX.MonitoringCenter.Management.Contracts.FlightSheet.MiningConfigs;
using MNX.MonitoringCenter.Management.Core.Mining.FlightSheet.Target;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Enums;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.UseCases.Mapping.FlightSheet;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Miner;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Builders;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Builders.MiningConfigs;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.EditFightSheet;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.Models;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.Models.MiningConfig;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner;
using Moq;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests;

using FlightSheet = Core.Mining.FlightSheet.FlightSheet;

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
    public void MapToCoreEntity_ValidFlightSheetInputModel_ReturnFlightSheet()
    {
        // Arrange

        var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        var gpuMiningConfigInputModel = CreateMiningConfigInput(MiningDeviceType.GPU);
        var cpuMiningConfigInputModel = CreateMiningConfigInput(MiningDeviceType.CPU);
        var flightSheetInputModel = CreateFlightSheetInputModel(
            (GpuMiningConfigInputModel)gpuMiningConfigInputModel,
            (CpuMiningConfigInputModel)cpuMiningConfigInputModel);

        var mappedGpuMiningConfig = CreateBaseMiningConfig(MiningDeviceType.GPU, gpuMiningConfigInputModel);
        var mappedCpuMiningConfig = CreateBaseMiningConfig(MiningDeviceType.CPU, cpuMiningConfigInputModel);

        _miningConfigMapperMock
            .Setup(x => x.MapToCoreEntity(It.IsAny<MiningConfigInputModel>()))
            .Returns((MiningConfigInputModel baseInput) => 
            {
                return baseInput switch
                {
                    GpuMiningConfigInputModel gpuInput => mappedGpuMiningConfig,
                    CpuMiningConfigInputModel cpuInput => mappedCpuMiningConfig,
                    _ => throw new ArgumentException($"Invalid {nameof(MiningConfigInputModel)}")
                };
            });


        // Act

        var mappedFlightSheet = _flightSheetMapper.MapToCoreEntity(flightSheetInputModel, userId);


        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(mappedFlightSheet, Is.Not.Null);
            Assert.That(mappedFlightSheet.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(mappedFlightSheet.Name, Is.EqualTo(flightSheetInputModel.Name));
            Assert.That(mappedFlightSheet.OwnerId, Is.EqualTo(userId));
            CheckFlightSheetTargets(flightSheetInputModel.Targets, mappedFlightSheet.Targets);
        });
    }

    [Test]
    public void MapToCoreEntity_ValidEditFlightSheetCommand_ReturnFlightSheet()
    {
        // Arrange

        var commandId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var userId = Guid.Parse("22222222-2222-2222-2222-222222222222");

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

        _miningConfigMapperMock
            .Setup(x => x.MapToCoreEntity(It.IsAny<MiningConfigInputModel>()))
            .Returns((MiningConfigInputModel baseInput) =>
            {
                return baseInput switch
                {
                    GpuMiningConfigInputModel gpuInput => mappedGpuMiningConfig,
                    CpuMiningConfigInputModel cpuInput => mappedCpuMiningConfig,
                    _ => throw new ArgumentException($"Invalid {nameof(MiningConfigInputModel)}")
                };
            });


        // Act

        var mappedFlightSheet = _flightSheetMapper.MapToCoreEntity(editFlightSheetCommand);


        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(mappedFlightSheet, Is.Not.Null);
            Assert.That(mappedFlightSheet.Id, Is.EqualTo(commandId));
            Assert.That(mappedFlightSheet.OwnerId, Is.EqualTo(userId));
            Assert.That(mappedFlightSheet.Name, Is.EqualTo(editFlightSheetCommand.Model.Name));
            CheckFlightSheetTargets(editFlightSheetCommand.Model.Targets, mappedFlightSheet.Targets);
        });
    }

    [Test]
    public void MapToModel_ValidFlightSheet_ReturnFlightSheetModel()
    {
        // Arrange

        var flightSheetId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var userId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var flightSheet = new FlightSheet()
        {
            Id = flightSheetId,
            Name = "FlightSheet",
            OwnerId = userId,
            Targets = CreateFlightSheetTargets(flightSheetId, userId)
        };

        _miningConfigMapperMock
            .Setup(x => x.MapToModel(It.IsAny<BaseMiningConfig>()))
            .Returns((BaseMiningConfig baseData) =>
            {
                return baseData switch
                {
                    GpuMiningConfig gpuConfig => new GpuMiningConfigModel()
                    {
                        AdditionalArguments = gpuConfig.AdditionalArguments,
                        ConfigFileContent = gpuConfig.ConfigFileContent,
                        CoinConfigs = ConvertToMiningCoinConfigModel(gpuConfig.CoinConfigs),
                    },
                    CpuMiningConfig cpuConfig => new CpuMiningConfigModel()
                    {
                        AdditionalArguments = cpuConfig.AdditionalArguments,
                        ConfigFileContent = cpuConfig.ConfigFileContent,
                        HugePages = cpuConfig.HugePages,
                        ThreadsCount = cpuConfig.ThreadsCount,
                        CoinConfigs = ConvertToMiningCoinConfigModel(cpuConfig.CoinConfigs)
                    },
                    _ => throw new ArgumentException($"Invalid type of {nameof(BaseMiningConfig)}")
                };
            });

        // Act

        var mappedFlightSheet = _flightSheetMapper.MapToModel(flightSheet);


        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(mappedFlightSheet, Is.Not.Null);
            Assert.That(mappedFlightSheet.Id, Is.EqualTo(flightSheet.Id));
            Assert.That(mappedFlightSheet.Name, Is.EqualTo(flightSheet.Name));
            Assert.That(mappedFlightSheet.Targets, Has.Count.EqualTo(flightSheet.Targets.Count));
            CheckFlightSheetTargetModels(flightSheet.Targets, mappedFlightSheet.Targets);
        });
    }

    private void CheckFlightSheetTargetModels(List<FlightSheetTarget> expected, List<FlightSheetTargetModel> checking)
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
            });
        }
    }

    private void CheckFlightSheetTargets(List<FlightSheetTargetInputModel> expected, List<FlightSheetTarget> checking)
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
            });
        }
    }

    private List<MiningCoinConfigModel> ConvertToMiningCoinConfigModel(List<MiningCoinConfig> miningCoinConfigs)
    {
        var result = new List<MiningCoinConfigModel>();
        foreach (var config in miningCoinConfigs)
        {
            result.Add(new MiningCoinConfigModel()
            {
                Wallet = new Contracts.WalletModel()
                {
                    Id = config.WalletId,
                    Name = config.Wallet.Name,
                    Address = config.Wallet.Address,
                    CryptocurrencyId = config.Wallet.CryptocurrencyId,
                    Cryptocurrency = config.Wallet.Cryptocurrency.FullName,
                },
                Pool = new Contracts.PoolModel()
                {
                    Id = config.PoolId,
                    Cryptocurrency = config.Pool.Cryptocurrency.FullName,
                    Domain = config.Pool.Domain,
                    CryptocurrencyId = config.Pool.CryptocurrencyId,
                    OwnerId = config.Pool.OwnerId,
                    Port = config.Pool.Port,
                    Tls = config.Pool.Tls
                }
            });
        }
        return result;
    }

    private static List<FlightSheetTarget> CreateFlightSheetTargets(Guid flightSheetId, Guid userId)
    {
        var minerId = Guid.Parse("55555555-5555-5555-5555-555555555555");
        return
        [
            new FlightSheetTargetBuilder()
                .WithId(Guid.Parse("33333333-3333-3333-3333-333333333333"))
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
                                        pool.WithId(Guid.Parse("99999999-9999-9999-9999-999999999999"))
                                            .WithOwner(Guid.Parse("22222222-2222-2222-2222-222222222222"))
                                            .WithCryptocurrency(cryptocurrency =>
                                                cryptocurrency.WithId(Guid.Parse("14141414-1414-1414-1414-141414141414"))
                                                              .WithOwner(Guid.Parse("22222222-2222-2222-2222-222222222222"))
                                                              .WithAlgorithm(algorithm =>
                                                                algorithm.WithId(Guid.Parse("24242424-2424-2424-2424-242424242424"))
                                                                         .WithOwner(Guid.Parse("22222222-2222-2222-2222-222222222222")))))
                                      .WithWallet(wallet =>
                                        wallet.WithId(Guid.Parse("88888888-8888-8888-8888-888888888888"))
                                            .WithOwnerId(Guid.Parse("22222222-2222-2222-2222-222222222222"))
                                            .WithCryptocurrency(cryptocurrency =>
                                                cryptocurrency.WithId(Guid.Parse("14141414-1414-1414-1414-141414141414"))
                                                            .WithOwner(Guid.Parse("22222222-2222-2222-2222-222222222222"))
                                                            .WithAlgorithm(algorithm =>
                                                              algorithm.WithId(Guid.Parse("24242424-2424-2424-2424-242424242424"))
                                                                       .WithOwner(Guid.Parse("22222222-2222-2222-2222-222222222222"))))))
                        .AddCoinConfig(coinConfig =>
                            coinConfig.WithPool(pool =>
                                pool.WithId(Guid.Parse("77777777-7777-7777-7777-777777777777"))
                                    .WithTls()
                                    .WithCryptocurrency(cryptocurrency =>
                                        cryptocurrency.WithId(Guid.Parse("33334444-3333-4444-3333-444433334444"))
                                                      .WithAlgorithm(algorithm =>
                                                        algorithm.WithId(Guid.Parse("44445555-4444-5555-4444-555544445555"))))))
                        .Build();
                })
                .Build(),
            new FlightSheetTargetBuilder()
                .WithId(Guid.Parse("44444444-4444-4444-4444-444444444444"))
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
                        coinConfig.WithId(Guid.Parse("01010101-0101-0101-0101-010101010101"))
                                  .WithPoolPassword("123456")
                                  .WithPool(pool =>
                                    pool.WithId(Guid.Parse("99999999-8888-8888-8888-999999999999"))
                                        .WithOwner(Guid.Parse("22222222-2222-2222-2222-222222222222"))
                                        .WithCryptocurrency(crypto =>
                                            crypto.WithId(Guid.Parse("41414141-1414-1414-1414-414141414141"))
                                                  .WithOwner(Guid.Parse("22222222-2222-2222-2222-222222222222"))
                                                  .WithAlgorithm(algorithm =>
                                                    algorithm.WithId(Guid.Parse("52525252-2424-2424-2424-525252525252"))
                                                             .WithOwner(Guid.Parse("22222222-2222-2222-2222-222222222222")))))
                                  .WithWallet(wallet =>
                                    wallet.WithId(Guid.Parse("88888888-7777-7777-7777-888888888888"))
                                          .WithOwnerId(Guid.Parse("22222222-2222-2222-2222-222222222222"))
                                          .WithCryptocurrency(crypto =>
                                            crypto.WithId(Guid.Parse("41414141-1414-1414-1414-414141414141"))
                                                  .WithOwner(Guid.Parse("22222222-2222-2222-2222-222222222222"))
                                                  .WithAlgorithm(algorithm =>
                                                    algorithm.WithId(Guid.Parse("52525252-2424-2424-2424-525252525252"))
                                                             .WithOwner(Guid.Parse("22222222-2222-2222-2222-222222222222"))))))
                    .Build();
                })
                .Build()
        ];
    }

    private static FlightSheetInputModel CreateFlightSheetInputModel(
        GpuMiningConfigInputModel gpuMiningConfigInputModel, CpuMiningConfigInputModel cpuMiningConfigInputModel)
    {
        return new FlightSheetInputModel()
        {
            Name = "FlightSheetName",
            Targets =
            [
                new FlightSheetTargetInputModel()
                {
                    MinerId = Guid.Parse("12121212-1212-1212-1212-121212121212"),
                    MiningConfig = gpuMiningConfigInputModel
                },
                new FlightSheetTargetInputModel()
                {
                    MinerId = Guid.Parse("21212121-2121-2121-2121-212121212121"),
                    MiningConfig = cpuMiningConfigInputModel
                }
            ]
        };
    }

    private static MiningConfigInputModel CreateMiningConfigInput(MiningDeviceType type)
    {
        switch (type)
        {
            case MiningDeviceType.GPU:
            {
                return new GpuMiningConfigInputModel()
                {
                    AdditionalArguments = "Argument1, Argument2, Argument3",
                    ConfigFileContent = "{\n\tconfigs:\n\t{\n\t\tConfig1,\n\t\tConfig2,\n\t\tConfig3\n\t}\n}",
                    CoinConfigs =
                    [
                        new MiningCoinConfigInputModel()
                        {
                            PoolId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                            WalletId = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                            PoolPassword = "123456"
                        },
                        new MiningCoinConfigInputModel()
                        {
                            PoolId = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                            WalletId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
                            PoolPassword = null
                        }
                    ]
                };
            }
            case MiningDeviceType.CPU:
            {
                return new CpuMiningConfigInputModel()
                {
                    AdditionalArguments = "Argument1, Argument2, Argument3",
                    ConfigFileContent = "{\n\tconfigs:\n\t{\n\t\tConfig1,\n\t\tConfig2,\n\t\tConfig3\n\t}\n}",
                    HugePages = 3,
                    ThreadsCount = 8,
                    CoinConfigs =
                    [
                        new MiningCoinConfigInputModel()
                        {
                            PoolId = Guid.Parse("99998888-9999-8888-9999-888899998888"),
                            WalletId = Guid.Parse("77776666-7777-6666-7777-666677776666"),
                            PoolPassword = "123456"
                        },
                        new MiningCoinConfigInputModel()
                        {
                            PoolId = Guid.Parse("55554444-5555-4444-5555-444455554444"),
                            WalletId = Guid.Parse("33332222-3333-2222-3333-222233332222"),
                            PoolPassword = null
                        }
                    ]
                };
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
                        coinConfig.WithId(Guid.Parse("22224444-2222-4444-2222-444422224444"))
                                  .WithPool(pool => pool.WithId(miningConfigInputModel.CoinConfigs[0].PoolId))
                                  .WithPoolPassword(miningConfigInputModel.CoinConfigs[0].PoolPassword)
                                  .WithWallet(wallet => wallet.WithId(miningConfigInputModel.CoinConfigs[0].WalletId)))
                    .AddCoinConfig(coinConfig =>
                        coinConfig.WithId(Guid.Parse("33335555-3333-5555-3333-555533335555"))
                                  .WithPool(pool => pool.WithId(miningConfigInputModel.CoinConfigs[1].PoolId))
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
                        coinConfig.WithId(Guid.Parse("66668888-6666-8888-6666-888866668888"))
                                  .WithPool(pool => pool.WithId(miningConfigInputModel.CoinConfigs[0].PoolId))
                                  .WithPoolPassword(miningConfigInputModel.CoinConfigs[0].PoolPassword)
                                  .WithWallet(wallet => wallet.WithId(miningConfigInputModel.CoinConfigs[0].WalletId)))
                    .AddCoinConfig(coinConfig =>
                        coinConfig.WithId(Guid.Parse("77779999-7777-9999-7777-999977779999"))
                                  .WithPool(pool => pool.WithId(miningConfigInputModel.CoinConfigs[1].PoolId))
                                  .WithPoolPassword(miningConfigInputModel.CoinConfigs[1].PoolPassword)
                                  .WithWallet(wallet => wallet.WithId(miningConfigInputModel.CoinConfigs[1].WalletId)))
                    .Build();
            }
            default:
                throw new ArgumentException($"Invalid type of {nameof(MiningConfigInputModel)}");
        }
    }
}
