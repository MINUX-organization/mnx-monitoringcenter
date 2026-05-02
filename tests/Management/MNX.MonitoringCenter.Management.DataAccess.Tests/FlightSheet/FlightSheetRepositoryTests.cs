using AutoMapper;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.DataAccess.Cryptocurrency;
using MNX.MonitoringCenter.Management.DataAccess.FlightSheet;
using MNX.MonitoringCenter.Management.DataAccess.Mapping;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.MiningConfigs;
using MNX.MonitoringCenter.Management.UseCases.Mining.Cryptocurrency;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.FlightSheet;

using FlightSheet = Core.Mining.FlightSheet.FlightSheet;

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

    private async Task PrepareDataBase(List<FlightSheet> data)
    {
        foreach (var item in data)
        {
            await _flightSheetRepository.Add(item);
        }
    }

    private async Task PrepareDataBase(FlightSheet data)
    {
        var cryptocurrencies = data.Targets
            .SelectMany(x => x.MiningConfig.CoinConfigs
                .SelectMany(x => new[]
                {
                    x.Wallet?.Cryptocurrency,
                    x.Pool?.Cryptocurrency,
                }))
            .ToList();

        foreach (var cryptocurrency in cryptocurrencies)
        {

            if (cryptocurrency is not null &&
                cryptocurrency.Algorithm is not null)
            {
                await _cryptocurrencyRepository.Add(cryptocurrency);
            }
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
