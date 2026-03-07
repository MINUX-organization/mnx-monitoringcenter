using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Wallet;

public partial class WalletRepositoryTests
{
    [Test]
    public async Task GetAvailableById_ValidIdAndUserId_ReturnsEntity()
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
        
        
        // Act

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
    public async Task GetAvailableById_InvalidIdAndUserId_ReturnsNull()
    {
        // Arrange

        var walletId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        await _walletRepository.Add(new WalletBuilder()
            .WithCryptocurrency(crypto =>
                crypto.WithAlgorithm())
            .Build());


        // Act

        var checkingWallet = await _walletRepository
            .GetAvailableById(walletId, userId, default);


        // Assert

        Assert.That(checkingWallet, Is.Null);
    }
}
