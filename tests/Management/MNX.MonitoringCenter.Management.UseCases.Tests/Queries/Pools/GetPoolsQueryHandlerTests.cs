using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Queries.GetPoolsQuery;
using Moq;
using AutoMapper;
using MNX.MonitoringCenter.Management.Contracts;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Queries.Pools;

[TestFixture]
public class GetPoolsQueryHandlerTests
{
    [Test]
    public async Task GetPools_ReturnsPools()
    {
        //Arrange
        var poolsList = new List<Pool>()
        {
            new() 
            { 
                Id = Guid.Parse("d60c4407-ccd5-4cf8-b5a2-064b51c20be9"),
                Domain = "Pool 1",
                Port = 1,
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
                Id = Guid.Parse("33fef879-06ff-420d-989a-a3b362129d77"),
                Domain = "Pool 2",
                Port = 2,
                CryptocurrencyId = 2,
                Cryptocurrency = new()
                {
                    FullName = "Bitcoin",
                    ShortName = "BCT",
                    AlgorithmName = "Algorithm"
                }
            },
    };

        var poolRepository = new Mock<IPoolRepository>();
        poolRepository.Setup(x => x.GetAll()).Returns(poolsList.ToAsyncEnumerable);

        var mapper = new Mock<IMapper>();
        mapper.Setup(x => x.Map<PoolModel>(It.IsAny<Pool>()))
              .Returns<Pool>(x => new PoolModel()
              {
                  Id = x.Id,
                  Domain = x.Domain,
                  Port = x.Port,
                  Cryptocurrency = x.Cryptocurrency?.FullName ?? throw new ArgumentNullException()
              });

        var handler = new GetPoolsQueryHandler(poolRepository.Object, mapper.Object);

        // Act
        var result = await handler.Handle(new GetPoolsQuery(), default).ToListAsync();

        // Assert
        Assert.Multiple(() =>
        {     
            Assert.That(result.ToAsyncEnumerable(), Is.InstanceOf<IAsyncEnumerable<PoolModel>>(),
                "Не совпадают типы");
            Assert.That(result, Is.Not.Empty, "Список пулов пуст");

            Assert.That(result[0].Id, Is.EqualTo(poolsList[0].Id), "Коллекции не равны");
            Assert.That(result[1].Id, Is.EqualTo(poolsList[1].Id), "Коллекции не равны");
        });
    }
}