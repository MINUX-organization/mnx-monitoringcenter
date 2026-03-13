using MNX.MonitoringCenter.Management.Tests.Service.Assertions;
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

        checkingCryptocurrency.ShouldBeEqualTo(cryptocurrency);
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
