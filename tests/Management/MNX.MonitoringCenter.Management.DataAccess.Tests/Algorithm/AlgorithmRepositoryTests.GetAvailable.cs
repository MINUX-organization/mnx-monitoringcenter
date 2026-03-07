using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;
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

        Assert.That(checkingAlgorithms, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(checkingAlgorithms, Has.Count.EqualTo(data.Count));

            AssertAlgorithms(data, checkingAlgorithms);
        });
    }

    [TestCaseSource(typeof(AlgorithmsTestCaseSource), nameof(AlgorithmsTestCaseSource.CoreAlgorithmLists))]
    public async Task GetAvailable_InvalidUserId_ReturnsAlgorithmListWithoutOwned(List<Algorithm> data)
    {
        // Arrange

        var expectedCount = data.Where(x => x.OwnerId is null).Count();

        var userId = Guid.NewGuid();
        var specification = new Specification(userId);

        foreach (var algorithm in data)
        {
            await _algorithmRepository.AddAsync(algorithm);
        }


        // Act

        var checkingAlgorithms = await _algorithmRepository.GetAvailable(specification).ToListAsync();


        // Assert

        Assert.That(checkingAlgorithms, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(checkingAlgorithms, Has.Count.EqualTo(expectedCount));

            var expectedData = data.Where(x => x.OwnerId is null).ToList();
            AssertAlgorithms(expectedData, checkingAlgorithms);
        });
    }

    private static void AssertAlgorithms(List<Algorithm> expected, List<Algorithm> checking)
    {
        expected = expected.OrderBy(x => x.Name).ToList();
        checking = checking.OrderBy(x => x.Name).ToList();

        for (var i = 0; i < expected.Count; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(checking[i].Id, Is.EqualTo(expected[i].Id));
                Assert.That(checking[i].OwnerId, Is.EqualTo(expected[i].OwnerId));
                Assert.That(checking[i].Name, Is.EqualTo(expected[i].Name));
                Assert.That(checking[i].IsDomain(), Is.EqualTo(expected[i].IsDomain()));
            });
        }
    }

    private static class AlgorithmsTestCaseSource
    {
        public static Guid UserId = Guid.NewGuid();

        public static IEnumerable<List<Algorithm>> CoreAlgorithmLists
        {
            get
            {
                yield return new List<Algorithm>
                {
                    new AlgorithmBuilder()
                        .WithOwner(UserId)
                        .Build(),
                    new AlgorithmBuilder()
                        .WithOwner(UserId)
                        .Build(),
                    new AlgorithmBuilder()
                        .Build(),
                };
            }
        }
    }
}
