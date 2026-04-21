using MNX.MonitoringCenter.Management.Tests.Service.Assertions;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Wallet;

using Wallet = Core.Mining.Wallet;

public partial class WalletRepositoryTests
{
    [TestCaseSource(typeof(WalletsTestCaseSource), nameof(WalletsTestCaseSource.Wallets))]
    public async Task Add_ValidWallet_ShouldAddNewEntity(Wallet data)
    {
        // Arrange

        var walletId = data.Id;
        var userId = WalletsTestCaseSource.UserId;


        // Act

        await _walletRepository.Add(data);
        var checkingWallet = await _walletRepository
            .GetAvailableById(walletId, userId, default);


        // Assert

        checkingWallet.ShouldBeEqualTo(data);
    }

    [TestCaseSource(typeof(WalletsTestCaseSource), nameof(WalletsTestCaseSource.Wallets))]
    public async Task Remove_ValidIdAndUserId_ShouldRemoveEntity(Wallet data)
    {
        // Arrange

        var walletId = data.Id;
        var userId = WalletsTestCaseSource.UserId;
        await _walletRepository.Add(data);


        // Act

        await _walletRepository.Remove(walletId, userId);
        var checkingWallet = await _walletRepository
            .GetAvailableById(walletId, userId, default);


        // Assert

        Assert.That(checkingWallet, Is.Null);
    }

    [TestCaseSource(typeof(WalletsTestCaseSource), nameof(WalletsTestCaseSource.Wallets))]
    public async Task Update_ValidWallet_ShouldUpdateEntity(Wallet data)
    {
        // Arrange

        var walletId = data.Id;
        var userId = WalletsTestCaseSource.UserId;

        await _walletRepository.Add(new WalletBuilder()
            .WithId(walletId)
            .WithOwnerId(userId)
            .WithName("OldWalletName_1")
            .WithAddress("OldWalletAddress_1")
            .WithCryptocurrency(crypto =>
                crypto.WithId(data.CryptocurrencyId)
                      .WithAlgorithm(algo =>
                        algo.WithId(data.Cryptocurrency.AlgorithmId)))
            .Build());

        ClearChangeTracker();


        // Act

        await _walletRepository.Update(data);
        var checkingWallet = await _walletRepository
            .GetAvailableById(walletId, userId, default);


        // Assert

        checkingWallet.ShouldBeEqualTo(data);
    }
}
