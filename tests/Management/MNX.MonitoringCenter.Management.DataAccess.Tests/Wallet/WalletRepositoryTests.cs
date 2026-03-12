using MNX.MonitoringCenter.Management.DataAccess.Wallet;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;
using MNX.MonitoringCenter.Management.UseCases.Mining.Wallet;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Wallet;

using Wallet = Core.Mining.Wallet;

public partial class WalletRepositoryTests : BaseTest
{
    private IWalletRepository _walletRepository;

    [SetUp]
    public void SetUp()
    {
        _walletRepository = new WalletRepository(Context);
    }

    private static class WalletsTestCaseSource
    {
        public static Guid UserId = Guid.NewGuid();

        public static IEnumerable<List<Wallet>> WalletLists
        {
            get
            {
                yield return
                [
                    new WalletBuilder()
                        .WithOwnerId(UserId)
                        .WithCryptocurrency(crypto =>
                            crypto.WithAlgorithm())
                        .Build(),
                    new WalletBuilder()
                        .WithOwnerId(UserId)
                        .WithCryptocurrency(crypto =>
                            crypto.WithAlgorithm())
                        .Build(),
                    new WalletBuilder()
                        .WithCryptocurrency(crypto =>
                            crypto.WithAlgorithm())
                        .Build(),
                ];
            }
        }
    }
}
