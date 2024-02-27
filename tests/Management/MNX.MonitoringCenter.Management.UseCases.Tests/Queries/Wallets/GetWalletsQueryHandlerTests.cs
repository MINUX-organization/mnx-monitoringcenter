using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using MNX.MonitoringCenter.Management.UseCases.Queries.GetWalletsQuery;
using Moq;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Queries.Wallets
{
    [TestFixture]
    public class GetWalletsQueryHandlerTests
    {
        [Test]
        public async Task GetPools_ReturnsPools()
        {
            // Arrange
            var walletsList = new List<Wallet>()
            {
                new Wallet()
                {
                    Id = Guid.NewGuid(),
                    Name = "test1",
                    Address = "Tomsk",
                    Cryptocurrency = new()
                    {
                        FullName = "Etherium",
                        ShortName = "ETH",
                        AlgorithmName = "Algorithm"
                    }
                },

                new Wallet()
                {
                    Id = Guid.NewGuid(),
                    Name = "test2",
                    Address = "Moscow",
                    Cryptocurrency = new()
                    {
                        FullName = "Bitcoin",
                        ShortName = "BCT",
                        AlgorithmName = "Algorithm"
                    }
                }
            };

            var walletRepository = new Mock<IWalletRepository>();

            walletRepository.Setup(x => x.GetAll()).Returns(walletsList.ToAsyncEnumerable);

            var handler = new GetWalletsQueryHandler(walletRepository.Object);

            // Act
            var result = await handler.Handle(new GetWalletsQuery(), default).ToListAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.ToAsyncEnumerable, Is.InstanceOf<IAsyncEnumerable<Wallet>>(),
                    "Не совпадают типы");
                Assert.That(result, Is.Not.Empty,
                    "Список пулов пуст");
                Assert.That(result, Is.EqualTo(walletsList),
                    "Коллекции не равны");
            });
        }
    }
}
