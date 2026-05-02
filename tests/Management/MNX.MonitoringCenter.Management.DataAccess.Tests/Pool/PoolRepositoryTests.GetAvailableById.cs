using MNX.MonitoringCenter.Management.Tests.Service.Assertions;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Pool;

using Pool = Core.Mining.Pool;

public partial class PoolRepositoryTests
{
    [TestCaseSource(typeof(PoolsTestCaseSource), nameof(PoolsTestCaseSource.Pools))]
    public async Task GetAvailableById_ValidIdAndUserId_ReturnsUsersEntity(Pool data)
    {
        // Arrange

        var poolId = data.Id;
        var userId = PoolsTestCaseSource.UserId;
        await _poolRepository.Add(data);


        // Act

        var checkingPool = await _poolRepository.GetAvailableById(poolId, userId, default);


        // Assert

        checkingPool.ShouldBeEqualTo(data);
    }

    [TestCaseSource(typeof(PoolsTestCaseSource), nameof(PoolsTestCaseSource.DomainPools))]
    public async Task GetAvailableById_ValidIdAndInvalidUserId_ReturnsDomainEntity(Pool data)
    {
        // Arrange

        var poolId = data.Id;
        var userId = Guid.NewGuid();

        await _poolRepository.Add(data);


        // Act

        var checkingPool = await _poolRepository
            .GetAvailableById(poolId, userId, default);


        // Assert

        checkingPool.ShouldBeEqualTo(data);
    }

    [TestCaseSource(typeof(PoolsTestCaseSource), nameof(PoolsTestCaseSource.Pools))]
    public async Task GetAvailableById_InvalidIdAndUserId_ReturnsNull(Pool data)
    {
        // Arrange

        var poolId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        await _poolRepository.Add(data);


        // Act

        var checkingPool = await _poolRepository.GetAvailableById(poolId, userId, default);


        // Assert

        Assert.That(checkingPool, Is.Null);
    }
}
