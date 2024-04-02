using MNX.MonitoringCenter.Management.UseCases.Commands.Wallets;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Wallets;

public class WalletCommandTestCase
{
    public static IEnumerable<WalletInputModel> CreateCorrectWalletModel()
    {
        yield return new WalletInputModel("name", "address", Guid.NewGuid());
    }

    public static IEnumerable<WalletInputModel> CreateIncorrectWalletModel()
    {
        yield return new WalletInputModel("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", 
            "zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz", Guid.Empty);
    }
}
