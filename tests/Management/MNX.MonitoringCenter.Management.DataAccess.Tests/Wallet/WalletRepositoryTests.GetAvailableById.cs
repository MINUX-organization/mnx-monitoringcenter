using MNX.MonitoringCenter.Management.Tests.Service.Assertions;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Wallet;

using Wallet = Core.Mining.Wallet;

public partial class WalletRepositoryTests
{
    [TestCaseSource(typeof(WalletsTestCaseSource), nameof(WalletsTestCaseSource.Wallets))]
    public async Task GetAvailableById_ValidIdAndUserId_ReturnsEntity(Wallet data)
    {
        // Arrange

        var walletId = data.Id;
        var userId = WalletsTestCaseSource.UserId;
        await _walletRepository.Add(data);
        
        
        // Act

        var checkingWallet = await _walletRepository
            .GetAvailableById(walletId, userId, default);


        // Assert

        checkingWallet.ShouldBeEqualTo(data);
    }

    [TestCaseSource(typeof(WalletsTestCaseSource), nameof(WalletsTestCaseSource.Wallets))]
    public async Task GetAvailableById_InvalidIdAndUserId_ReturnsNull(Wallet data)
    {
        // Arrange

        var walletId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        await _walletRepository.Add(data);


        // Act

        var checkingWallet = await _walletRepository
            .GetAvailableById(walletId, userId, default);


        // Assert

        Assert.That(checkingWallet, Is.Null);
    }
}
