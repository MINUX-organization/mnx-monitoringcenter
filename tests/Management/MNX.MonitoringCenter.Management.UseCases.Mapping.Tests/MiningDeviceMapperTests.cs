using MNX.MonitoringCenter.Management.Core.Mining;
using MNX.MonitoringCenter.Management.Core.Mining.FlightSheet.Target;
using MNX.MonitoringCenter.Management.Core.Mining.Miner;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Enums;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.UseCases.Mapping.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.Core.Overclocking.Enums;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests;

using Cryptocurrency = Core.Mining.Cryptocurrency;
using Miner = Core.Mining.Miner.Miner;
using Wallet = Core.Mining.Wallet;

[TestFixture]
public sealed class MiningDeviceMapperTests
{
    private IMiningDeviceMapper _miningDeviceMapper;

    [SetUp]
    public void SetUp()
    {
        _miningDeviceMapper = new MiningDeviceMapper();
    }

    [Test]
    public void MapToModel_ValidMiningDeviceInfo_ReturnMiningDeviceModel()
    {
        // Arrange

        var checkingMinerName = "Miner1";
        var checkingMinerVersion = "1.0.1";
        var deviceInfo = new MiningDeviceInfo()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-111111111111"),
            Manufacturer = "Nvidia",
            Model = "RTX 4060 ti",
            RigId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            OwnerId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            FlightSheetId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            FlightSheetConfirmationState = FlightSheetConfirmationState.Unconfirmed,
            FlightSheet = new()
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Name = "FlightSheetName",
                OwnerId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Targets =
                [
                    new FlightSheetTarget()
                    {
                        Id = Guid.Parse("10101010-1010-1010-1010-101010101010"),
                        FlightSheetId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                        MiningConfig = new GpuMiningConfig()
                        {
                            ConfigFileContent = "content",
                            AdditionalArguments = "arguments",
                            CoinConfigs =
                            [
                                new MiningCoinConfig
                                {
                                    Id = Guid.Parse("30303030-3030-3030-3030-303030303030"),
                                    WalletId = Guid.Parse("40404040-4040-4040-4040-404040404040"),
                                    Wallet = new Wallet
                                    {
                                        Id = Guid.Parse("40404040-4040-4040-4040-404040404040"),
                                        Name = "Wallet1",
                                        Address = "Address1",
                                        CryptocurrencyId = Guid.Parse("50505050-5050-5050-5050-505050505050"),
                                        Cryptocurrency = new Cryptocurrency
                                        {
                                            Id = Guid.Parse("50505050-5050-5050-5050-505050505050"),
                                            FullName = "Crypto1",
                                            ShortName = "C1",
                                            AlgorithmId = Guid.Parse("01010101-0101-0101-0101-010101010101"),
                                            Algorithm = new Algorithm
                                            {
                                                Id = Guid.Parse("01010101-0101-0101-0101-010101010101"),
                                                Name = "Algotithm1",
                                                OwnerId = null
                                            }
                                        },
                                        OwnerId = Guid.Parse("22222222-2222-2222-2222-222222222222")
                                    }
                                }
                            ]
                        },
                        MinerId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                        Miner = new Miner()
                        {
                            Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                            Name = checkingMinerName,
                            InstallationUrl = "www.install.com",
                            Version = checkingMinerVersion,
                            OwnerId = null,
                            Type = MinerTypeEnum.Integrated,
                            MiningMode = MiningModeEnum.Triple,
                            SupportedAlgorithms =
                            [
                                new MinerAlgorithm()
                                {
                                    AlgorithmId = Guid.Parse("01010101-0101-0101-0101-010101010101"),
                                    MinerId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                                    Name = "Algotithm1"
                                }
                            ],
                            SupportedDevices = DeviceTypeManufacturerCombination.NvidiaGpu,
                        }
                    },
                    new FlightSheetTarget()
                    {
                        Id = Guid.Parse("20202020-2020-2020-2020-202020202020"),
                        FlightSheetId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                        MiningConfig = new GpuMiningConfig()
                        {
                            ConfigFileContent = "content",
                            AdditionalArguments = "arguments",
                            CoinConfigs =
                            [
                                new MiningCoinConfig
                                {
                                    Id = Guid.Parse("03030303-0303-0303-0303-030303030303"),
                                    WalletId = Guid.Parse("44404440-4440-4440-4440-444044404440"),
                                    Wallet = new Wallet
                                    {
                                        Id = Guid.Parse("44404440-4440-4440-4440-444044404440"),
                                        Name = "Wallet2",
                                        Address = "Address2",
                                        CryptocurrencyId = Guid.Parse("55505550-5550-5550-5550-555055505550"),
                                        Cryptocurrency = new Cryptocurrency
                                        {
                                            Id = Guid.Parse("55505550-5550-5550-5550-555055505550"),
                                            FullName = "Crypto2",
                                            ShortName = "C2",
                                            AlgorithmId = Guid.Parse("02220222-0222-0222-0222-022202220222"),
                                            Algorithm = new Algorithm
                                            {
                                                Id = Guid.Parse("02220222-0222-0222-0222-022202220222"),
                                                Name = "Algotithm2",
                                                OwnerId = null
                                            }
                                        },
                                        OwnerId = Guid.Parse("22222222-2222-2222-2222-222222222222")
                                    }
                                }
                            ]
                        },
                        MinerId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                        Miner = new Miner()
                        {
                            Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                            Name = "Miner2",
                            InstallationUrl = "www.install2.com",
                            Version = "1.0.2",
                            OwnerId = null,
                            Type = MinerTypeEnum.Integrated,
                            MiningMode = MiningModeEnum.Dual,
                            SupportedAlgorithms =
                            [
                                new MinerAlgorithm()
                                {
                                    AlgorithmId = Guid.Parse("02220222-0222-0222-0222-022202220222"),
                                    MinerId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                                    Name = "Algotithm2"
                                }
                            ],
                            SupportedDevices = DeviceTypeManufacturerCombination.NvidiaGpu,
                        }
                    }
                ]
            },
            Type = MiningDeviceType.GPU,
            LifeCycleStatus = MiningDeviceLifeCycleStatus.Offline,
            PresetId = Guid.Parse("66666666-6666-6666-6666-666666666666"),
            Preset = new()
            {
                Id = Guid.Parse("77777777-7777-7777-7777-777777777777"),
                DeviceName = "Nvidia RTX 4060 ti",
                Name = "Preset1",
                IsVisible = true,
                OwnerId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                OverclockingId = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                Overclocking = new NvidiaGpuOverclocking()
                {
                    Id = Guid.Parse("88888888-8888-8888-8888-888888888888"),
                    FanOverclocking = new FanOverclockingWithLinearDependence()
                    {
                        Id = Guid.Parse("99999999-9999-9999-9999-999999999999"),
                        TargetPoints =
                        [
                            new FanGraphicPoint()
                            {
                                PointIndex = 0,
                                FanSpeedValueTarget = 20,
                                TemperatureValueTarget = 35,
                            },
                            new FanGraphicPoint()
                            {
                                PointIndex = 1,
                                FanSpeedValueTarget = 50,
                                TemperatureValueTarget = 65,
                            },
                            new FanGraphicPoint()
                            {
                                PointIndex = 2,
                                FanSpeedValueTarget = 100,
                                TemperatureValueTarget = 80,
                            }
                        ]
                    },
                    CoreClockLock = 0,
                    CoreClockOffset = 250,
                    CoreVoltage = 0,
                    CoreVoltageOffset = 0,
                    MemoryClockLock = 0,
                    MemoryClockOffset = 2500,
                    MemoryVoltage = 0,
                    MemoryVoltageOffset = 0,
                    PowerLimit = 108
                }
            }
        };


        // Act

        var mappedModel = _miningDeviceMapper.MapToModel(deviceInfo);


        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(mappedModel, Is.Not.Null);
            Assert.That(mappedModel.Id, Is.EqualTo(deviceInfo.Id));
            Assert.That(mappedModel.Manufacturer, Is.EqualTo(deviceInfo.Manufacturer));
            Assert.That(mappedModel.Model, Is.EqualTo(deviceInfo.Model));
            Assert.That(mappedModel.Type, Is.EqualTo(deviceInfo.Type.ToString()));
            Assert.That(mappedModel.RigId, Is.EqualTo(deviceInfo.RigId));
            Assert.That(mappedModel.FlightSheetId, Is.EqualTo(deviceInfo.FlightSheetId));
            Assert.That(mappedModel.FlightSheetName, Is.EqualTo(deviceInfo.FlightSheet.Name));
            Assert.That(mappedModel.FlightSheetConfirmationState, Is.EqualTo(deviceInfo.FlightSheetConfirmationState));
            Assert.That(mappedModel.PresetName, Is.EqualTo(deviceInfo.Preset.Name));
            Assert.That(mappedModel.MinerName, Is.EqualTo(checkingMinerName));
            Assert.That(mappedModel.MinerVersion, Is.EqualTo(checkingMinerVersion));
            Assert.That(mappedModel.IsOnline, Is.EqualTo(deviceInfo.IsOnline));
        });
    }
}
