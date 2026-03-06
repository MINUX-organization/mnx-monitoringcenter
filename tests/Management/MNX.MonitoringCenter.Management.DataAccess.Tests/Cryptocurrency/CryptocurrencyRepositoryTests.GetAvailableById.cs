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

        Assert.That(checkingCryptocurrency, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(checkingCryptocurrency.IsDomain(), Is.True);
            Assert.That(checkingCryptocurrency.OwnerId, Is.Null);

            Assert.That(checkingCryptocurrency.Id, Is.EqualTo(cryptocurrencyId));
            Assert.That(checkingCryptocurrency.FullName, Is.EqualTo(cryptocurrency.FullName));
            Assert.That(checkingCryptocurrency.ShortName, Is.EqualTo(cryptocurrency.ShortName));
            Assert.That(checkingCryptocurrency.AlgorithmId, Is.EqualTo(cryptocurrency.AlgorithmId));
            Assert.That(checkingCryptocurrency.Algorithm?.Id, Is.EqualTo(cryptocurrency.Algorithm?.Id));
            Assert.That(checkingCryptocurrency.Algorithm?.OwnerId, Is.EqualTo(cryptocurrency.Algorithm?.OwnerId));
            Assert.That(checkingCryptocurrency.Algorithm?.Name, Is.EqualTo(cryptocurrency.Algorithm?.Name));
        });
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
