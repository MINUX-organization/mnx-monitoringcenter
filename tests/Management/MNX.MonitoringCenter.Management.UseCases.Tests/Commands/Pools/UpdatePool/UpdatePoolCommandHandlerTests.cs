using AutoMapper;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Commands.Pools;
using MNX.MonitoringCenter.Management.UseCases.Commands.Pools.UpdatePool;
using MNX.MonitoringCenter.Management.UseCases.Cryptocurrency;
using MNX.MonitoringCenter.Management.UseCases.Pool;
using MNX.MonitoringCenter.Management.UseCases.Pool.Commands.UpdatePool;
using MNX.MonitoringCenter.Management.UseCases.Pool.UpdatePool;
using Moq;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Pools.UpdatePool;

[TestFixture]
public class UpdatePoolCommandHandlerTests
{
    private IMapper _mapper;

    private static readonly Guid _poolId = Guid.Parse("f8b51c3b-d4eb-40b1-8465-4d16a79e429b");

    [SetUp]
    public void Setup()
    {
        _mapper = TestHelper.GetMapper();
    }

    [Test]
    public async Task UpdatePool_ReturnsPoolModel()
    {
        // Arrange
        var poolRepository = new Mock<IPoolRepository>();

        poolRepository.Setup(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId))
                      .ReturnsAsync(GetPool());

        poolRepository.Setup(x => x.Exists(TestHelper.UserId, It.IsAny<string>(), It.IsAny<int>()))
                      .ReturnsAsync(false);

        poolRepository.Setup(x => x.Update(It.IsAny<Pool>()));

        var cryptoRepository = new Mock<ICryptocurrencyRepository>();
        cryptoRepository.Setup(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.Cryptocurrency.UserId))
                        .ReturnsAsync(TestHelper.Cryptocurrency);

        var handler = new UpdatePoolCommandHandler(poolRepository.Object,
                                                   cryptoRepository.Object,
                                                   _mapper);

        // Act
        var result = await handler.Handle(GetCommand("new_domain", 8001, TestHelper.Cryptocurrency.Id), default);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True, "Операция завершилась неудачно");

            Assert.IsTrue(result.GetValue().Id == _poolId &&
                          result.GetValue().Domain == "new_domain" &&
                          result.GetValue().Port == 8001 &&
                          result.GetValue().Cryptocurrency == TestHelper.Cryptocurrency.FullName,
                   "Результат не соответствует ожиданию");
        });

        poolRepository.Verify(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId), Times.Once);
        poolRepository.Verify(x => x.Update(It.IsAny<Pool>()), Times.Once);
    }

    [Test]
    public async Task UpdatePool_WhenPoolIsNull_ReturnsError()
    {
        // Arrange
        var poolRepository = new Mock<IPoolRepository>();

        poolRepository.Setup(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId))
            .ReturnsAsync(null as Pool);

        var cryptoRepository = new Mock<ICryptocurrencyRepository>();

        var handler = new UpdatePoolCommandHandler(poolRepository.Object,
                                                   cryptoRepository.Object,
                                                   _mapper);

        // Act
        var result = await handler.Handle(GetCommand("new_domain", 8001, TestHelper.Cryptocurrency.Id), default);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False,
                "Операция завершилась успешно, когда ожидалась неудача");
            Assert.That(result.Errors, Is.Not.Null,
                "Список ошибок пуст");
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Invalid),
                "Статус результата не 400");
            Assert.That(result.Errors?.ElementAt(0), Is.EqualTo("Pool with this id wasn`t found"),
                 "Сообщение об ошибке отличается от ожидаемого");
        });

        poolRepository.Verify(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId), Times.Once);
        poolRepository.Verify(x => x.Update(It.IsAny<Pool>()), Times.Never);
    }

    [Test]
    public async Task UpdatePool_WhenPoolsAreEquals_ReturnsModel()
    {
        // Arrange

        var poolRepository = new Mock<IPoolRepository>();

        poolRepository.Setup(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId)).ReturnsAsync(GetPool());

        poolRepository.Setup(x => x.Exists(TestHelper.UserId, It.IsAny<string>(), It.IsAny<int>()))
                      .ReturnsAsync(false);

        var cryptoRepository = new Mock<ICryptocurrencyRepository>();
        cryptoRepository.Setup(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.Cryptocurrency.UserId))
                        .ReturnsAsync(TestHelper.Cryptocurrency);

        var handler = new UpdatePoolCommandHandler(poolRepository.Object,
                                                   cryptoRepository.Object,
                                                   _mapper);

        // Act
        var result = await handler.Handle(GetCommand("domain", 8000, TestHelper.Cryptocurrency.Id), default);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True, "Операция завершилась неудачно");

            Assert.That(result.Errors, Is.Null, "Список не ошибок пуст");

            Assert.That(result.Status, Is.EqualTo(ResultStatus.Ok), "Статус результата не 200");

            Assert.IsTrue(result.GetValue().Id == _poolId &&
                          result.GetValue().Domain == "domain" &&
                          result.GetValue().Port == 8000 &&
                          result.GetValue().Cryptocurrency == TestHelper.Cryptocurrency.FullName,
                   "Результат не соответствует ожиданию");
        });

        poolRepository.Verify(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId), Times.Once);
        poolRepository.Verify(x => x.Update(It.IsAny<Pool>()), Times.Never);
    }

    [Test]
    public async Task UpdatePool_WhenPoolAlreadyExists_ReturnsError()
    {
        // Arrange
        var poolRepository = new Mock<IPoolRepository>();

        poolRepository.Setup(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId))
                      .ReturnsAsync(GetPool());

        poolRepository.Setup(x => x.Exists(TestHelper.UserId, It.IsAny<string>(), It.IsAny<int>()))
                      .ReturnsAsync(true);

        var cryptoRepository = new Mock<ICryptocurrencyRepository>();

        var handler = new UpdatePoolCommandHandler(poolRepository.Object,
                                                   cryptoRepository.Object,
                                                   _mapper);

        // Act
        var result = await handler.Handle(GetCommand("new_domain", 8001, TestHelper.Cryptocurrency.Id), default);

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
                "Статус результата не 400");
        });

        poolRepository.Verify(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId), Times.Once);
        poolRepository.Verify(x => x.Update(It.IsAny<Pool>()), Times.Never);
    }

    [Test]
    public async Task UpdatePool_WhenCoinDoesNotExist_ReturnsError()
    {
        // Arrange
        var poolRepository = new Mock<IPoolRepository>();

        poolRepository.Setup(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId))
                      .ReturnsAsync(GetPool());

        poolRepository.Setup(x => x.Exists(TestHelper.UserId, It.IsAny<string>(), It.IsAny<int>()))
                    .ReturnsAsync(false);

        var cryptoRepository = new Mock<ICryptocurrencyRepository>();
        cryptoRepository.Setup(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.Cryptocurrency.UserId))
                        .ReturnsAsync(null as Cryptocurrency);

        var handler = new UpdatePoolCommandHandler(poolRepository.Object,
                                                   cryptoRepository.Object,
                                                   _mapper);

        // Act
        var result = await handler.Handle(GetCommand("new_domain", 8001, TestHelper.Cryptocurrency.Id), default);

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
                "Статус результата не 400");
        });

        poolRepository.Verify(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId), Times.Once);
        poolRepository.Verify(x => x.Update(It.IsAny<Pool>()), Times.Never);
    }

    private static UpdatePoolCommand GetCommand(string domain, int port, Guid cryptoId)
    {
        return new UpdatePoolCommand(_poolId, new PoolInputModel(domain, port, cryptoId), TestHelper.UserId);
    }

    private static Pool GetPool()
    {
        return new Pool()
        {
            Id = _poolId,
            Domain = "domain",
            Port = 8000,
            CryptocurrencyId = TestHelper.Cryptocurrency.Id,
            Cryptocurrency = TestHelper.Cryptocurrency
        };
    }
}