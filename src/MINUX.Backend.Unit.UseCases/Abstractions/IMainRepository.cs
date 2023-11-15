namespace MINUX.Backend.Unit.UseCases.Abstractions;

public interface IMainRepository
{
    ICryptocurrencyRepository Cryptocurrencies { get; }

    IFlightSheetRepository FlightSheets { get; }

    IPoolRepository Pools { get; }

    IPresetRepositoty Presets { get; }

    IWalletRepository Wallets { get; }

    IAlgorithmRepository Algorithms { get; }

    IMinerRepository Miners { get; }

    Task SaveChangesAsync();
}