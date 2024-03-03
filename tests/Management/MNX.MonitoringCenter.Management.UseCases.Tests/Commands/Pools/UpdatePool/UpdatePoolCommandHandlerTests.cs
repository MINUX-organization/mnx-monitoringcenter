using AutoMapper;
using Kernel.UseCases;
using MediatR;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using MNX.MonitoringCenter.Management.UseCases.Commands.Pools;
using MNX.MonitoringCenter.Management.UseCases.Commands.Pools.UpdatePool;
using Moq;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Pools.UpdatePool;

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
        cryptoRepository.Setup(x => x.GetById(It.IsAny<Guid>()))
                        .ReturnsAsync(new Cryptocurrency());

        var mapper = new Mock<IMapper>();
        mapper.Setup(x => x.Map<Pool>(It.IsAny<PoolInputModel>())).Returns(new Pool());

        var handler = new UpdatePoolCommandHandler(poolRepository.Object,
                                                    cryptoRepository.Object,
                                                    mapper.Object);

        // Act
        var result = await handler.Handle(GetCommand(), default);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True,
                "Операция завершилась неудачно");
            Assert.That(result.Errors, Is.Null,
                "Список ошибок не пуст");
            Assert.That(result, Is.TypeOf<Result<Unit>>(),
                "Неверный тип результата");
            Assert.That(result, Is.EqualTo(Result<Unit>.Empty()),
                "Результат не пуст");
            Assert.That(result.Status, Is.EqualTo(ResultStatus.NoContent),
                "Статус результата не 'NoContent'");
        });

        poolRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
        poolRepository.Verify(x => x.Update(It.IsAny<Pool>()), Times.Once);
    }

    [Test]
    public async Task UpdatePool_WhenPoolIsNull_ReturnsError()
    {
        // Arrange
        var poolRepository = new Mock<IPoolRepository>();

        poolRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(null as Pool);

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
            Assert.That(result.IsSuccess, Is.False,
                "Операция завершилась успешно, когда ожидалась неудача");
            Assert.That(result.Errors, Is.Not.Null,
                "Список ошибок пуст");
            Assert.That(result, Is.TypeOf<Result<Unit>>(),
                "Неверный тип результата");
            Assert.That(result, Is.Not.EqualTo(Result<Unit>.Empty()),
                "Результат пуст");
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Invalid),
                "Статус результата не 'Invalid'");
            Assert.That(result.Errors?.ElementAt(0), Is.EqualTo("Pool with this id wasn`t found"),
                 "Сообщение об ошибке отличается от ожидаемого");
        });

        poolRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
        poolRepository.Verify(x => x.Update(It.IsAny<Pool>()), Times.Never);
    }

    [Test]
    public async Task UpdatePool_WhenPoolsAreEquals_ReturnsEmptyResult()
    {
        // Arrange
        var pool = new Pool()
        {
            Id = It.IsAny<Guid>(),
            Domain = "domain",
            Port = 8000,
            CryptocurrencyId = Guid.Parse("f8b51c3b-d4eb-40b1-8465-4d16a79e429a"),
            Cryptocurrency = new()
            {
                Id = Guid.Parse("f8b51c3b-d4eb-40b1-8465-4d16a79e429a"),
                FullName = "Bitcoin",
                ShortName = "BCT",
                Algorithm = "Algorithm"
            }
        };

        var poolRepository = new Mock<IPoolRepository>();

        poolRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(pool);

        poolRepository.Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<int>()))
                    .ReturnsAsync(false);

        var cryptoRepository = new Mock<ICryptocurrencyRepository>();
        cryptoRepository.Setup(x => x.GetById(It.IsAny<Guid>()))
                        .ReturnsAsync(new Cryptocurrency());

        var mapper = new Mock<IMapper>();
        mapper.Setup(x => x.Map<Pool>(It.IsAny<PoolInputModel>())).Returns(new Pool());

        var handler = new UpdatePoolCommandHandler(poolRepository.Object,
                                                    cryptoRepository.Object,
                                                    mapper.Object);

        // Act
        var result = await handler.Handle(GetCommand(), default);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True,
                 "Операция завершилась неудачно");
            Assert.That(result.Errors, Is.Null,
                "Список не ошибок пуст");
            Assert.That(result.Status, Is.EqualTo(ResultStatus.NoContent),
                "Статус результата не 'NoContent'");
            Assert.That(result, Is.EqualTo(Result<Unit>.Empty()),
                "Результат не пуст");
            Assert.That(result, Is.TypeOf<Result<Unit>>(),
                 "Неверный тип результата");
        });

        poolRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
        poolRepository.Verify(x => x.Update(It.IsAny<Pool>()), Times.Never);
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
            Assert.That(result.IsSuccess, Is.False,
                "Операция завершилась успешно, когда ожидалась неудача");
            Assert.That(result.Errors, Is.Not.Null,
                "Список ошибок пуст");
            Assert.That(result.Errors?.ElementAt(0), Is.EqualTo("Pool already exists"),
                 "Сообщение об ошибке отличается от ожидаемого");
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Invalid),
                "Статус результата не 'Invalid'");
            Assert.That(result, Is.TypeOf<Result<Unit>>(),
                 "Неверный тип результата");
        });

        poolRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
        poolRepository.Verify(x => x.Update(It.IsAny<Pool>()), Times.Never);
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
        cryptoRepository.Setup(x => x.GetById(It.IsAny<Guid>()))
                        .ReturnsAsync(null as Cryptocurrency);

        var mapper = new Mock<IMapper>();

        var handler = new UpdatePoolCommandHandler(poolRepository.Object,
                                                    cryptoRepository.Object,
                                                    mapper.Object);

        // Act
        var result = await handler.Handle(GetCommand(), default);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False,
                "Операция завершилась успешно, когда ожидалась неудача");
            Assert.That(result.Errors, Is.Not.Null,
                "Список ошибок пуст");
            Assert.That(result.Errors?.ElementAt(0), Is.EqualTo("Cryptocurrency wasn't found"),
                 "Сообщение об ошибке отличается от ожидаемого");
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Invalid),
                "Статус результата не 'Invalid'");
            Assert.That(result, Is.TypeOf<Result<Unit>>(),
                 "Неверный тип результата");
        });

        poolRepository.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Once);
        poolRepository.Verify(x => x.Update(It.IsAny<Pool>()), Times.Never);
    }

    private static UpdatePoolCommand GetCommand()
    {
        return new UpdatePoolCommand(It.IsAny<Guid>(),
                                    new PoolInputModel("domain",
                                                       8000,
                                                       Guid.Parse("f8b51c3b-d4eb-40b1-8465-4d16a79e429a")));
    }
}