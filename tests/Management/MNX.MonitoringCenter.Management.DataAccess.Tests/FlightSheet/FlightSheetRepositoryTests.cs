using AutoMapper;
using MNX.MonitoringCenter.Management.Core.Mining.FlightSheet.Target;
using MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.DataAccess.Cryptocurrency;
using MNX.MonitoringCenter.Management.DataAccess.FlightSheet;
using MNX.MonitoringCenter.Management.DataAccess.Mapping;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.MiningConfigs;
using MNX.MonitoringCenter.Management.UseCases.Mining.Cryptocurrency;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.FlightSheet;

using Algorithm = Core.Mining.Algorithm;
using Cryptocurrency = Core.Mining.Cryptocurrency;
using FlightSheet = Core.Mining.FlightSheet.FlightSheet;
using Miner = Core.Mining.Miner.Miner;
using Pool = Core.Mining.Pool;
using Wallet = Core.Mining.Wallet;

public partial class FlightSheetRepositoryTests : BaseTest
{
    private IMapper _mapper;
    private IFlightSheetRepository _flightSheetRepository;
    private ICryptocurrencyRepository _cryptocurrencyRepository;

    [SetUp]
    public void SetUp()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new DbMappingProfile());
        });
        _mapper = config.CreateMapper();
        _flightSheetRepository = new FlightSheetRepository(Context, _mapper);
        _cryptocurrencyRepository = new CryptocurrencyRepository(Context);
    }

    private static void AssertFlightSheet(FlightSheet expected, FlightSheet checking)
    {
        Assert.Multiple(() =>
        {
            Assert.Multiple(() =>
            {
                Assert.That(checking.Id, Is.EqualTo(expected.Id));
                Assert.That(checking.OwnerId, Is.EqualTo(expected.OwnerId));
                Assert.That(checking.Name, Is.EqualTo(expected.Name));
                AssertTargets(expected.Targets, checking.Targets);
            });
        });
    }

    private static void AssertTargets(List<FlightSheetTarget> expected, List<FlightSheetTarget> checking)
    {
        expected = [.. expected.OrderBy(x => x.Id)];
        checking = [.. checking.OrderBy(x => x.Id)];

        for (var i = 0; i < expected.Count; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(checking[i].Id, Is.EqualTo(expected[i].Id));
                Assert.That(checking[i].FlightSheetId, Is.EqualTo(expected[i].FlightSheetId));
                Assert.That(checking[i].MinerId, Is.EqualTo(expected[i].MinerId));
                Assert.That(checking[i].DeviceType, Is.EqualTo(expected[i].DeviceType));

                AssertMiningConfig(expected[i].MiningConfig, checking[i].MiningConfig);
                AssertMiner(expected[i].Miner, checking[i].Miner);
            });
        }

        void AssertMiningConfig(BaseMiningConfig expected, BaseMiningConfig checking)
        {
            Assert.Multiple(() =>
            {
                Assert.That(checking.DeviceType, Is.EqualTo(expected.DeviceType));
                Assert.That(checking.AdditionalArguments, Is.EqualTo(expected.AdditionalArguments));
                Assert.That(checking.ConfigFileContent, Is.EqualTo(expected.ConfigFileContent));
                AssertCoinConfigs(expected.CoinConfigs, checking.CoinConfigs);

                switch (expected, checking)
                {
                    case (GpuMiningConfig e, GpuMiningConfig c):
                        AssertGpuMiningConfig(e, c);
                        break;

                    case (CpuMiningConfig e, CpuMiningConfig c):
                        AssertCpuMiningConfig(e, c);
                        break;
                }
            });

            void AssertCoinConfigs(List<MiningCoinConfig> expected, List<MiningCoinConfig> checking)
            {
                expected = [.. expected.OrderBy(x => x.Id)];
                checking = [.. checking.OrderBy(x => x.Id)];

                for (var i = 0; i < expected.Count; i++)
                {
                    Assert.Multiple(() =>
                    {
                        Assert.That(checking[i].Id, Is.EqualTo(expected[i].Id));
                        Assert.That(checking[i].PoolId, Is.EqualTo(expected[i].PoolId));
                        Assert.That(checking[i].WalletId, Is.EqualTo(expected[i].WalletId));
                        Assert.That(checking[i].PoolPassword, Is.EqualTo(expected[i].PoolPassword));
                        AssertPool(expected[i].Pool, checking[i].Pool);
                        AssertWallet(expected[i].Wallet, checking[i].Wallet);
                    });
                }

                void AssertPool(Pool? expected, Pool? checking)
                {
                    Assert.Multiple(() =>
                    {
                        Assert.That(checking?.Id, Is.EqualTo(expected?.Id));
                        Assert.That(checking?.CryptocurrencyId, Is.EqualTo(expected?.CryptocurrencyId));
                        Assert.That(checking?.OwnerId, Is.EqualTo(expected?.OwnerId));
                        Assert.That(checking?.Domain, Is.EqualTo(expected?.Domain));
                        Assert.That(checking?.Port, Is.EqualTo(expected?.Port));
                        Assert.That(checking?.Tls, Is.EqualTo(expected?.Tls));
                        AssertCryptocurrency(expected?.Cryptocurrency, checking?.Cryptocurrency);
                    });
                }

                void AssertWallet(Wallet? expected, Wallet? checking)
                {
                    Assert.Multiple(() =>
                    {
                        Assert.That(checking?.Id, Is.EqualTo(expected?.Id));
                        Assert.That(checking?.CryptocurrencyId, Is.EqualTo(expected?.CryptocurrencyId));
                        Assert.That(checking?.OwnerId, Is.EqualTo(expected?.OwnerId));
                        Assert.That(checking?.Name, Is.EqualTo(expected?.Name));
                        Assert.That(checking?.Address, Is.EqualTo(expected?.Address));
                        AssertCryptocurrency(expected?.Cryptocurrency, checking?.Cryptocurrency);
                    });
                }

                void AssertCryptocurrency(Cryptocurrency? expected, Cryptocurrency? checking)
                {
                    Assert.Multiple(() =>
                    {
                        Assert.That(checking?.Id, Is.EqualTo(expected?.Id));
                        Assert.That(checking?.OwnerId, Is.EqualTo(expected?.OwnerId));
                        Assert.That(checking?.AlgorithmId, Is.EqualTo(expected?.AlgorithmId));
                        Assert.That(checking?.FullName, Is.EqualTo(expected?.FullName));
                        Assert.That(checking?.ShortName, Is.EqualTo(expected?.ShortName));
                        AssertAlgorithm(expected?.Algorithm, checking?.Algorithm);
                    });

                    void AssertAlgorithm(Algorithm? expected, Algorithm? checking)
                    {
                        Assert.Multiple(() =>
                        {
                            Assert.That(checking?.Id, Is.EqualTo(expected?.Id));
                            Assert.That(checking?.OwnerId, Is.EqualTo(expected?.OwnerId));
                            Assert.That(checking?.Name, Is.EqualTo(expected?.Name));
                        });
                    }
                }
            }

            void AssertCpuMiningConfig(CpuMiningConfig expected, CpuMiningConfig checking)
            {
                Assert.Multiple(() =>
                {
                    Assert.That(checking.ThreadsCount, Is.EqualTo(expected.ThreadsCount));
                    Assert.That(checking.HugePages, Is.EqualTo(expected.HugePages));
                });
            }

            void AssertGpuMiningConfig(GpuMiningConfig expected, GpuMiningConfig checking)
            {
                // Дополнительных полей нет.
            }
        }

        void AssertMiner(Miner? expected, Miner? checking)
        {
            Assert.Multiple(() =>
            {
                Assert.That(checking?.Id, Is.EqualTo(expected?.Id));
                Assert.That(checking?.Name, Is.EqualTo(expected?.Name));
                Assert.That(checking?.Version, Is.EqualTo(expected?.Version));
                Assert.That(checking?.InstallationUrl, Is.EqualTo(expected?.InstallationUrl));
                Assert.That(checking?.Type, Is.EqualTo(expected?.Type));
                Assert.That(checking?.SupportedDevices, Is.EqualTo(expected?.SupportedDevices));
                Assert.That(checking?.MiningMode, Is.EqualTo(expected?.MiningMode));
                Assert.That(checking?.OwnerId, Is.EqualTo(expected?.OwnerId));
                Assert.That(checking?.PoolTemplate, Is.EqualTo(expected?.PoolTemplate));
                Assert.That(checking?.WalletWorkerTemplate, Is.EqualTo(expected?.WalletWorkerTemplate));
            });
        }
    }

    private static class FlightSheetsTestCaseSource
    {
        public static Guid UserId = Guid.NewGuid();

        public static IEnumerable<List<FlightSheet>> FlightSheetLists
        {
            get
            {
                yield return
                [
                    new FlightSheetBuilder()
                        .WithOwnerId(UserId)
                        .AddTarget(target =>
                            target.WithMiner(miner => 
                                miner.WithSupportedDevices(DeviceTypeManufacturerCombination.NvidiaGpu))
                                  .WithMiningConfig(() =>
                                  {
                                      return new GpuMiningConfigBuilder()
                                          .AddCoinConfig(coinConfig =>
                                            coinConfig.WithPool(pool =>
                                                pool.WithCryptocurrency(crypto => crypto.WithAlgorithm()))
                                                      .WithWallet(wallet =>
                                                        wallet.WithOwnerId(UserId)
                                                              .WithCryptocurrency(crypto => crypto.WithAlgorithm())))
                                          .Build();
                                  }))
                        .Build(),
                    new FlightSheetBuilder()
                        .WithOwnerId(UserId)
                        .AddTarget(target =>
                            target.WithMiner(miner =>
                                miner.WithSupportedDevices(DeviceTypeManufacturerCombination.AmdCpu))
                                  .WithMiningConfig(() =>
                                  {
                                      return new CpuMiningConfigBuilder()
                                          .AddCoinConfig(coinConfig =>
                                            coinConfig.WithPool(pool =>
                                                pool.WithCryptocurrency(crypto => crypto.WithAlgorithm()))
                                                      .WithWallet(wallet =>
                                                        wallet.WithOwnerId(UserId)
                                                              .WithCryptocurrency(crypto => crypto.WithAlgorithm())))
                                          .Build();
                                  }))
                        .Build(),
                    new FlightSheetBuilder()
                        .WithOwnerId(UserId)
                        .AddTarget(target =>
                            target.WithMiner(miner =>
                                miner.WithSupportedDevices(DeviceTypeManufacturerCombination.NvidiaGpu))
                                  .WithMiningConfig(() =>
                                  {
                                      return new GpuMiningConfigBuilder()
                                          .AddCoinConfig(coinConfig =>
                                            coinConfig.WithPool(pool =>
                                                pool.WithCryptocurrency(crypto => crypto.WithAlgorithm()))
                                                      .WithWallet(wallet =>
                                                        wallet.WithOwnerId(UserId)
                                                              .WithCryptocurrency(crypto => crypto.WithAlgorithm())))
                                          .Build();
                                  }))
                        .AddTarget(target =>
                            target.WithMiner(miner =>
                                miner.WithSupportedDevices(DeviceTypeManufacturerCombination.AmdCpu))
                                  .WithMiningConfig(() =>
                                  {
                                      return new CpuMiningConfigBuilder()
                                          .AddCoinConfig(coinConfig =>
                                            coinConfig.WithPool(pool =>
                                                pool.WithCryptocurrency(crypto => crypto.WithAlgorithm()))
                                                      .WithWallet(wallet =>
                                                        wallet.WithOwnerId(UserId)
                                                              .WithCryptocurrency(crypto => crypto.WithAlgorithm())))
                                          .Build();
                                  }))
                        .Build(),
                    new FlightSheetBuilder()
                        .WithOwnerId(UserId)
                        .Build(),
                ];
            }
        }

        public static IEnumerable<FlightSheet> FlightSheets
        {
            get
            {
                yield return new FlightSheetBuilder()
                    .WithOwnerId(UserId)
                    .AddTarget(target =>
                            target.WithMiner(miner =>
                                miner.WithSupportedDevices(DeviceTypeManufacturerCombination.NvidiaGpu))
                                  .WithMiningConfig(() =>
                                  {
                                      return new GpuMiningConfigBuilder()
                                          .AddCoinConfig(coinConfig =>
                                            coinConfig.WithPool(pool =>
                                                pool.WithCryptocurrency(crypto => crypto.WithAlgorithm()))
                                                      .WithWallet(wallet =>
                                                        wallet.WithOwnerId(UserId)
                                                              .WithCryptocurrency(crypto => crypto.WithAlgorithm())))
                                          .Build();
                                  }))
                    .Build();
                yield return new FlightSheetBuilder()
                    .WithOwnerId(UserId)
                    .AddTarget(target =>
                            target.WithMiner(miner =>
                                miner.WithSupportedDevices(DeviceTypeManufacturerCombination.AmdCpu))
                                  .WithMiningConfig(() =>
                                  {
                                      return new CpuMiningConfigBuilder()
                                          .AddCoinConfig(coinConfig =>
                                            coinConfig.WithPool(pool =>
                                                pool.WithCryptocurrency(crypto => crypto.WithAlgorithm()))
                                                      .WithWallet(wallet =>
                                                        wallet.WithOwnerId(UserId)
                                                              .WithCryptocurrency(crypto => crypto.WithAlgorithm())))
                                          .Build();
                                  }))
                    .Build();
                yield return new FlightSheetBuilder()
                    .WithOwnerId(UserId)
                    .AddTarget(target =>
                            target.WithMiner(miner =>
                                miner.WithSupportedDevices(DeviceTypeManufacturerCombination.NvidiaGpu))
                                  .WithMiningConfig(() =>
                                  {
                                      return new GpuMiningConfigBuilder()
                                          .AddCoinConfig(coinConfig =>
                                            coinConfig.WithPool(pool =>
                                                pool.WithCryptocurrency(crypto => crypto.WithAlgorithm()))
                                                      .WithWallet(wallet =>
                                                        wallet.WithOwnerId(UserId)
                                                              .WithCryptocurrency(crypto => crypto.WithAlgorithm())))
                                          .Build();
                                  }))
                        .AddTarget(target =>
                            target.WithMiner(miner =>
                                miner.WithSupportedDevices(DeviceTypeManufacturerCombination.AmdCpu))
                                  .WithMiningConfig(() =>
                                  {
                                      return new CpuMiningConfigBuilder()
                                          .AddCoinConfig(coinConfig =>
                                            coinConfig.WithPool(pool =>
                                                pool.WithCryptocurrency(crypto => crypto.WithAlgorithm()))
                                                      .WithWallet(wallet =>
                                                        wallet.WithOwnerId(UserId)
                                                              .WithCryptocurrency(crypto => crypto.WithAlgorithm())))
                                          .Build();
                                  }))
                    .Build();
            }
        }
    }
}
