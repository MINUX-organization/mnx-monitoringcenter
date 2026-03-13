using MNX.MonitoringCenter.Management.Tests.Service.Assertions;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Cryptocurrency;

public partial class CryptocurrencyRepositoryTests
{
    [Test]
    public async Task GetAvailableById_ValidIdAndUserId_ReturnsUsersCryptocurrency()
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
        await _cryptocurrencyRepository.Add(cryptocurrency);


        // Act

        var checkingCryptocurrency = await _cryptocurrencyRepository
            .GetAvailableById(cryptocurrencyId, userId, default);


        // Assert

        checkingCryptocurrency.ShouldBeEqualTo(cryptocurrency);
    }

    [Test]
    public async Task GetAvailableById_ValidIdAndInvalidUserId_ReturnsDomainCryptocurrency()
    {
        // Arrange

        var cryptocurrencyId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var cryptocurrency = new CryptocurrencyBuilder()
            .WithId(cryptocurrencyId)
            .WithAlgorithm(algo =>
                algo.WithName("Algo_1"))
            .Build();

        await _cryptocurrencyRepository.Add(cryptocurrency);


        // Act

        var checkingCryptocurrency = await _cryptocurrencyRepository
            .GetAvailableById(cryptocurrencyId, userId, default);


        // Assert

        checkingCryptocurrency.ShouldBeEqualTo(cryptocurrency);
    }

    [Test]
    public async Task GetAvailableById_InvalidIdAndUserId_ReturnsNull()
    {
        // Arrange

        var cryptocurrencyId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        await _cryptocurrencyRepository.Add(
            new CryptocurrencyBuilder()
                .WithAlgorithm(algo =>
                    algo.WithName("Algo_2"))
                .Build());


        // Act

        var checkingCryptocurrency = await _cryptocurrencyRepository
            .GetAvailableById(cryptocurrencyId, userId, default);


        // Assert

        Assert.That(checkingCryptocurrency, Is.Null);
    }
}
