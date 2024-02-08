using AutoMapper;
using Kernel.UseCases;
using MediatR;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using MNX.MonitoringCenter.Management.UseCases.Commands.Pools;
using MNX.MonitoringCenter.Management.UseCases.Commands.Pools.UpdatePool;
using Moq;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Pools;

[TestFixture]
public class UpdatePoolCommandHandlerTests
{
    [Test]
    public async Task UpdatePool_ReturnsEmptyResult()
    {
        // Arrange
        var poolRepository = new Mock<IPoolRepository>();

        poolRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(new Pool());

        poolRepository.Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<int>()))
                    .ReturnsAsync(false);

        poolRepository.Setup(x => x.Update(It.IsAny<Pool>()));

        var cryptoRepository = new Mock<ICryptocurrencyRepository>();
        cryptoRepository.Setup(x => x.Exists(It.IsAny<string>(), null))
                        .ReturnsAsync(true);

        var mapper = new Mock<IMapper>();
        mapper.Setup(x => x.Map<Pool>(It.IsAny<PoolModel>())).Returns(new Pool());

        var handler = new UpdatePoolCommandHandler(poolRepository.Object,
                                                    cryptoRepository.Object,
                                                    mapper.Object);

        // Act
        var result = await handler.Handle(GetCommand(), default);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Errors, Is.Null);
            Assert.That(result, Is.TypeOf<Result<Unit>>());
            Assert.That(result, Is.EqualTo(Result<Unit>.Empty()));
            Assert.That(result.Status, Is.EqualTo(ResultStatus.NoContent));
            Assert.That(PoolsAreEquals(new Pool(), new PoolModel("domain", 8000, "Bitcoin")), Is.False);
        });
    }

    [Test]
    public async Task UpdatePool_WhenPoolIsNull_ReturnsError()
    {
        // Arrange
        var poolRepository = new Mock<IPoolRepository>();

        poolRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync((Pool)null);

        var cryptoRepository = new Mock<ICryptocurrencyRepository>();

        var mapper = new Mock<IMapper>();

        var handler = new UpdatePoolCommandHandler(poolRepository.Object,
                                                    cryptoRepository.Object,
                                                    mapper.Object);

        // Act
        var result = await handler.Handle(GetCommand(), default);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Errors, Is.Not.Null);
            Assert.That(result, Is.TypeOf<Result<Unit>>());
            Assert.That(result, Is.Not.EqualTo(Result<Unit>.Empty()));
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Invalid));
            Assert.That(result.Errors?.ElementAt(0), Is.EqualTo("Pool with this id wasn`t found"));
        });
    }

    [Test]
    public async Task UpdatePool_WhenPoolsAreEquals_ReturnsError()
    {
        // Arrange
        var pool = new Pool() 
        { 
            Id = It.IsAny<Guid>(),
            Domain = "domain",
            Port = 8000,
            Cryptocurrency = "Bitcoin"
        };

        var poolRepository = new Mock<IPoolRepository>();

        poolRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(new Pool());

        poolRepository.Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<int>()))
                    .ReturnsAsync(false);

        var cryptoRepository = new Mock<ICryptocurrencyRepository>();
        cryptoRepository.Setup(x => x.Exists(It.IsAny<string>(), null))
                        .ReturnsAsync(true);

        var mapper = new Mock<IMapper>();
        mapper.Setup(x => x.Map<Pool>(It.IsAny<PoolModel>())).Returns(new Pool());

        var handler = new UpdatePoolCommandHandler(poolRepository.Object,
                                                    cryptoRepository.Object,
                                                    mapper.Object);

        // Act
        var result = await handler.Handle(GetCommand(), default);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(PoolsAreEquals(pool, new PoolModel("domain", 8000, "Bitcoin")), Is.True);
            Assert.That(result.Errors, Is.Null);
            Assert.That(result.Status, Is.EqualTo(ResultStatus.NoContent));
            Assert.That(result, Is.EqualTo(Result<Unit>.Empty()));
            Assert.That(result, Is.TypeOf<Result<Unit>>());
        });
    }

    [Test]
    public async Task UpdatePool_WhenPoolAlreadyExists_ReturnsError()
    {
        // Arrange
        var poolRepository = new Mock<IPoolRepository>();

        poolRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(new Pool());

        poolRepository.Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<int>()))
                    .ReturnsAsync(true);

        var cryptoRepository = new Mock<ICryptocurrencyRepository>();

        var mapper = new Mock<IMapper>();

        var handler = new UpdatePoolCommandHandler(poolRepository.Object,
                                                    cryptoRepository.Object,
                                                    mapper.Object);

        // Act
        var result = await handler.Handle(GetCommand(), default);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Errors, Is.Not.Null);
            Assert.That(result.Errors?.ElementAt(0), Is.EqualTo("Pool already exists"));
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Invalid));
            Assert.That(result, Is.TypeOf<Result<Unit>>());
            Assert.That(PoolsAreEquals(new Pool(), new PoolModel("domain", 8000, "Bitcoin")), Is.False);
        });
    }

    [Test]
    public async Task UpdatePool_WhenCoinDoesNotExist_ReturnsError()
    {
        // Arrange
        var poolRepository = new Mock<IPoolRepository>();

        poolRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(new Pool());

        poolRepository.Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<int>()))
                    .ReturnsAsync(false);

        var cryptoRepository = new Mock<ICryptocurrencyRepository>();
        cryptoRepository.Setup(x => x.Exists(It.IsAny<string>(), null))
                        .ReturnsAsync(false);

        var mapper = new Mock<IMapper>();

        var handler = new UpdatePoolCommandHandler(poolRepository.Object,
                                                    cryptoRepository.Object,
                                                    mapper.Object);

        // Act
        var result = await handler.Handle(GetCommand(), default);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Errors, Is.Not.Null);
            Assert.That(result.Errors?.ElementAt(0), Is.EqualTo("Cryptocurrency wasn't found"));
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Invalid));
            Assert.That(result, Is.TypeOf<Result<Unit>>());
            Assert.That(PoolsAreEquals(new Pool(), new PoolModel("domain", 8000, "Bitcoin")), Is.False);
        });
    }

    private static UpdatePoolCommand GetCommand()
    {
        return new UpdatePoolCommand(It.IsAny<Guid>(), new PoolModel("domain", 8000, "Bitcoin"));
    }

    private static bool PoolsAreEquals(Pool pool, PoolModel newPool)
    {
        return pool.Domain == newPool.Domain && pool.Port == newPool.Port;
    }
}