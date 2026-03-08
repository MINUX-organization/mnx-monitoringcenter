using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Pool;

public partial class PoolRepositoryTests
{
    [Test]
    public async Task GetAvailableById_ValidIdAndUserId_ReturnsUsersEntity()
    {
        // Arrange

        var poolId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var pool = new PoolBuilder()
            .WithId(poolId)
            .WithOwner(userId)
            .WithCryptocurrency(crypto =>
                crypto.WithAlgorithm())
            .Build();

        await _poolRepository.Add(pool);


        // Act

        var checkingPool = await _poolRepository.GetAvailableById(poolId, userId, default);


        // Assert

        Assert.That(checkingPool, Is.Not.Null);

        Assert.Multiple(() =>
        {
            Assert.That(checkingPool.Id, Is.EqualTo(poolId));
            Assert.That(checkingPool.OwnerId, Is.EqualTo(userId));
            Assert.That(checkingPool.Domain, Is.EqualTo(pool.Domain));
            Assert.That(checkingPool.Port, Is.EqualTo(pool.Port));
            Assert.That(checkingPool.Tls, Is.EqualTo(pool.Tls));
            Assert.That(checkingPool.CryptocurrencyId, Is.EqualTo(pool.CryptocurrencyId));

            var expectedCryprocurrency = pool.Cryptocurrency;
            var checkingCryptocurrency = checkingPool.Cryptocurrency;
            Assert.That(checkingCryptocurrency?.Id, Is.EqualTo(expectedCryprocurrency?.Id));
            Assert.That(checkingCryptocurrency?.OwnerId, Is.EqualTo(expectedCryprocurrency?.OwnerId));
            Assert.That(checkingCryptocurrency?.AlgorithmId, Is.EqualTo(expectedCryprocurrency?.AlgorithmId));
            Assert.That(checkingCryptocurrency?.FullName, Is.EqualTo(expectedCryprocurrency?.FullName));
            Assert.That(checkingCryptocurrency?.ShortName, Is.EqualTo(expectedCryprocurrency?.ShortName));
        });
    }

    [Test]
    public async Task GetAvailableById_ValidIdAndInvalidUserId_ReturnsDomainEntity()
    {
        // Arrange

        var poolId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var pool = new PoolBuilder()
            .WithId(poolId)
            .WithTls()
            .WithCryptocurrency(crypto =>
                crypto.WithAlgorithm())
            .Build();

        await _poolRepository.Add(pool);


        // Act

        var checkingPool = await _poolRepository.GetAvailableById(poolId, userId, default);


        // Assert

        Assert.That(checkingPool, Is.Not.Null);
        Assert.That(checkingPool.OwnerId, Is.Null);

        Assert.Multiple(() =>
        {
            Assert.That(checkingPool.Id, Is.EqualTo(poolId));
            Assert.That(checkingPool.IsDomain(), Is.True);
            Assert.That(checkingPool.Domain, Is.EqualTo(pool.Domain));
            Assert.That(checkingPool.Port, Is.EqualTo(pool.Port));
            Assert.That(checkingPool.Tls, Is.EqualTo(pool.Tls));
            Assert.That(checkingPool.CryptocurrencyId, Is.EqualTo(pool.CryptocurrencyId));

            var expectedCryprocurrency = pool.Cryptocurrency;
            var checkingCryptocurrency = checkingPool.Cryptocurrency;
            Assert.That(checkingCryptocurrency?.Id, Is.EqualTo(expectedCryprocurrency?.Id));
            Assert.That(checkingCryptocurrency?.OwnerId, Is.EqualTo(expectedCryprocurrency?.OwnerId));
            Assert.That(checkingCryptocurrency?.AlgorithmId, Is.EqualTo(expectedCryprocurrency?.AlgorithmId));
            Assert.That(checkingCryptocurrency?.FullName, Is.EqualTo(expectedCryprocurrency?.FullName));
            Assert.That(checkingCryptocurrency?.ShortName, Is.EqualTo(expectedCryprocurrency?.ShortName));
        });
    }

    [Test]
    public async Task GetAvailableById_InvalidIdAndUserId_ReturnsNull()
    {
        // Arrange

        var poolId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var pool = new PoolBuilder()
            .WithCryptocurrency(crypto =>
                crypto.WithAlgorithm())
            .Build();

        await _poolRepository.Add(pool);


        // Act

        var checkingPool = await _poolRepository.GetAvailableById(poolId, userId, default);


        // Assert

        Assert.That(checkingPool, Is.Null);
    }
}
