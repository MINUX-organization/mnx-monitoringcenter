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

    private async Task PrepareDataBase(List<Wallet> data)
    {
        foreach (var item in data)
        {
            await _walletRepository.Add(item);
        }
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

        public static IEnumerable<Wallet> Wallets
        {
            get
            {
                yield return new WalletBuilder()
                    .WithOwnerId(UserId)
                    .WithCryptocurrency(crypto => crypto.WithAlgorithm().WithOwner(UserId))
                    .Build();
                yield return new WalletBuilder()
                    .WithOwnerId(UserId)
                    .WithCryptocurrency(crypto => crypto.WithAlgorithm(algo => algo.WithOwner(UserId)).WithOwner(UserId))
                    .Build();
                yield return new WalletBuilder()
                    .WithOwnerId(UserId)
                    .WithCryptocurrency()
                    .Build();
            }
        }
    }
}
