using MNX.MonitoringCenter.Management.Tests.Service.Assertions;
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

        checkingPool.ShouldBeEqualTo(pool);
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

        checkingPool.ShouldBeEqualTo(newPool);
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
