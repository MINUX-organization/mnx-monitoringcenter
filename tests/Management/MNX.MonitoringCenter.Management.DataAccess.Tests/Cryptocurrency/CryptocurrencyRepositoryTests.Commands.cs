using MNX.MonitoringCenter.Management.Tests.Service.Assertions;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Cryptocurrency;

using Cryptocurrency = Core.Mining.Cryptocurrency;

public partial class CryptocurrencyRepositoryTests
{
    [TestCaseSource(typeof(CryptocurrencyTestCaseSource), nameof(CryptocurrencyTestCaseSource.CustomCryptocurrencies))]
    public async Task Add_ValidCryptocurrency_ShoudAddEntity(Cryptocurrency data)
    {
        // Arrange

        var cryptocurrencyId = data.Id;
        var userId = CryptocurrencyTestCaseSource.UserId;

        // Act

        await _cryptocurrencyRepository.Add(data);
        var checkingCryptocurrency = await _cryptocurrencyRepository
            .GetAvailableById(cryptocurrencyId, userId, default);
        

        // Assert

        checkingCryptocurrency.ShouldBeEqualTo(data);
    }

    [TestCaseSource(typeof(CryptocurrencyTestCaseSource), nameof(CryptocurrencyTestCaseSource.CustomCryptocurrencies))]
    public async Task Remove_ValidCryptocurrency_ShouldRemove(Cryptocurrency data)
    {
        // Arrange
        
        var cryptocurrencyId = data.Id;
        var userId = CryptocurrencyTestCaseSource.UserId;

        await _cryptocurrencyRepository.Add(data);


        // Act

        await _cryptocurrencyRepository.Remove(cryptocurrencyId, userId);
        var checkingCryptocurrency = await _cryptocurrencyRepository
            .GetAvailableById(cryptocurrencyId, userId, default);


        // Assert

        Assert.That(checkingCryptocurrency, Is.Null);
    }
}
