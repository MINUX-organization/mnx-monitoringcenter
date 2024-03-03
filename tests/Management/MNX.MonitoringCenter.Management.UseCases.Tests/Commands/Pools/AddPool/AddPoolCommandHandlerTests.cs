using AutoMapper;
using Kernel.UseCases;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using MNX.MonitoringCenter.Management.UseCases.Commands.Pools;
using MNX.MonitoringCenter.Management.UseCases.Commands.Pools.AddPool;
using Moq;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Pools.AddPool;

[TestFixture]
public class AddPoolCommandHandlerTests
{
    [Test]
    public async Task AddPool_ReturnsId()
    {
        // Arrange
        var poolRepository = new Mock<IPoolRepository>();

        poolRepository.Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<int>()))
                      .ReturnsAsync(false);

        poolRepository.Setup(x => x.Add(It.IsAny<Pool>()))
                      .ReturnsAsync(It.IsAny<Guid>());

        var cryptoRepository = new Mock<ICryptocurrencyRepository>();
        cryptoRepository.Setup(x => x.GetById(It.IsAny<Guid>()))
                        .ReturnsAsync(new Cryptocurrency());

        var mapper = new Mock<IMapper>();
        mapper.Setup(x => x.Map<Pool>(It.IsAny<PoolInputModel>())).Returns(new Pool());

        var handler = new AddPoolCommandHandler(poolRepository.Object,
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
            Assert.That(result, Is.TypeOf<Result<Guid>>(),
                "Неверный тип результата");
            Assert.That(result, Is.EqualTo(Result<Guid>.SuccessfullyCreated(It.IsAny<Guid>())),
                "Результат не соответствует успешному созданию");
            Assert.That(result.GetValue(), Is.EqualTo(It.IsAny<Guid>()),
                "Возвращенное значение не совпадает с ожидаемым идентификатором");
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Created),
                "Статус результата не 'Created'");
        });

        poolRepository.Verify(x => x.Add(It.IsAny<Pool>()), Times.Once);
    }

    [Test]
    public async Task AddPool_WhenPoolAlreadyExists_ReturnsError()
    {
        // Arrange
        var poolRepository = new Mock<IPoolRepository>();

        poolRepository.Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<int>()))
                      .ReturnsAsync(true);

        var cryptoRepository = new Mock<ICryptocurrencyRepository>();
        var mapper = new Mock<IMapper>();

        var handler = new AddPoolCommandHandler(poolRepository.Object,
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
            Assert.That(result, Is.TypeOf<Result<Guid>>(),
                "Неверный тип результата");
            Assert.That(result, Is.Not.EqualTo(Result<Guid>.Empty()),
                "Результат пуст");
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Invalid),
                "Статус результата не 'Invalid'");
            Assert.That(result.Errors?.ElementAt(0), Is.EqualTo("Pool already exists"),
                "Сообщение об ошибке отличается от ожидаемого");
        });

        poolRepository.Verify(x => x.Add(It.IsAny<Pool>()), Times.Never);
    }

    [Test]
    public async Task AddPool_WhenCoinDoesNotExist_ReturnsError()
    {
        // Arrange
        var poolRepository = new Mock<IPoolRepository>();

        poolRepository.Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<int>()))
                      .ReturnsAsync(false);

        var cryptoRepository = new Mock<ICryptocurrencyRepository>();
        cryptoRepository.Setup(x => x.GetById(It.IsAny<Guid>()))
                        .ReturnsAsync(null as Cryptocurrency);

        var mapper = new Mock<IMapper>();

        var handler = new AddPoolCommandHandler(poolRepository.Object,
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
            Assert.That(result, Is.TypeOf<Result<Guid>>(),
                "Неверный тип результата");
            Assert.That(result, Is.Not.EqualTo(Result<Guid>.Empty()),
                "Результат пуст");
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Invalid),
                "Статус результата не 'Invalid'");
            Assert.That(result.Errors?.ElementAt(0), Is.EqualTo("Cryptocurrency wasn't found"),
                "Сообщение об ошибке отличается от ожидаемого");
        });

        poolRepository.Verify(x => x.Add(It.IsAny<Pool>()), Times.Never);
    }

    private static AddPoolCommand GetCommand()
    {
        return new AddPoolCommand(new PoolInputModel("domain", 8000, Guid.Parse("f8b51c3b-d4eb-40b1-8465-4d16a79e429a")));
    }
}