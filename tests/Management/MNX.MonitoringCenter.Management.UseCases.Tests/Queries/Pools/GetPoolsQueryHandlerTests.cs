using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Queries.GetPoolsQuery;
using Moq;

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
            new Pool() 
            { 
                Id = Guid.Parse("d60c4407-ccd5-4cf8-b5a2-064b51c20be9"),
                Domain = "Pool 1", 
                Cryptocurrency = "Bitcoin", 
                Port = 1 
            },

            new Pool() 
            { 
                Id = Guid.Parse("33fef879-06ff-420d-989a-a3b362129d77"),
                Domain = "Pool 2", 
                Cryptocurrency = "Tether", 
                Port = 2 
            },
    };

        var poolRepository = new Mock<IPoolRepository>();

        poolRepository.Setup(x => x.GetAll()).Returns(poolsList.ToAsyncEnumerable);

        var handler = new GetPoolsQueryHandler(poolRepository.Object);    

        // Act
        var result = await handler.Handle(new GetPoolsQuery(), default).ToListAsync();

        // Assert
        Assert.Multiple(() =>
        {     
            Assert.That(result.ToAsyncEnumerable, Is.InstanceOf<IAsyncEnumerable<Pool>>(), 
                "Не совпадают типы");
            Assert.That(result, Is.Not.Empty,
                   "Список пулов пуст");
            Assert.That(result, Is.EqualTo(poolsList),
                "Коллекции не равны");
        });
    }
}