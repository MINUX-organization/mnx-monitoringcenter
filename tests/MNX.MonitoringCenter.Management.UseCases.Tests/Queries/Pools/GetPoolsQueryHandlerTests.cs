using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Queries.GetPoolsQuery;
using Moq;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Queries.Pools;

[TestFixture]
public class GetPoolsQueryHandlerTests
{
    private static readonly Pool testPool1 = 
        new() { Id = Guid.Parse("d60c4407-ccd5-4cf8-b5a2-064b51c20be9"), 
            Domain = "Pool 1", Cryptocurrency = "Bitcoin", Port = 1 };

    private static readonly Pool testPool2 = 
        new() { Id = Guid.Parse("33fef879-06ff-420d-989a-a3b362129d77"), 
            Domain = "Pool 2", Cryptocurrency = "Tether", Port = 2 };

    private static readonly Pool testPool3 = 
        new() { Id = Guid.Parse("eba6b890-2b31-4e73-8908-00170558ea32"), 
            Domain = "Pool 3", Cryptocurrency = "Ethereum", Port = 3 };

    [Test]
    public async Task GetPools_ReturnsPools()
    {
        //Arrange
        var poolRepository = new Mock<IPoolRepository>();

        poolRepository.Setup(x => x.GetAll()).Returns(GetTestPools());

        var handler = new GetPoolsQueryHandler(poolRepository.Object);    

        // Act
        var result = handler.Handle(new GetPoolsQuery(), default);

        var poolList = new List<Pool>();

        await foreach (var pool in result)
        {
            poolList.Add(pool);
        }

        // Assert
        Assert.Multiple(() =>
        {     
            Assert.That(result, Is.InstanceOf<IAsyncEnumerable<Pool>>(), 
                "Не совпадают типы");
            Assert.That(poolList, Is.Not.Empty,
                "Список пулов пуст");
            Assert.That(poolList[0], Is.EqualTo(testPool1),
                "Пул 1 не соответствует ожидаемому");
            Assert.That(poolList[1], Is.EqualTo(testPool2),
                "Пул 2 не соответствует ожидаемому");
            Assert.That(poolList[2], Is.EqualTo(testPool3),
                "Пул 3 не соответствует ожидаемому");
            Assert.That(poolList, Has.Count.EqualTo(3),
                "Неверное количество объектов в результате");
        });
    }

    private async static IAsyncEnumerable<Pool> GetTestPools()
    {
        var pools = new List<Pool>() { testPool1, testPool2, testPool3 };

        foreach (var pool in pools)
        {
            yield return pool;
        }
    }
}