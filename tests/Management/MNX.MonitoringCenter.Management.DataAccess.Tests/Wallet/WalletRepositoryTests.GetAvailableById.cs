using MNX.MonitoringCenter.Management.Tests.Service.Assertions;
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

        checkingWallet.ShouldBeEqualTo(wallet);
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
