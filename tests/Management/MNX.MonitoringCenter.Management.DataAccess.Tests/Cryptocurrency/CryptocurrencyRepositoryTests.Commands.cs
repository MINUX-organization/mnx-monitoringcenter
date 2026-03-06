using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Cryptocurrency;

public partial class CryptocurrencyRepositoryTests
{
    [Test]
    public async Task Add_ValidCryptocurrency_ShoudAddEntity()
    {
        // Arrange

        var cryptocurrencyId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var cryptocurrency = new CryptocurrencyBuilder()
            .WithId(cryptocurrencyId)
            .WithOwner(userId)
            .WithAlgorithm(algo =>
                algo.WithOwner(userId))
            .Build();


        // Act

        await _cryptocurrencyRepository.Add(cryptocurrency);
        var checkingCryptocurrency = await _cryptocurrencyRepository
            .GetAvailableById(cryptocurrencyId, userId, default);
        

        // Assert

        Assert.That(checkingCryptocurrency, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(checkingCryptocurrency.Id, Is.EqualTo(cryptocurrencyId));
            Assert.That(checkingCryptocurrency.OwnerId, Is.EqualTo(userId));
            Assert.That(checkingCryptocurrency.FullName, Is.EqualTo(cryptocurrency.FullName));
            Assert.That(checkingCryptocurrency.ShortName, Is.EqualTo(cryptocurrency.ShortName));
            Assert.That(checkingCryptocurrency.AlgorithmId, Is.EqualTo(cryptocurrency.AlgorithmId));
            Assert.That(checkingCryptocurrency.Algorithm?.Id, Is.EqualTo(cryptocurrency.Algorithm?.Id));
            Assert.That(checkingCryptocurrency.Algorithm?.OwnerId, Is.EqualTo(cryptocurrency.Algorithm?.OwnerId));
            Assert.That(checkingCryptocurrency.Algorithm?.Name, Is.EqualTo(cryptocurrency.Algorithm?.Name));
        });
    }

    [Test]
    public async Task Add_ExistingCryptocurrency_ShouldThrowDbUpdateException()
    {
        // Arrange

        var cryptocurrencyId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        
        await _cryptocurrencyRepository.Add(new CryptocurrencyBuilder()
            .WithId(cryptocurrencyId)
            .WithOwner(userId)
            .WithAlgorithm(algo =>
                algo.WithOwner(userId))
            .Build());

        Context.ChangeTracker.Clear();

        // Act

        var exception = Assert.ThrowsAsync<DbUpdateException>(async () =>
        {
            await _cryptocurrencyRepository.Add(new CryptocurrencyBuilder()
                .WithId(cryptocurrencyId)
                .WithOwner(userId)
                .WithAlgorithm(algo =>
                    algo.WithOwner(userId))
                .Build());
        });

        // Assert
        
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception, Is.TypeOf<DbUpdateException>());
    }

    [Test]
    public async Task Remove_ValidCryptocurrency_ShouldRemove()
    {
        // Arrange
        
        var cryptocurrencyId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        await _cryptocurrencyRepository.Add(new CryptocurrencyBuilder()
            .WithId(cryptocurrencyId)
            .WithOwner(userId)
            .WithAlgorithm(algo =>
                algo.WithOwner(userId))
            .Build());

        // Act

        await _cryptocurrencyRepository.Remove(cryptocurrencyId, userId);
        var checkingCryptocurrency = await _cryptocurrencyRepository
            .GetAvailableById(cryptocurrencyId, userId, default);

        // Assert

        Assert.That(checkingCryptocurrency, Is.Null);
    }
}
