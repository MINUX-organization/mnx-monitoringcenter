using MNX.MonitoringCenter.Management.UseCases.Commands.Crypto;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Crypto;

public static class CryptocurrencyCommandTestCase
{
    public static IEnumerable<CryptocurrencyInputModel> CreateCorrectInputModel()
    {
        yield return new CryptocurrencyInputModel("SOL", "Solana", "algorithm");
    }
    
    public static IEnumerable<CryptocurrencyInputModel> CreateIncorrectInputModel()
    {
        yield return new CryptocurrencyInputModel("N", "N", "algorithm");
    }
}
