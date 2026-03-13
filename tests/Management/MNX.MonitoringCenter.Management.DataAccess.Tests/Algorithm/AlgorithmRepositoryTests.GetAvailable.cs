using MNX.MonitoringCenter.Management.Tests.Service.Assertions;
using MNX.MonitoringCenter.Management.UseCases;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Algorithm;

using Algorithm = Core.Mining.Algorithm;

public partial class AlgorithmRepositoryTests
{
    [TestCaseSource(typeof(AlgorithmsTestCaseSource), nameof(AlgorithmsTestCaseSource.CoreAlgorithmLists))]
    public async Task GetAvailable_ValidUserId_ReturnsAlgorithmList(List<Algorithm> data)
    {
        // Arrange

        var userId = AlgorithmsTestCaseSource.UserId;
        var specification = new Specification(userId);
        foreach (var algorithm in data)
        {
            await _algorithmRepository.AddAsync(algorithm);
        }

        // Act

        var checkingAlgorithms = await _algorithmRepository
            .GetAvailable(specification).OrderBy(x => x.Name).ToListAsync();


        // Assert

        checkingAlgorithms.ShouldBeEqualTo(data);
    }

    [TestCaseSource(typeof(AlgorithmsTestCaseSource), nameof(AlgorithmsTestCaseSource.CoreAlgorithmLists))]
    public async Task GetAvailable_InvalidUserId_ReturnsAlgorithmListWithoutOwned(List<Algorithm> data)
    {
        // Arrange

        var query = data.Where(x => x.OwnerId is null);
        var expectedCount = query.Count();

        var userId = Guid.NewGuid();
        var specification = new Specification(userId);

        foreach (var algorithm in data)
        {
            await _algorithmRepository.AddAsync(algorithm);
        }


        // Act

        var checkingAlgorithms = await _algorithmRepository.GetAvailable(specification).ToListAsync();


        // Assert

        checkingAlgorithms.ShouldBeEqualTo(query);
    }
}
