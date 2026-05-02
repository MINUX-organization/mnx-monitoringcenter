using MNX.MonitoringCenter.Management.Tests.Service.Assertions;
using MNX.MonitoringCenter.Management.UseCases;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Pool;

using Pool = Core.Mining.Pool;

public partial class PoolRepositoryTests
{
    [TestCaseSource(typeof(PoolsTestCaseSource), nameof(PoolsTestCaseSource.PoolLists))]
    public async Task GetAllAvailable_ValidUserId_ReturnsAvailablePools(List<Pool> data)
    {
        // Arrange

        var specification = new Specification(PoolsTestCaseSource.UserId);

        await PrepareDataBase(data);

        foreach (var item in data)
            item.Cryptocurrency!.Algorithm = null;


        // Act

        var checkingPoolsList = await _poolRepository
            .GetAllAvailable(specification)
            .ToListAsync();


        // Assert

        checkingPoolsList.ShouldBeEqualTo(data);
    }

    [TestCaseSource(typeof(PoolsTestCaseSource), nameof(PoolsTestCaseSource.PoolLists))]
    public async Task GetAllAvailable_InvalidUserId_ReturnsAvailablePools(List<Pool> data)
    {
        // Arrange
        
        var specification = new Specification(Guid.NewGuid());
        var query = data.Where(x => x.OwnerId == specification.UserId ||
                                    x.OwnerId is null);
        var expectedCount = query.Count();

        await PrepareDataBase(data);
        foreach (var item in data)
            item.Cryptocurrency!.Algorithm = null;


        // Act

        var checkingPools = await _poolRepository.GetAllAvailable(specification).ToListAsync();


        // Assert

        checkingPools.ShouldBeEqualTo(query);
    }
}
