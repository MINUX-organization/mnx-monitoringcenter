using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using MNX.MonitoringCenter.Management.UseCases.Queries.GetCryptocurrenciesQuery;
using Moq;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Queries.Crypto
{
    public class GetCryptocurrenciesQueryHandlerTests
    {
        [Test]
        public async Task GetCryptoQuery_ReturnsQuery()
        {
            var cryptocurrencies = new List<Cryptocurrency>
            {
                new Cryptocurrency
                {
                    ShortName = "BTC",
                    FullName = "Bitcoin",
                    AlgorithmName = "SHA-256"
                },

                new Cryptocurrency
                {
                    ShortName = "Eht",
                    FullName = "Ethereum",
                    AlgorithmName = "KECCAK-256"
                }
            };

            var cryptoRepository = new Mock<ICryptocurrencyRepository>();
            cryptoRepository
                .Setup(x => x.GetAll())
                .Returns(cryptocurrencies.ToAsyncEnumerable());

            var handler = new GetCryptocurrenciesQueryHandler(
                cryptoRepository.Object);

            var query = new GetCryptocurrenciesQuery();

            var result = await handler.Handle(query, default).ToListAsync();

            Assert.NotNull(result);
            Assert.That(result, Is.EqualTo(cryptocurrencies));
        }
    }
}
