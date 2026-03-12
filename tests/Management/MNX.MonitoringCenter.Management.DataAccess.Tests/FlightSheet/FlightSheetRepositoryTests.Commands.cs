using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.MiningConfigs;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.FlightSheet;

using FlightSheet = Core.Mining.FlightSheet.FlightSheet;

public partial class FlightSheetRepositoryTests
{
    [TestCaseSource(typeof(FlightSheetsTestCaseSource), nameof(FlightSheetsTestCaseSource.FlightSheets))]
    public async Task Add_ValidFlightSheet_ShouldAddEntity(FlightSheet data)
    {
        // Arrange

        var flightSheetId = data.Id;
        var userId = FlightSheetsTestCaseSource.UserId;
        await PrepareDataBase(data);


        // Act

        await _flightSheetRepository.Add(data);
        var checkingFlightSheet = await _flightSheetRepository
            .GetAvailableById(flightSheetId, userId, default);


        // Assert

        Assert.That(checkingFlightSheet, Is.Not.Null);
        AssertFlightSheet(data, checkingFlightSheet);
    }

    [TestCaseSource(typeof(FlightSheetsTestCaseSource), nameof(FlightSheetsTestCaseSource.FlightSheets))]
    public async Task Edit_ValidFlightSheet_ShouldEditEntity(FlightSheet data)
    {
        // Arrange

        var flightSheetId = data.Id;
        var userId = FlightSheetsTestCaseSource.UserId;

        var cryptocurrency = new CryptocurrencyBuilder()
            .WithAlgorithm()
            .Build();

        var newFlightSheet = new FlightSheetBuilder()
            .WithId(flightSheetId)
            .WithOwnerId(userId)
            .WithName("NewFlightSheet")
            .AddTarget(target =>
                target.WithMiner(miner =>
                    miner.WithSupportedDevices(DeviceTypeManufacturerCombination.NvidiaGpu))
                         .WithMiningConfig(() =>
                         {
                             return new GpuMiningConfigBuilder()
                                .AddCoinConfig(coin =>
                                    coin.WithPool(pool =>
                                        pool.WithCryptocurrency(cryptocurrency))
                                        .WithWallet(wallet =>
                                        wallet.WithCryptocurrency(cryptocurrency)))
                                .Build();
                         }))
            .Build();

        await PrepareDataBase(data);
        await _flightSheetRepository.Add(data);
        ClearChangeTracker();


        // Act

        await _flightSheetRepository.Edit(newFlightSheet);
        var checkingFlightSheet = await _flightSheetRepository
            .GetAvailableById(flightSheetId, userId, default);


        // Assert

        Assert.That(checkingFlightSheet, Is.Not.Null);
        AssertFlightSheet(newFlightSheet, checkingFlightSheet);
    }

    [TestCaseSource(typeof(FlightSheetsTestCaseSource), nameof(FlightSheetsTestCaseSource.FlightSheets))]
    public async Task Remove_ValidFlightSheet_ShouldRemoveEntity(FlightSheet data)
    {
        // Arrange

        var flightSheetId = data.Id;
        var userId = FlightSheetsTestCaseSource.UserId;

        await PrepareDataBase(data);
        await _flightSheetRepository.Add(data);


        // Act

        await _flightSheetRepository.Remove(flightSheetId, userId);
        var checkingFlightSheet = await _flightSheetRepository
            .GetAvailableById(flightSheetId, userId, default);


        // Assert

        Assert.That(checkingFlightSheet, Is.Null);
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
}
