using MNX.MonitoringCenter.Management.Tests.Service.Assertions;
using MNX.MonitoringCenter.Management.UseCases;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Miner;

using Miner = Core.Mining.Miner.Miner;

public partial class MinerRepositoryTests
{
    [TestCaseSource(typeof(MinersTestCaseSource), nameof(MinersTestCaseSource.MinerLists))]
    public async Task GetAvailableMiners_ValidUserId_ReturnsAllAvailableEntities(List<Miner> data)
    {
        // Arrange

        var specification = new Specification(MinersTestCaseSource.UserId);
        var query = data.Where(x => x.OwnerId == specification.UserId ||
                                    x.OwnerId is null);
        var expectedCount = query.Count();

        await PrepareDataBase(data);


        // Act

        var checkingMiners = await _minerRepository.GetAvailableMiners(specification).ToListAsync();


        // Assert

        checkingMiners.ShouldBeEqualTo(query);
    }

    [TestCaseSource(typeof(MinersTestCaseSource), nameof(MinersTestCaseSource.MinerLists))]
    public async Task GetAvailableMiners_InvalidUserId_ReturnsDomainEntitiesList(List<Miner> data)
    {
        // Arrange

        var specification = new Specification(Guid.NewGuid());
        var query = data.Where(x => x.OwnerId == specification.UserId ||
                                    x.OwnerId is null);
        var expectedCount = query.Count();

        await PrepareDataBase(data);


        // Act

        var checkingMiners = await _minerRepository
            .GetAvailableMiners(specification).ToListAsync();


        // Assert

        checkingMiners.ShouldBeEqualTo(query);
    }
}
