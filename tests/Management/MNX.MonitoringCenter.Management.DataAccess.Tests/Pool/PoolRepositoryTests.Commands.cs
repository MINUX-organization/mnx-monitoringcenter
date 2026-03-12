using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Pool;

public partial class PoolRepositoryTests
{
    [Test]
    public async Task Add_ValidPool_ShouldAddEntity()
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


        // Act

        await _poolRepository.Add(pool);
        var checkingPool = await _poolRepository
            .GetAvailableById(poolId, userId, default);


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
    public async Task Update_ValidPoolWithNewName_ShouldEditEntity()
    {
        // Arrange

        var poolId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var oldPool = new PoolBuilder()
            .WithId(poolId)
            .WithOwner(userId)
            .WithCryptocurrency(crypto =>
                crypto.WithAlgorithm())
            .Build();
        var newPool = new PoolBuilder()
            .WithId(poolId)
            .WithOwner(userId)
            .WithDomain("www.new-pool-install.com")
            .WithPort(12345)
            .WithCryptocurrency(crypto =>
                crypto.WithId(oldPool.CryptocurrencyId)
                      .WithAlgorithm(algo =>
                        algo.WithId(oldPool.Cryptocurrency!.AlgorithmId)))
            .Build();

        await _poolRepository.Add(oldPool);

        ClearChangeTracker();


        // Act

        await _poolRepository.Update(newPool);
        var checkingPool = await _poolRepository
            .GetAvailableById(poolId, userId, default);
        

        // Assert

        Assert.That(checkingPool, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(checkingPool.Id, Is.EqualTo(poolId));
            Assert.That(checkingPool.OwnerId, Is.EqualTo(userId));
            Assert.That(checkingPool.Domain, Is.EqualTo(newPool.Domain));
            Assert.That(checkingPool.Port, Is.EqualTo(newPool.Port));
            Assert.That(checkingPool.Tls, Is.EqualTo(newPool.Tls));
            Assert.That(checkingPool.CryptocurrencyId, Is.EqualTo(newPool.CryptocurrencyId));

            var expectedCryprocurrency = newPool.Cryptocurrency;
            var checkingCryptocurrency = checkingPool.Cryptocurrency;
            Assert.That(checkingCryptocurrency?.Id, Is.EqualTo(expectedCryprocurrency?.Id));
            Assert.That(checkingCryptocurrency?.OwnerId, Is.EqualTo(expectedCryprocurrency?.OwnerId));
            Assert.That(checkingCryptocurrency?.AlgorithmId, Is.EqualTo(expectedCryprocurrency?.AlgorithmId));
            Assert.That(checkingCryptocurrency?.FullName, Is.EqualTo(expectedCryprocurrency?.FullName));
            Assert.That(checkingCryptocurrency?.ShortName, Is.EqualTo(expectedCryprocurrency?.ShortName));
        });
    }

    [Test]
    public async Task Remove_ValidPoolId_ShouldRemoveEntity()
    {
        // Arrange

        var poolId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        await _poolRepository.Add(new PoolBuilder()
            .WithId(poolId)
            .WithOwner(userId)
            .WithCryptocurrency(crypto =>
                crypto.WithAlgorithm())
            .Build());


        // Act

        await _poolRepository.Remove(poolId, userId);
        var checkingPool = await _poolRepository
            .GetAvailableById(poolId, userId, default);
        

        // Assert

        Assert.That(checkingPool, Is.Null);
    }
}
