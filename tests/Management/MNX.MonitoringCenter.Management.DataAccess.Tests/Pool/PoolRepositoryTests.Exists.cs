using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Pool;

public partial class PoolRepositoryTests
{
    [Test]
    public async Task Exists_ValidUserIdAndDomainAndPort_ReturnsTrue()
    {
        // Arrange

        var userId = Guid.NewGuid();
        var domain = "www.pool-domain.com";
        var port = 5556;

        await _poolRepository.Add(new PoolBuilder()
            .WithOwner(userId)
            .WithDomain(domain)
            .WithPort(port)
            .WithCryptocurrency(crypto =>
                crypto.WithAlgorithm())
            .Build());


        // Act

        var isExists = await _poolRepository
            .Exists(userId, domain, port, default);


        // Assert

        Assert.That(isExists, Is.True);
    }

    [Test]
    public async Task Exists_InvalidUserIdAndDomainAndPort_ReturnsFalse()
    {
        // Arrange

        var userId = Guid.NewGuid();
        var domain = "www.pool-domain.com";
        var port = 5556;
        await _poolRepository.Add(new PoolBuilder()
            .WithCryptocurrency(crypto =>
                crypto.WithAlgorithm())
            .Build());

        // Act

        var isExists = await _poolRepository.Exists(userId, domain, port, default);


        // Assert

        Assert.That(isExists, Is.False);
    }
}
