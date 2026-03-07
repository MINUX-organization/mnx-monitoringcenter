using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.DataAccess.Cryptocurrency;
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

        Assert.That(checkingWallet, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(checkingWallet.Id, Is.EqualTo(walletId));
            Assert.That(checkingWallet.OwnerId, Is.EqualTo(userId));
            Assert.That(checkingWallet.Name, Is.EqualTo(wallet.Name));
            Assert.That(checkingWallet.Address, Is.EqualTo(wallet.Address));
            Assert.That(checkingWallet.CryptocurrencyId, Is.EqualTo(wallet.CryptocurrencyId));

            var expectedCryptocurrency = wallet.Cryptocurrency;
            var checkingCryptocurrency = checkingWallet.Cryptocurrency;
            Assert.That(checkingCryptocurrency?.Id, Is.EqualTo(expectedCryptocurrency?.Id));
            Assert.That(checkingCryptocurrency?.FullName, Is.EqualTo(expectedCryptocurrency?.FullName));
            Assert.That(checkingCryptocurrency?.ShortName, Is.EqualTo(expectedCryptocurrency?.ShortName));
            Assert.That(checkingCryptocurrency?.AlgorithmId, Is.EqualTo(expectedCryptocurrency?.AlgorithmId));

            var expectedAlgorithm = expectedCryptocurrency?.Algorithm;
            var checkingAlgorithm = checkingCryptocurrency?.Algorithm;
            Assert.That(checkingAlgorithm?.Id, Is.EqualTo(expectedAlgorithm?.Id));
            Assert.That(checkingAlgorithm?.OwnerId, Is.EqualTo(expectedAlgorithm?.OwnerId));
            Assert.That(checkingAlgorithm?.Name, Is.EqualTo(expectedAlgorithm?.Name));
        });
    }

    [Test]
    public async Task Add_ExistingWallet_ShouldThrowAnDbUpdateException()
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

        await _walletRepository.Add(wallet);
        Context.ChangeTracker.Clear();


        // Act

        var exception = Assert.ThrowsAsync<DbUpdateException>(async () =>
            await _walletRepository.Add(wallet));


        // Assert

        Assert.That(exception, Is.Not.Null);
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

        Context.ChangeTracker.Clear();


        // Act

        await _walletRepository.Update(newWallet);
        var checkingWallet = await _walletRepository
            .GetAvailableById(walletId, userId, default);


        // Assert

        Assert.That(checkingWallet, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(checkingWallet.Id, Is.EqualTo(newWallet.Id));
            Assert.That(checkingWallet.OwnerId, Is.EqualTo(newWallet.OwnerId));
            Assert.That(checkingWallet.Name, Is.EqualTo(newWallet.Name));
            Assert.That(checkingWallet.Address, Is.EqualTo(newWallet.Address));
            Assert.That(checkingWallet.CryptocurrencyId, Is.EqualTo(newWallet.CryptocurrencyId));

            var expectedCryptocurrency = newWallet.Cryptocurrency;
            var checkingCryptocurrency = checkingWallet.Cryptocurrency;
            Assert.That(checkingCryptocurrency?.Id, Is.EqualTo(expectedCryptocurrency?.Id));
            Assert.That(checkingCryptocurrency?.OwnerId, Is.EqualTo(expectedCryptocurrency?.OwnerId));
            Assert.That(checkingCryptocurrency?.FullName, Is.EqualTo(expectedCryptocurrency?.FullName));
            Assert.That(checkingCryptocurrency?.ShortName, Is.EqualTo(expectedCryptocurrency?.ShortName));
            Assert.That(checkingCryptocurrency?.AlgorithmId, Is.EqualTo(expectedCryptocurrency?.AlgorithmId));

            var expectedAlgorithm = expectedCryptocurrency?.Algorithm;
            var checkingAlgorithm = checkingCryptocurrency?.Algorithm;
            Assert.That(checkingAlgorithm?.Id, Is.EqualTo(expectedAlgorithm?.Id));
            Assert.That(checkingAlgorithm?.OwnerId, Is.EqualTo(expectedAlgorithm?.OwnerId));
            Assert.That(checkingAlgorithm?.Name, Is.EqualTo(expectedAlgorithm?.Name));
        });
    }
}
