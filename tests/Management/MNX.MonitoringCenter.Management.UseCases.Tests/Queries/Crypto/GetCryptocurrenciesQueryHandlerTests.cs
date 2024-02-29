using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using MNX.MonitoringCenter.Management.UseCases.Queries.GetCryptocurrenciesQuery;
using Moq;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Queries.Crypto;

[TestFixture]
public class GetCryptocurrenciesQueryHandlerTests
{
    [Test]
    public async Task GetCryptoQuery_ReturnsQuery()
    {
        var cryptocurrencies = new List<Cryptocurrency>
        {
            new() {
                ShortName = "BTC",
                FullName = "Bitcoin",
                AlgorithmName = "SHA-256"
            },

            new() {
                ShortName = "Eht",
                FullName = "Ethereum",
                AlgorithmName = "KECCAK-256"
            }
        };

        var cryptoRepository = new Mock<ICryptocurrencyRepository>();

        cryptoRepository
            .Setup(x => x.GetAll())
            .Returns(cryptocurrencies.ToAsyncEnumerable());

        var handler = new GetCryptocurrenciesQueryHandler(cryptoRepository.Object);

        var query = new GetCryptocurrenciesQuery();

        var result = await handler.Handle(query, default).ToListAsync();

        Assert.That(result.Count, Is.EqualTo(cryptocurrencies.Count()));

        foreach (var cryptoModel in cryptocurrencies)
        {
            Assert.IsTrue(result.Any(resultCrypto =>
                                     resultCrypto.FullName == cryptoModel.FullName &&
                                     resultCrypto.ShortName == cryptoModel.ShortName &&
                                     resultCrypto.AlgorithmName == cryptoModel.AlgorithmName));
        }
    }
}