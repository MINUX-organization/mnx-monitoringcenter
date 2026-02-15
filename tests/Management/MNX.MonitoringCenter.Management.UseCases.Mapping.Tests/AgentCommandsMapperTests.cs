using MNX.MonitoringCenter.Management.Agent.Commands.Mining.ApplySettings.Models;
using MNX.MonitoringCenter.Management.Contracts.MiningDevice;
using MNX.MonitoringCenter.Management.Core.Mining;
using MNX.MonitoringCenter.Management.Core.Mining.FlightSheet.Target;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Enums;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.UseCases.Mapping.AgentCommands;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Pool;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Wallet;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Events;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool;
using MNX.MonitoringCenter.Management.UseCases.Mining.Wallet;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests;

using Cryptocurrency = Core.Mining.Cryptocurrency;
using FlightSheet = Core.Mining.FlightSheet.FlightSheet;
using Miner = Core.Mining.Miner.Miner;
using Pool = Core.Mining.Pool;
using Wallet = Core.Mining.Wallet;

[TestFixture]
public sealed class AgentCommandsMapperTests
{
    private IAgentCommandsMapper _agentCommandsMapper;
    private IPoolMapper _poolMapper;
    private IWalletMapper _walletMapper;

    [SetUp]
    public void SetUp()
    {
        _poolMapper = new PoolMapper();
        _walletMapper = new WalletMapper();
        _agentCommandsMapper = new AgentCommandsMapper(_poolMapper, _walletMapper);
    }

    [TestCaseSource(typeof(MappingTestCases), nameof(MappingTestCases.DevicesTestData))]
    public void MapToWorkerSettings_ValidFlightSheetList_ReturnsWorkerSettings(List<DeviceFLightSheet> data)
    {
        // Arrange

        // Act
        var mappedData = _agentCommandsMapper.MapToWorkerSettings(data);

        // Assert
        Assert.That(mappedData, Is.Not.Null);
        Assert.That(mappedData, Is.Not.Empty);
        Assert.That(mappedData, Has.Count.EqualTo(data.Count));

        for (int i = 0; i < data.Count; i++)
        {
            var expectedFlightSheet = data[i].FlightSheet;
            var checkingSettingsModel = mappedData[i].SettingsModel;

            Assert.Multiple(() =>
            {
                Assert.That(mappedData[i], Is.Not.Null);
                Assert.That(mappedData[i].WorkerId, Is.EqualTo(data[i].Device.Id));

                Assert.That(checkingSettingsModel, Is.Not.Null);
                Assert.That(checkingSettingsModel.MinerName, Is.EqualTo(expectedFlightSheet.Miner.Name));
                Assert.That(checkingSettingsModel.MinerVersion, Is.EqualTo(expectedFlightSheet.Miner.Version));
                Assert.That(checkingSettingsModel.AdditionalArguments, Is.EqualTo(expectedFlightSheet.MiningConfig.AdditionalArguments));
                Assert.That(checkingSettingsModel.ConfigFileContent, Is.EqualTo(expectedFlightSheet.MiningConfig.ConfigFileContent));

                Assert.That(checkingSettingsModel.CoinConfigs, Is.Not.Null);
                AssertCoinConfigs(expectedFlightSheet.MiningConfig.CoinConfigs, checkingSettingsModel.CoinConfigs);
            });
        }
    }

    private static void AssertCoinConfigs(IList<MiningCoinConfig> expected, IList<MiningCoinConfigModel> checking)
    {
        Assert.That(checking, Has.Count.EqualTo(expected.Count));

        for (int i = 0; i < expected.Count; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(checking[i], Is.Not.Null);
                Assert.That(checking[i].WalletAddress, Is.EqualTo(expected[i].Wallet.Address));
                Assert.That(checking[i].AlgorithmName, Is.EqualTo(expected[i].Wallet.Cryptocurrency.Algorithm.Name));
                Assert.That(checking[i].PoolHost, Is.EqualTo(expected[i].Pool.Domain));
                Assert.That(checking[i].PoolPort, Is.EqualTo(expected[i].Pool.Port));
                Assert.That(checking[i].Tls, Is.EqualTo(expected[i].Pool.Tls));
                Assert.That(checking[i].PoolPassword, Is.EqualTo(expected[i].PoolPassword));
            });
        }
    }

    private class MappingTestCases
    {

        public static IEnumerable<List<DeviceFLightSheet>> DevicesTestData
        {
            get
            {
                var flightSheet1Id = Guid.Parse("11111111-1111-1111-1111-111111111111");
                var flightSheet2Id = Guid.Parse("22222222-2222-2222-2222-222222222222");
                var flightSheet3Id = Guid.Parse("33333333-3333-3333-3333-333333333333");
                var rigId = Guid.Parse("44444444-4444-4444-4444-444444444444");
                var minerId = Guid.Parse("55555555-5555-5555-5555-555555555555");
                var poolId = Guid.Parse("66666666-6666-6666-6666-666666666666");
                var walletId = Guid.Parse("77777777-7777-7777-7777-777777777777");
                var userId = Guid.Parse("88888888-8888-8888-8888-888888888888");
                var algorithmId = Guid.Parse("99999999-9999-9999-9999-999999999999");
                var cryptocurrencyId = Guid.Parse("00000000-0000-0000-0000-000000000000");

                yield return new List<DeviceFLightSheet>()
                {
                    new DeviceFLightSheet(
                        new MiningDeviceModel()
                        {
                            Id = Guid.Parse("01010101-0101-0101-0101-010101010101"),
                            Manufacturer = "AMD",
                            Model = "RX 6700 XT",
                            RigId = rigId,
                            Type = "GPU",
                            FlightSheetId = flightSheet1Id,
                            FlightSheetName = "FlightSheet1",
                            PresetName = "Preset1",
                            FlightSheetConfirmationState = FlightSheetConfirmationState.Unconfirmed,
                            MinerName = "Miner",
                            MinerVersion = "1.0.1"
                        },
                        new FlightSheet()
                        {
                            Id = flightSheet1Id,
                            Name = "FlightSheet1",
                            Targets =
                            [
                                new FlightSheetTarget()
                                {
                                    Id = Guid.Parse("10101010-1010-1010-1010-101010101010"),
                                    FlightSheetId = flightSheet1Id,
                                    MiningConfig = new GpuMiningConfig()
                                    {
                                        AdditionalArguments = "Argument1, Argument2",
                                        ConfigFileContent = "Config:\n{\n\tSubConfig1: 1,\n\tSubConfig2: 2\n}",
                                        CoinConfigs =
                                        [
                                            new MiningCoinConfig()
                                            {
                                                Id = Guid.Parse("11100111-1011-1011-1011-111000000111"),
                                                PoolId = poolId,
                                                Pool = CreatePool(poolId, userId, cryptocurrencyId, algorithmId),
                                                PoolPassword = "123456",
                                                WalletId = walletId,
                                                Wallet = CreateWallet(walletId, userId, cryptocurrencyId, algorithmId)
                                            }
                                        ]
                                    },
                                    MinerId = minerId,
                                    Miner = new Miner()
                                    {
                                        Id = minerId,
                                        Name = "Miner",
                                        Version = "1.0.1",
                                        InstallationUrl = "www.install.com",
                                        Type = MinerTypeEnum.Custom,
                                        SupportedDevices = DeviceTypeManufacturerCombination.AmdGpu,
                                        MiningMode = MiningModeEnum.Single,
                                        SupportedAlgorithms = []
                                    }
                                }
                            ]
                        }
                    ),
                    new DeviceFLightSheet(
                        new MiningDeviceModel()
                        {
                            Id = Guid.Parse("02020202-0202-0202-0202-020202020202"),
                            Manufacturer = "AMD",
                            Model = "RX 6600 XT",
                            RigId = rigId,
                            Type = "GPU",
                            FlightSheetId = flightSheet2Id,
                            FlightSheetName = "FlightSheet2",
                            PresetName = "Preset2",
                            FlightSheetConfirmationState = FlightSheetConfirmationState.Unconfirmed,
                            MinerName = "Miner2",
                            MinerVersion = "1.0.2"
                        },
                        new FlightSheet()
                        {
                            Id = flightSheet2Id,
                            Name = "FlightSheet2",
                            Targets =
                            [
                                new FlightSheetTarget()
                                {
                                    Id = Guid.Parse("20202020-2020-2020-2020-202020202020"),
                                    FlightSheetId = flightSheet2Id,
                                    MiningConfig = new GpuMiningConfig()
                                    {
                                        AdditionalArguments = "Argument1, Argument2",
                                        ConfigFileContent = "Config:\n{\n\tSubConfig1: 1,\n\tSubConfig2: 2\n}",
                                        CoinConfigs =
                                        [
                                            new MiningCoinConfig()
                                            {
                                                Id = Guid.Parse("11100222-1022-1022-1022-111000000222"),
                                                PoolId = poolId,
                                                Pool = CreatePool(poolId, userId, cryptocurrencyId, algorithmId),
                                                PoolPassword = "123456",
                                                WalletId = walletId,
                                                Wallet = CreateWallet(walletId, userId, cryptocurrencyId, algorithmId)
                                            },
                                            new MiningCoinConfig()
                                            {
                                                Id = Guid.Parse("22200222-2022-2022-2022-222000000222"),
                                                PoolId = poolId,
                                                Pool = CreatePool(poolId, userId, cryptocurrencyId, algorithmId),
                                                PoolPassword = "654321",
                                                WalletId = walletId,
                                                Wallet = CreateWallet(walletId, userId, cryptocurrencyId, algorithmId)
                                            }
                                        ]
                                    },
                                    MinerId = minerId,
                                    Miner = new Miner()
                                    {
                                        Id = minerId,
                                        Name = "Miner",
                                        Version = "1.0.1",
                                        InstallationUrl = "www.install.com",
                                        Type = MinerTypeEnum.Custom,
                                        SupportedDevices = DeviceTypeManufacturerCombination.AmdGpu,
                                        MiningMode = MiningModeEnum.Single,
                                        SupportedAlgorithms = []
                                    }
                                }
                            ]
                        }
                    ),
                    new DeviceFLightSheet(
                        new MiningDeviceModel()
                        {
                            Id = Guid.Parse("03030303-0303-0303-0303-030303030303"),
                            Manufacturer = "AMD",
                            Model = "RX 580",
                            RigId = rigId,
                            Type = "GPU",
                            FlightSheetId = flightSheet3Id,
                            FlightSheetName = "FlightSheet3",
                            PresetName = "Preset3",
                            FlightSheetConfirmationState = FlightSheetConfirmationState.Unconfirmed,
                            MinerName = "Miner3",
                            MinerVersion = "1.0.3"
                        },
                        new FlightSheet()
                        {
                            Id = flightSheet3Id,
                            Name = "FlightSheet3",
                            Targets =
                            [
                                new FlightSheetTarget()
                                {
                                    Id = Guid.Parse("30303030-3030-3030-3030-303030303030"),
                                    FlightSheetId = flightSheet3Id,
                                    MiningConfig = new GpuMiningConfig()
                                    {
                                        AdditionalArguments = "Argument1, Argument2",
                                        ConfigFileContent = "Config:\n{\n\tSubConfig1: 1,\n\tSubConfig2: 2\n}",
                                        CoinConfigs =
                                        [
                                            new MiningCoinConfig()
                                            {
                                                Id = Guid.Parse("11100333-1033-1033-1033-111000000333"),
                                                PoolId = poolId,
                                                Pool = CreatePool(poolId, userId, cryptocurrencyId, algorithmId),
                                                PoolPassword = "123456",
                                                WalletId = walletId,
                                                Wallet = CreateWallet(walletId, userId, cryptocurrencyId, algorithmId)
                                            },
                                            new MiningCoinConfig()
                                            {
                                                Id = Guid.Parse("22200333-2033-2033-2033-222000000333"),
                                                PoolId = poolId,
                                                Pool = CreatePool(poolId, userId, cryptocurrencyId, algorithmId),
                                                PoolPassword = "654321",
                                                WalletId = walletId,
                                                Wallet = CreateWallet(walletId, userId, cryptocurrencyId, algorithmId)
                                            },
                                            new MiningCoinConfig()
                                            {
                                                Id = Guid.Parse("33300333-3033-3033-3033-333000000333"),
                                                PoolId = poolId,
                                                Pool = CreatePool(poolId, userId, cryptocurrencyId, algorithmId),
                                                PoolPassword = "142536",
                                                WalletId = walletId,
                                                Wallet = CreateWallet(walletId, userId, cryptocurrencyId, algorithmId)
                                            }
                                        ]
                                    },
                                    MinerId = minerId,
                                    Miner = new Miner()
                                    {
                                        Id = minerId,
                                        Name = "Miner",
                                        Version = "1.0.1",
                                        InstallationUrl = "www.install.com",
                                        Type = MinerTypeEnum.Custom,
                                        SupportedDevices = DeviceTypeManufacturerCombination.AmdGpu,
                                        MiningMode = MiningModeEnum.Single,
                                        SupportedAlgorithms = []
                                    }
                                }
                            ]
                        }
                    )
                };
            }
        }

        private static Pool CreatePool(Guid poolId, Guid userId, Guid cryptocurrencyId, Guid algorithmId)
        {
            return new Pool()
            {
                Id = poolId,
                Domain = "www.pool_domain1",
                Port = 5050,
                Tls = true,
                OwnerId = userId,
                CryptocurrencyId = cryptocurrencyId,
                Cryptocurrency = new Cryptocurrency()
                {
                    Id = cryptocurrencyId,
                    ShortName = "c",
                    FullName = "Cryptocurrency",
                    OwnerId = userId,
                    AlgorithmId = algorithmId,
                    Algorithm = new Algorithm()
                    {
                        Id = algorithmId,
                        Name = "Algorithm",
                        OwnerId = userId
                    }
                },

            };

        }

        private static Wallet CreateWallet(Guid walletId, Guid userId, Guid cryptocurrencyId, Guid algorithmId)
        {
            return new Wallet()
            {
                Id = walletId,
                Name = "Wallet1",
                Address = "Address1",
                CryptocurrencyId = cryptocurrencyId,
                Cryptocurrency = new Cryptocurrency()
                {
                    Id = cryptocurrencyId,
                    ShortName = "c",
                    FullName = "Cryptocurrency",
                    OwnerId = userId,
                    AlgorithmId = algorithmId,
                    Algorithm = new Algorithm()
                    {
                        Id = algorithmId,
                        Name = "Algorithm1",
                        OwnerId = userId
                    }
                },
                OwnerId = userId
            };
        }
    }
}
