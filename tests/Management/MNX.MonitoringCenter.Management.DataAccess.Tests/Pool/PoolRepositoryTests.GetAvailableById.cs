using MNX.MonitoringCenter.Management.Tests.Service.Assertions;
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

        checkingPool.ShouldBeEqualTo(pool);
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

        var checkingPool = await _poolRepository
            .GetAvailableById(poolId, userId, default);


        // Assert

        checkingPool.ShouldBeEqualTo(pool);
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
