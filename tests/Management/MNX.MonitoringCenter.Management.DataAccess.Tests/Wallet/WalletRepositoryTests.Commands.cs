using MNX.MonitoringCenter.Management.DataAccess.Cryptocurrency;
using MNX.MonitoringCenter.Management.Tests.Service.Assertions;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Wallet;

public partial class WalletRepositoryTests
{
    [Test]
    public async Task Add_ValidWallet_ShouldAddNewEntity()
    {
        // Arrange

        var walletId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var wallet = new WalletBuilder()
            .WithId(walletId)
            .WithOwnerId(userId)
            .WithCryptocurrency(crypto =>
                crypto.WithAlgorithm())
            .Build();


        // Act

        await _walletRepository.Add(wallet);
        var checkingWallet = await _walletRepository
            .GetAvailableById(walletId, userId, default);


        // Assert

        checkingWallet.ShouldBeEqualTo(wallet);
    }

    [Test]
    public async Task Remove_ValidIdAndUserId_ShouldRemoveEntity()
    {
        // Arrange

        var walletId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        await _walletRepository.Add(new WalletBuilder()
            .WithId(walletId)
            .WithOwnerId(userId)
            .WithCryptocurrency(crypto =>
                crypto.WithAlgorithm())
            .Build());


        // Act

        await _walletRepository.Remove(walletId, userId);
        var checkingWallet = await _walletRepository
            .GetAvailableById(walletId, userId, default);


        // Assert

        Assert.That(checkingWallet, Is.Null);
    }

    [Test]
    public async Task Update_ValidWallet_ShouldUpdateEntity()
    {
        // Arrange

        var cryptocurrencyRepository = new CryptocurrencyRepository(Context);

        var walletId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var newWallet = new WalletBuilder()
            .WithId(walletId)
            .WithOwnerId(userId)
            .WithName("NewWalletName_2")
            .WithAddress("NewWalletAddress_2")
            .WithCryptocurrency(crypto =>
                crypto.WithAlgorithm())
            .Build();

        await _walletRepository.Add(new WalletBuilder()
            .WithId(walletId)
            .WithOwnerId(userId)
            .WithName("OldWalletName_1")
            .WithAddress("OldWalletAddress_1")
            .WithCryptocurrency(crypto =>
                crypto.WithId(newWallet.CryptocurrencyId)
                      .WithAlgorithm(algo =>
                        algo.WithId(newWallet.Cryptocurrency.AlgorithmId)))
            .Build());

        ClearChangeTracker();


        // Act

        await _walletRepository.Update(newWallet);
        var checkingWallet = await _walletRepository
            .GetAvailableById(walletId, userId, default);


        // Assert

        checkingWallet.ShouldBeEqualTo(newWallet);
    }
}
