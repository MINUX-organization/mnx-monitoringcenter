using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Wallet;

public partial class WalletRepositoryTests
{
    [Test]
    public async Task ExistsWithName_ValidUserIdAndName_ReturnsTrue()
    {
        // Arrange

        var userId = Guid.NewGuid();
        var name = "Wallet_1";

        await _walletRepository.Add(new WalletBuilder()
            .WithOwnerId(userId)
            .WithName(name)
            .WithCryptocurrency(crypto =>
                crypto.WithAlgorithm())
            .Build());


        // Act

        var isExists = await _walletRepository
            .ExistsWithName(userId, name, default);


        // Assert

        Assert.That(isExists, Is.True);
    }

    [Test]
    public async Task ExistsWithName_InvalidUserIdAndName_ReturnsFalse()
    {
        // Arrange

        var userId = Guid.NewGuid();
        var name = "Wallet_1";

        await _walletRepository.Add(new WalletBuilder()
            .WithCryptocurrency(crypto =>
                crypto.WithAlgorithm())
            .Build());


        // Act

        var isExists = await _walletRepository
            .ExistsWithName(userId, name, default);


        // Assert

        Assert.That(isExists, Is.False);
    }

    [Test]
    public async Task ExistsWithAddress_ValidUserIdAndAddress_ReturnsTrue()
    {
        // Arrange

        var userId = Guid.NewGuid();
        var address = "WalletAddress_1";

        await _walletRepository.Add(new WalletBuilder()
            .WithOwnerId(userId)
            .WithAddress(address)
            .WithCryptocurrency(crypto =>
                crypto.WithAlgorithm())
            .Build());


        // Act

        var isExists = await _walletRepository
            .ExistsWithAddress(userId, address, default);


        // Assert

        Assert.That(isExists, Is.True);
    }

    [Test]
    public async Task ExistsWithAddress_InvalidUserIdAndAddress_ReturnsFalse() 
    {
        // Arrange

        var userId = Guid.NewGuid();
        var address = "WalletAddress_1";

        await _walletRepository.Add(new WalletBuilder()
            .WithCryptocurrency(crypto =>
                crypto.WithAlgorithm())
            .Build());


        // Act

        var isExists = await _walletRepository.ExistsWithAddress(userId, address, default);


        // Assert

        Assert.That(isExists, Is.False);
    }
}
