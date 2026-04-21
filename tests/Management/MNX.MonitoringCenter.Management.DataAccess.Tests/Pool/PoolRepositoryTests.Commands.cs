using MNX.MonitoringCenter.Management.Tests.Service.Assertions;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Pool;

using Pool = Core.Mining.Pool;

public partial class PoolRepositoryTests
{
    [TestCaseSource(typeof(PoolsTestCaseSource), nameof(PoolsTestCaseSource.Pools))]
    public async Task Add_ValidPool_ShouldAddEntity(Pool data)
    {
        // Arrange

        var poolId = data.Id;
        var userId = PoolsTestCaseSource.UserId;


        // Act

        await _poolRepository.Add(data);
        var checkingPool = await _poolRepository
            .GetAvailableById(poolId, userId, default);


        // Assert

        checkingPool.ShouldBeEqualTo(data);
    }

    [TestCaseSource(typeof(PoolsTestCaseSource), nameof(PoolsTestCaseSource.Pools))]
    public async Task Update_ValidPoolWithNewName_ShouldEditEntity(Pool data)
    {
        // Arrange

        var poolId = data.Id;
        var userId = PoolsTestCaseSource.UserId;
        var newPool = new PoolBuilder()
            .WithId(poolId)
            .WithOwner(userId)
            .WithDomain("www.new-pool-install.com")
            .WithPort(12345)
            .WithCryptocurrency(crypto =>
                crypto.WithId(data.CryptocurrencyId)
                      .WithAlgorithm(algo =>
                        algo.WithId(data.Cryptocurrency!.AlgorithmId)))
            .Build();

        await _poolRepository.Add(data);

        ClearChangeTracker();


        // Act

        await _poolRepository.Update(newPool);
        var checkingPool = await _poolRepository
            .GetAvailableById(poolId, userId, default);


        // Assert

        checkingPool.ShouldBeEqualTo(newPool);
    }

    [TestCaseSource(typeof(PoolsTestCaseSource), nameof(PoolsTestCaseSource.Pools))]
    public async Task Remove_ValidPoolId_ShouldRemoveEntity(Pool data)
    {
        // Arrange

        var poolId = data.Id;
        var userId = PoolsTestCaseSource.UserId;
        await _poolRepository.Add(data);


        // Act

        await _poolRepository.Remove(poolId, userId);
        var checkingPool = await _poolRepository
            .GetAvailableById(poolId, userId, default);
        

        // Assert

        Assert.That(checkingPool, Is.Null);
    }
}
