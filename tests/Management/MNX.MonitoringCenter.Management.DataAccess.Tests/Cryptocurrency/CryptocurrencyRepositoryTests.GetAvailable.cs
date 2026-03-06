using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;
using MNX.MonitoringCenter.Management.UseCases;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Cryptocurrency;

using Cryptocurrency = Core.Mining.Cryptocurrency;

public partial class CryptocurrencyRepositoryTests
{
    [TestCaseSource(typeof(CryptocurrencyTestCaseSource), nameof(CryptocurrencyTestCaseSource.CryptocurrencyList))]
    public async Task GetAvailvable_ValidUserId_ReturnsCryptocurrencyList(List<Cryptocurrency> data)
    {
        // Arrange

        var userId = CryptocurrencyTestCaseSource.UserId;
        var specification = new Specification(userId);

        foreach (var item in data)
        {
            await _cryptocurrencyRepository.Add(item);
        }

        // Act

        var checkingCryptocurrencyList = await _cryptocurrencyRepository
            .GetAllAvailable(specification).ToListAsync();


        // Assert

        Assert.That(checkingCryptocurrencyList, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(checkingCryptocurrencyList, Has.Count.EqualTo(data.Count));
            AssertCryptocurrencies(data, checkingCryptocurrencyList);
        });
    }

    [TestCaseSource(typeof(CryptocurrencyTestCaseSource), nameof(CryptocurrencyTestCaseSource.CryptocurrencyList))]
    public async Task GetAvailable_InvalidUserId_ReturnsCryptocurrencyList(List<Cryptocurrency> data)
    {
        // Arrange

        var userId = Guid.NewGuid();
        var specification = new Specification(userId);
        var expectedCount = data.Where(x => x.OwnerId == userId || x.OwnerId is null).Count();

        foreach (var item in data)
        {
            await _cryptocurrencyRepository.Add(item);
        }


        // Act

        var checkingCryptocurrencyList = await _cryptocurrencyRepository
            .GetAllAvailable(specification).ToListAsync();


        // Assert

        Assert.That(checkingCryptocurrencyList, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(checkingCryptocurrencyList, Has.Count.EqualTo(expectedCount));

            var expectedData = data.Where(x => x.OwnerId == userId || x.OwnerId is null).ToList();
            AssertCryptocurrencies(expectedData, checkingCryptocurrencyList);
        });
    }

    private static void AssertCryptocurrencies(List<Cryptocurrency> expected, List<Cryptocurrency> checking)
    {
        expected = expected.OrderBy(x => x.FullName).ToList();
        checking = checking.OrderBy(x => x.FullName).ToList();

        for (var i = 0; i < expected.Count; i++)
        {
            Assert.Multiple(() =>
            {
                Assert.That(checking[i].Id, Is.EqualTo(expected[i].Id));
                Assert.That(checking[i].FullName, Is.EqualTo(expected[i].FullName));
                Assert.That(checking[i].ShortName, Is.EqualTo(expected[i].ShortName));
                Assert.That(checking[i].OwnerId, Is.EqualTo(expected[i].OwnerId));
                Assert.That(checking[i].AlgorithmId, Is.EqualTo(expected[i].AlgorithmId));
                Assert.That(checking[i].Algorithm?.Id, Is.EqualTo(expected[i].Algorithm?.Id));
                Assert.That(checking[i].Algorithm?.OwnerId, Is.EqualTo(expected[i].Algorithm?.OwnerId));
                Assert.That(checking[i].Algorithm?.Name, Is.EqualTo(expected[i].Algorithm?.Name));
            });
        }
    }

    private static class CryptocurrencyTestCaseSource
    {
        public static Guid UserId = Guid.NewGuid();

        public static IEnumerable<List<Cryptocurrency>> CryptocurrencyList
        {
            get
            {
                yield return
                [
                    new CryptocurrencyBuilder()
                        .WithOwner(UserId)
                        .WithAlgorithm(algo =>
                            algo.WithOwner(UserId))
                        .Build(),
                    new CryptocurrencyBuilder()
                        .WithOwner(UserId)
                        .WithAlgorithm(algo =>
                            algo.WithOwner(UserId))
                        .Build(),
                    new CryptocurrencyBuilder()
                        .WithAlgorithm(algo =>
                            algo.WithName("DomainAlgorithm_1"))
                        .Build()
                ];
            }
        }
    }
}
