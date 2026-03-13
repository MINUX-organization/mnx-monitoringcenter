using MNX.MonitoringCenter.Management.Tests.Service.Assertions;
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

        checkingCryptocurrencyList.ShouldBeEqualTo(data);
    }

    [TestCaseSource(typeof(CryptocurrencyTestCaseSource), nameof(CryptocurrencyTestCaseSource.CryptocurrencyList))]
    public async Task GetAvailable_InvalidUserId_ReturnsCryptocurrencyList(List<Cryptocurrency> data)
    {
        // Arrange

        var userId = Guid.NewGuid();
        var specification = new Specification(userId);

        var query = data.Where(x => x.OwnerId == userId ||
                                    x.OwnerId is null);
        var expectedCount = query.Count();

        foreach (var item in data)
        {
            await _cryptocurrencyRepository.Add(item);
        }


        // Act

        var checkingCryptocurrencyList = await _cryptocurrencyRepository
            .GetAllAvailable(specification).ToListAsync();


        // Assert

        checkingCryptocurrencyList.ShouldBeEqualTo(query);
    }
}
