using MNX.MonitoringCenter.Management.UseCases;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Pool;

using Pool = Core.Mining.Pool;

public partial class PoolRepositoryTests
{
    [TestCaseSource(typeof(PoolsTestCaseSource), nameof(PoolsTestCaseSource.Pools))]
    public async Task GetAllAvailable_ValidUserId_ReturnsAvailablePools(List<Pool> data)
    {
        // Arrange

        var specification = new Specification(PoolsTestCaseSource.UserId);

        foreach (var item in data)
        {
            await _poolRepository.Add(item);
        }


        // Act

        var checkingPoolsList = await _poolRepository.GetAllAvailable(specification).ToListAsync();


        // Assert

        Assert.That(checkingPoolsList, Is.Not.Null);
        Assert.That(checkingPoolsList, Has.Count.EqualTo(data.Count));
        AssertPools(data, checkingPoolsList);
    }

    [TestCaseSource(typeof(PoolsTestCaseSource), nameof(PoolsTestCaseSource.Pools))]
    public async Task GetAllAvailable_InvalidUserId_ReturnsAvailablePools(List<Pool> data)
    {
        // Arrange
        
        var specification = new Specification(Guid.NewGuid());
        var query = data.Where(x => x.OwnerId == specification.UserId ||
                                    x.OwnerId is null);
        var expectedCount = query.Count();

        foreach (var item in data)
        {
            await _poolRepository.Add(item);
        }


        // Act

        var checkingPools = await _poolRepository.GetAllAvailable(specification).ToListAsync();


        // Assert

        Assert.That(checkingPools, Is.Not.Null);
        Assert.That(checkingPools, Has.Count.EqualTo(expectedCount));
        AssertPools(query.ToList(), checkingPools);
    }

    private void AssertPools(List<Pool> expected, List<Pool> checking)
    {
        expected = expected.OrderBy(x => x.Domain).ToList();
        checking = checking.OrderBy(x => x.Domain).ToList();

        for (var i = 0; i < expected.Count; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(checking[i].Id, Is.EqualTo(expected[i].Id));
                Assert.That(checking[i].Tls, Is.EqualTo(expected[i].Tls));
                Assert.That(checking[i].Domain, Is.EqualTo(expected[i].Domain));
                Assert.That(checking[i].Port, Is.EqualTo(expected[i].Port));
                Assert.That(checking[i].OwnerId, Is.EqualTo(expected[i].OwnerId));
                Assert.That(checking[i].CryptocurrencyId, Is.EqualTo(expected[i].CryptocurrencyId));

                var expectedCryptocurrency = expected[i].Cryptocurrency;
                var checkingCryptocurrency = checking[i].Cryptocurrency;
                Assert.That(checkingCryptocurrency?.Id, Is.EqualTo(expectedCryptocurrency?.Id));
                Assert.That(checkingCryptocurrency?.OwnerId, Is.EqualTo(expectedCryptocurrency?.OwnerId));
                Assert.That(checkingCryptocurrency?.FullName, Is.EqualTo(expectedCryptocurrency?.FullName));
                Assert.That(checkingCryptocurrency?.ShortName, Is.EqualTo(expectedCryptocurrency?.ShortName));
                Assert.That(checkingCryptocurrency?.AlgorithmId, Is.EqualTo(expectedCryptocurrency?.AlgorithmId));
            });
        }
    }
}
