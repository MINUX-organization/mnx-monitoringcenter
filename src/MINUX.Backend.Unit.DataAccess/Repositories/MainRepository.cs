using MINUX.Backend.Unit.UseCases.Abstractions;

namespace MINUX.Backend.Unit.DataAccess.Repositories;

public class MainRepository : IMainRepository
{
    private readonly Context _context;

    public ICryptocurrencyRepository Cryptocurrencies { get; }

    public IFlightSheetRepository FlightSheets { get; }

    public IPoolRepository Pools { get; }

    public IPresetRepositoty Presets { get; }

    public IWalletRepository Wallets { get; }

    public IAlgorithmRepository Algorithms { get; }

    public IMinerRepository Miners {  get; }

    public MainRepository(Context context)
    {
        _context = context;
        Cryptocurrencies = new CryptocurrencyRepository(context);
        FlightSheets = new FlightSheetRepository(context);
        Pools = new PoolRepository(context);
        Presets = new PresetRepository(context);
        Wallets = new WalletRepository(context);
        Algorithms = new AlgorithmRepository(context);
        Miners = new MinerRepository(context);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
