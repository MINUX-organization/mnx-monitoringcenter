using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;
using MNX.MonitoringCenter.Management.UseCases.Mapping.MiningConfig;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Pool;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Wallet;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.Models.MiningConfig;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool;
using MNX.MonitoringCenter.Management.UseCases.Mining.Wallet;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.Contracts.FlightSheet.MiningConfigs;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests;

using Pool = Core.Mining.Pool;
using Cryptocurrency = Core.Mining.Cryptocurrency;
using Algorithm = Core.Mining.Algorithm;
using Wallet = Core.Mining.Wallet;

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
        // TODO: Для генерации данных можно реализовать фабрику, чтобы было более читаемо.
        public static IEnumerable<MiningConfigInputModel> CpuMiningConfigInputModels
        {
            get
            {
                yield return new CpuMiningConfigInputModel()
                {
                    AdditionalArguments = "Argument1, Argument2, Argument3",
                    ConfigFileContent = "{\n\tcontent:\n\t{\n\t\tParam1,\n\t\tParam2,\n\t\tParam3\n\t}\n}",
                    HugePages = 3,
                    ThreadsCount = 8,
                    CoinConfigs =
                    [
                        new MiningCoinConfigInputModel()
                        {
                            PoolId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                            WalletId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                            PoolPassword = "123456"
                        },
                        new MiningCoinConfigInputModel()
                        {
                            PoolId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                            WalletId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                            PoolPassword = null
                        },
                        new MiningCoinConfigInputModel()
                        {
                            PoolId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                            WalletId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                            PoolPassword = "qwertyui"
                        }
                    ]
                };
                yield return new CpuMiningConfigInputModel()
                {
                    AdditionalArguments = "Argument43",
                    ConfigFileContent = null,
                    HugePages = 4,
                    ThreadsCount = 6,
                    CoinConfigs =
                    [
                        new MiningCoinConfigInputModel()
                        {
                            PoolId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                            WalletId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                            PoolPassword = "x"
                        }
                    ]
                };
                yield return new CpuMiningConfigInputModel()
                {
                    AdditionalArguments = null,
                    ConfigFileContent = "{\n\tcontent:\n\t{\n\t\tParam1,\n\t\tParam2,\n\t\tParam3,\n\t\tParam4,\n\t\tParam5\n\t}\n}",
                    HugePages = null,
                    ThreadsCount = null,
                    CoinConfigs = []
                };
            }
        }

        public static IEnumerable<MiningConfigInputModel> GpuMiningConfigInputModels
        {
            get
            {
                yield return new GpuMiningConfigInputModel()
                {
                    AdditionalArguments = null,
                    ConfigFileContent = "{\n\tcontent:\n\t{\n\t\tParam1,\n\t\tParam2,\n\t\tParam3,\n\t\tParam4,\n\t\tParam5\n\t}\n}",
                    CoinConfigs = []
                };
                yield return new GpuMiningConfigInputModel()
                {
                    AdditionalArguments = "Argument1, Argument2, Argument3",
                    ConfigFileContent = "{\n\tcontent:\n\t{\n\t\tParam1,\n\t\tParam2,\n\t\tParam3\n\t}\n}",
                    CoinConfigs =
                    [
                        new MiningCoinConfigInputModel()
                        {
                            PoolId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                            WalletId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                            PoolPassword = "123456"
                        },
                        new MiningCoinConfigInputModel()
                        {
                            PoolId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                            WalletId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                            PoolPassword = null
                        },
                        new MiningCoinConfigInputModel()
                        {
                            PoolId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                            WalletId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                            PoolPassword = "qwertyui"
                        }
                    ]
                };
                yield return new GpuMiningConfigInputModel()
                {
                    AdditionalArguments = "Argument43",
                    ConfigFileContent = null,
                    CoinConfigs =
                    [
                        new MiningCoinConfigInputModel()
                        {
                            PoolId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                            WalletId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                            PoolPassword = "x"
                        }
                    ]
                };
            }
        }

        public static IEnumerable<BaseMiningConfig> CpuMiningConfigs
        {
            get
            {
                yield return new CpuMiningConfig()
                {
                    HugePages = 3,
                    ThreadsCount = 5,
                    AdditionalArguments = "Argument7",
                    ConfigFileContent = "{\n\tcontent:\n\t{\n\t\tParam1,\n\t\tParam2,\n\t\tParam3,\n\t\tParam4,\n\t\tParam5\n\t}\n}",
                    CoinConfigs = 
                    [
                        new MiningCoinConfig()
                        {
                            PoolId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                            Pool = new Pool
                            {
                                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                                Domain = "www.domain.com",
                                OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                                Port = 8080,
                                Tls = true,
                                CryptocurrencyId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                                Cryptocurrency = new Cryptocurrency
                                {
                                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                                    FullName = "Crypto1",
                                    ShortName = "C1",
                                    OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                                    AlgorithmId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                                    Algorithm = new Algorithm
                                    {
                                        Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                                        Name = "Algo1",
                                        OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555")
                                    }
                                }
                            },
                            WalletId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                            Wallet = new Wallet
                            {
                                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                                Address = "Address1",
                                Name = "Wallet1",
                                CryptocurrencyId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                                Cryptocurrency = new Cryptocurrency
                                {
                                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                                    FullName = "Crypto1",
                                    ShortName = "C1",
                                    OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                                    AlgorithmId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                                    Algorithm = new Algorithm
                                    {
                                        Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                                        Name = "Algo1",
                                        OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555")
                                    }
                                },
                                OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555")
                            },
                            PoolPassword = "123456"
                        },
                        new MiningCoinConfig()
                        {
                            PoolId = Guid.Parse("11111111-1111-0000-1111-111111111111"),
                            Pool = new Pool
                            {
                                Id = Guid.Parse("11111111-1111-0000-1111-111111111111"),
                                Domain = "www.domain2.com",
                                OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                                Port = 8080,
                                Tls = true,
                                CryptocurrencyId = Guid.Parse("33333333-0000-3333-0000-333333333333"),
                                Cryptocurrency = new Cryptocurrency
                                {
                                    Id = Guid.Parse("33333333-0000-3333-0000-333333333333"),
                                    FullName = "Crypto2",
                                    ShortName = "C2",
                                    OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                                    AlgorithmId = Guid.Parse("44444444-0000-4444-0000-444444444444"),
                                    Algorithm = new Algorithm
                                    {
                                        Id = Guid.Parse("44444444-0000-4444-0000-444444444444"),
                                        Name = "Algo2",
                                        OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555")
                                    }
                                }
                            },
                            WalletId = Guid.Parse("22222222-0000-2222-0000-222222222222"),
                            Wallet = new Wallet
                            {
                                Id = Guid.Parse("22222222-0000-2222-0000-222222222222"),
                                Address = "Address2",
                                Name = "Wallet2",
                                CryptocurrencyId = Guid.Parse("33333333-0000-3333-0000-333333333333"),
                                Cryptocurrency = new Cryptocurrency
                                {
                                    Id = Guid.Parse("33333333-0000-3333-0000-333333333333"),
                                    FullName = "Crypto2",
                                    ShortName = "C2",
                                    OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                                    AlgorithmId = Guid.Parse("44444444-0000-4444-0000-444444444444"),
                                    Algorithm = new Algorithm
                                    {
                                        Id = Guid.Parse("44444444-0000-4444-0000-444444444444"),
                                        Name = "Algo2",
                                        OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555")
                                    }
                                },
                                OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555")
                            },
                            PoolPassword = null
                        },
                        new MiningCoinConfig()
                        {
                            PoolId = Guid.Parse("11111111-0000-0000-0000-111111111111"),
                            Pool = new Pool
                            {
                                Id = Guid.Parse("11111111-0000-0000-0000-111111111111"),
                                Domain = "www.domain3.com",
                                OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                                Port = 8080,
                                Tls = true,
                                CryptocurrencyId = Guid.Parse("33333333-1111-1111-1111-333333333333"),
                                Cryptocurrency = new Cryptocurrency
                                {
                                    Id = Guid.Parse("33333333-1111-1111-1111-333333333333"),
                                    FullName = "Crypto2",
                                    ShortName = "C2",
                                    OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                                    AlgorithmId = Guid.Parse("44444444-0000-4444-0000-444444444444"),
                                    Algorithm = new Algorithm
                                    {
                                        Id = Guid.Parse("44444444-0000-4444-0000-444444444444"),
                                        Name = "Algo2",
                                        OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555")
                                    }
                                }
                            },
                            WalletId = Guid.Parse("22222222-1111-1111-1111-222222222222"),
                            Wallet = new Wallet
                            {
                                Id = Guid.Parse("22222222-1111-1111-1111-222222222222"),
                                Address = "Address2",
                                Name = "Wallet2",
                                CryptocurrencyId = Guid.Parse("33333333-1111-1111-1111-333333333333"),
                                Cryptocurrency = new Cryptocurrency
                                {
                                    Id = Guid.Parse("33333333-1111-1111-1111-333333333333"),
                                    FullName = "Crypto2",
                                    ShortName = "C2",
                                    OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                                    AlgorithmId = Guid.Parse("44444444-0000-4444-0000-444444444444"),
                                    Algorithm = new Algorithm
                                    {
                                        Id = Guid.Parse("44444444-0000-4444-0000-444444444444"),
                                        Name = "Algo2",
                                        OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555")
                                    }
                                },
                                OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555")
                            },
                            PoolPassword = "qwertyui"
                        }
                    ]
                };
                yield return new CpuMiningConfig()
                {
                    HugePages = null,
                    ThreadsCount = 9,
                    AdditionalArguments = "Argument4, Argument5, Argument6",
                    ConfigFileContent = "{\n\tcontent:\n\t{\n\t\tParam1,\n\t\tParam2,\n\t\tParam3\n\t}\n}",
                    CoinConfigs = 
                    [
                        new MiningCoinConfig()
                        {
                            PoolId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                            Pool = new Pool
                            {
                                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                                Domain = "www.domain.com",
                                OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                                Port = 8080,
                                Tls = true,
                                CryptocurrencyId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                                Cryptocurrency = new Cryptocurrency
                                {
                                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                                    FullName = "Crypto1",
                                    ShortName = "C1",
                                    OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                                    AlgorithmId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                                    Algorithm = new Algorithm
                                    {
                                        Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                                        Name = "Algo1",
                                        OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555")
                                    }
                                }
                            },
                            WalletId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                            Wallet = new Wallet
                            {
                                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                                Address = "Address1",
                                Name = "Wallet1",
                                CryptocurrencyId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                                Cryptocurrency = new Cryptocurrency
                                {
                                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                                    FullName = "Crypto1",
                                    ShortName = "C1",
                                    OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                                    AlgorithmId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                                    Algorithm = new Algorithm
                                    {
                                        Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                                        Name = "Algo1",
                                        OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555")
                                    }
                                },
                                OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555")
                            },
                            PoolPassword = "123456"
                        },
                    ]
                };
                yield return new CpuMiningConfig()
                {
                    HugePages = null,
                    ThreadsCount = null,
                    AdditionalArguments = "Argument1, Argument2, Argument3",
                    ConfigFileContent = null,
                    CoinConfigs = []
                };
            }
        }

        public static IEnumerable<BaseMiningConfig> GpuMiningConfigs
        {
            get
            {
                yield return new GpuMiningConfig()
                {
                    AdditionalArguments = "Argument1, Argument2, Argument3, Argument4",
                    ConfigFileContent = "{\n\tcontent:\n\t{\n\t\tParam1,\n\t\tParam2,\n\t\tParam3,\n\t\tParam4,\n\t\tParam5\n\t}\n}",
                    CoinConfigs =
                    [
                        new MiningCoinConfig()
                        {
                            PoolId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                            Pool = new Pool
                            {
                                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                                Domain = "www.domain.com",
                                OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                                Port = 8080,
                                Tls = true,
                                CryptocurrencyId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                                Cryptocurrency = new Cryptocurrency
                                {
                                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                                    FullName = "Crypto1",
                                    ShortName = "C1",
                                    OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                                    AlgorithmId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                                    Algorithm = new Algorithm
                                    {
                                        Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                                        Name = "Algo1",
                                        OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555")
                                    }
                                }
                            },
                            WalletId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                            Wallet = new Wallet
                            {
                                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                                Address = "Address1",
                                Name = "Wallet1",
                                CryptocurrencyId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                                Cryptocurrency = new Cryptocurrency
                                {
                                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                                    FullName = "Crypto1",
                                    ShortName = "C1",
                                    OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                                    AlgorithmId = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                                    Algorithm = new Algorithm
                                    {
                                        Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                                        Name = "Algo1",
                                        OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555")
                                    }
                                },
                                OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555")
                            },
                            PoolPassword = "123456"
                        },
                        new MiningCoinConfig()
                        {
                            PoolId = Guid.Parse("11111111-0000-0000-0000-111111111111"),
                            Pool = new Pool
                            {
                                Id = Guid.Parse("11111111-0000-0000-0000-111111111111"),
                                Domain = "www.domain3.com",
                                OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                                Port = 8080,
                                Tls = true,
                                CryptocurrencyId = Guid.Parse("33333333-1111-1111-1111-333333333333"),
                                Cryptocurrency = new Cryptocurrency
                                {
                                    Id = Guid.Parse("33333333-1111-1111-1111-333333333333"),
                                    FullName = "Crypto2",
                                    ShortName = "C2",
                                    OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                                    AlgorithmId = Guid.Parse("44444444-0000-4444-0000-444444444444"),
                                    Algorithm = new Algorithm
                                    {
                                        Id = Guid.Parse("44444444-0000-4444-0000-444444444444"),
                                        Name = "Algo2",
                                        OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555")
                                    }
                                }
                            },
                            WalletId = Guid.Parse("22222222-1111-1111-1111-222222222222"),
                            Wallet = new Wallet
                            {
                                Id = Guid.Parse("22222222-1111-1111-1111-222222222222"),
                                Address = "Address2",
                                Name = "Wallet2",
                                CryptocurrencyId = Guid.Parse("33333333-1111-0000-1111-333333333333"),
                                Cryptocurrency = new Cryptocurrency
                                {
                                    Id = Guid.Parse("33333333-1111-0000-1111-333333333333"),
                                    FullName = "Crypto2",
                                    ShortName = "C2",
                                    OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                                    AlgorithmId = Guid.Parse("44444444-0000-4444-0000-444444444444"),
                                    Algorithm = new Algorithm
                                    {
                                        Id = Guid.Parse("44444444-0000-4444-0000-444444444444"),
                                        Name = "Algo2",
                                        OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555")
                                    }
                                },
                                OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555")
                            },
                            PoolPassword = "qwertyui"
                        },
                    ]
                };
                yield return new GpuMiningConfig()
                {
                    AdditionalArguments = "Argument1, Argument2, Argument3, Argument4",
                    ConfigFileContent = "{\n\tcontent:\n\t{\n\t\tParam1,\n\t\tParam2,\n\t\tParam3,\n\t\tParam4,\n\t\tParam5\n\t}\n}",
                    CoinConfigs = 
                    [
                        new MiningCoinConfig()
                        {
                            PoolId = Guid.Parse("11111111-1111-0000-1111-111111111111"),
                            Pool = new Pool
                            {
                                Id = Guid.Parse("11111111-1111-0000-1111-111111111111"),
                                Domain = "www.domain2.com",
                                OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                                Port = 8080,
                                Tls = true,
                                CryptocurrencyId = Guid.Parse("33333333-0000-3333-0000-333333333333"),
                                Cryptocurrency = new Cryptocurrency
                                {
                                    Id = Guid.Parse("33333333-0000-3333-0000-333333333333"),
                                    FullName = "Crypto2",
                                    ShortName = "C2",
                                    OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                                    AlgorithmId = Guid.Parse("44444444-0000-4444-0000-444444444444"),
                                    Algorithm = new Algorithm
                                    {
                                        Id = Guid.Parse("44444444-0000-4444-0000-444444444444"),
                                        Name = "Algo2",
                                        OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555")
                                    }
                                }
                            },
                            WalletId = Guid.Parse("22222222-0000-2222-0000-222222222222"),
                            Wallet = new Wallet
                            {
                                Id = Guid.Parse("22222222-0000-2222-0000-222222222222"),
                                Address = "Address2",
                                Name = "Wallet2",
                                CryptocurrencyId = Guid.Parse("33333333-0000-3333-0000-333333333333"),
                                Cryptocurrency = new Cryptocurrency
                                {
                                    Id = Guid.Parse("33333333-0000-3333-0000-333333333333"),
                                    FullName = "Crypto2",
                                    ShortName = "C2",
                                    OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                                    AlgorithmId = Guid.Parse("44444444-0000-4444-0000-444444444444"),
                                    Algorithm = new Algorithm
                                    {
                                        Id = Guid.Parse("44444444-0000-4444-0000-444444444444"),
                                        Name = "Algo2",
                                        OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555")
                                    }
                                },
                                OwnerId = Guid.Parse("55555555-5555-5555-5555-555555555555")
                            },
                            PoolPassword = null
                        }
                    ]
                };
                yield return new GpuMiningConfig()
                {
                    AdditionalArguments = null,
                    ConfigFileContent = null,
                    CoinConfigs = []
                };
            }
        }
    }
}
