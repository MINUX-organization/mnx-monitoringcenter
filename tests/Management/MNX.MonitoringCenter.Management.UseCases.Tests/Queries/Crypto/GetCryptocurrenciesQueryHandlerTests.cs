using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using MNX.MonitoringCenter.Management.UseCases.Queries.GetCryptocurrenciesQuery;
using MNX.MonitoringCenter.Management.UseCases.Tests.Commands;
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
                Algorithm = "SHA-256"
            },

            new() {
                ShortName = "Eht",
                FullName = "Ethereum",
                Algorithm = "KECCAK-256"
            }
        };

        var cryptoRepository = new Mock<ICryptocurrencyRepository>();

        cryptoRepository
            .Setup(x => x.GetAllAvailable(TestHelper.Cryptocurrency.UserId))
            .Returns(cryptocurrencies.ToAsyncEnumerable());

        var handler = new GetCryptocurrenciesQueryHandler(cryptoRepository.Object, TestHelper.GetMapper());

        var query = new GetCryptocurrenciesQuery(TestHelper.Cryptocurrency.UserId);

        var result = await handler.Handle(query, default).ToListAsync();

        Assert.That(result.Count, Is.EqualTo(cryptocurrencies.Count()));

        foreach (var cryptoModel in cryptocurrencies)
        {
            Assert.IsTrue(result.Any(resultCrypto =>
                                     resultCrypto.FullName == cryptoModel.FullName &&
                                     resultCrypto.ShortName == cryptoModel.ShortName &&
                                     resultCrypto.Algorithm == cryptoModel.Algorithm));
        }
    }
}