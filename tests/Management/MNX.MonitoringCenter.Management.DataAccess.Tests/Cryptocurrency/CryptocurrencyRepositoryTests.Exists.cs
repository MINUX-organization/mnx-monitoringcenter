using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Cryptocurrency;

public partial class CryptocurrencyRepositoryTests
{
    [Test]
    public async Task Exists_ValidIdAndNames_ReturnsTrue()
    {
        // Arrange

        var userId = Guid.NewGuid();
        var fullName = "Cryptocurrency_1";
        var shortName = "Crypto_1";

        await _cryptocurrencyRepository.Add(new CryptocurrencyBuilder()
            .WithOwner(userId)
            .WithFullName(fullName)
            .WithShortName(shortName)
            .WithAlgorithm(algo =>
                algo.WithOwner(userId))
            .Build());


        // Act

        var isExists = await _cryptocurrencyRepository
            .Exists(userId, fullName, shortName, default);


        // Assert

        Assert.That(isExists, Is.True);
    }

    [Test]
    public async Task Exists_InvalidIdAndNames_ReturnsFalse()
    {
        // Arrange

        var userId = Guid.NewGuid();
        var fullName = "Cryptocurrency_1";
        var shortName = "Crypto_1";

        await _cryptocurrencyRepository.Add(
            new CryptocurrencyBuilder()
                .WithAlgorithm(algo => 
                    algo.WithName("Algo_1"))
                .Build());


        // Act

        var isExists = await _cryptocurrencyRepository
            .Exists(userId, fullName, shortName, default);


        // Assert

        Assert.That(isExists, Is.False);
    }
}
