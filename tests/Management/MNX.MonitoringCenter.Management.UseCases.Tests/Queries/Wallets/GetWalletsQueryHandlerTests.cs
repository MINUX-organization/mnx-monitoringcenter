using AutoMapper;
using MNX.MonitoringCenter.Management.Contracts;
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
        public async Task GetWallets_ReturnsWallets()
        {
            // Arrange
            var walletsList = new List<Wallet>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test1",
                    Address = "Tomsk",
                    CryptocurrencyId = 1,
                    Cryptocurrency = new()
                    {
                        FullName = "Etherium",
                        ShortName = "ETH",
                        AlgorithmName = "Algorithm"
                    }
                },

                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test2",
                    Address = "Moscow",
                    CryptocurrencyId = 2,
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

            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.Map<WalletModel>(It.IsAny<Wallet>()))
                  .Returns<Wallet>(x => new WalletModel()
                  {
                      Id = x.Id,
                      Name = x.Name,
                      Address = x.Address,
                      Cryptocurrency = x.Cryptocurrency?.FullName ?? throw new ArgumentNullException()
                  });

            var handler = new GetWalletsQueryHandler(walletRepository.Object, mapper.Object);

            // Act
            var result = await handler.Handle(new GetWalletsQuery(), default).ToListAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.ToAsyncEnumerable(), Is.InstanceOf<IAsyncEnumerable<WalletModel>>(),
                    "Не совпадают типы");
                Assert.That(result, Is.Not.Empty, "Список пулов пуст");
                Assert.That(result[0].Id, Is.EqualTo(walletsList[0].Id), "Коллекции не равны");
                Assert.That(result[1].Id, Is.EqualTo(walletsList[1].Id), "Коллекции не равны");
            });
        }
    }
}
