using AutoMapper;
using Kernel.UseCases;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using MNX.MonitoringCenter.Management.UseCases.Commands.Pools;
using MNX.MonitoringCenter.Management.UseCases.Commands.Pools.AddPool;
using Moq;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Pools.AddPool;

[TestFixture]
public class AddPoolCommandHandlerTests
{
    private IMapper _mapper;

    private static readonly Guid _poolId = Guid.Parse("f8b51c3b-d4eb-40b1-8465-4d16a79e429b");

    [SetUp]
    public void Setup()
    {
        _mapper = TestHelper.GetMapper();
    }

    [Test]
    public async Task AddPool_ReturnsModel()
    {
        // Arrange
        var poolRepository = new Mock<IPoolRepository>();

        poolRepository.Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<int>()))
                      .ReturnsAsync(false);

        poolRepository.Setup(x => x.Add(It.IsAny<Pool>()))
                      .ReturnsAsync(_poolId);

        var cryptoRepository = new Mock<ICryptocurrencyRepository>();
        cryptoRepository.Setup(x => x.GetById(It.IsAny<Guid>()))
                        .ReturnsAsync(TestHelper.Cryptocurrency);

        var handler = new AddPoolCommandHandler(poolRepository.Object,
                                                cryptoRepository.Object,
                                                _mapper);
        // Act
        var result = await handler.Handle(GetCommand(), default);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True, "Операция завершилась неудачно");

            Assert.That(result.Status, Is.EqualTo(ResultStatus.Created), "Статус результата не 201");

            Assert.IsTrue(result.GetValue().Id == GetPoolModel().Id &&
                          result.GetValue().Domain == GetPoolModel().Domain &&
                          result.GetValue().Port == GetPoolModel().Port &&
                          result.GetValue().Cryptocurrency == GetPoolModel().Cryptocurrency,
                    "Возвращенное значение не совпадает с ожидаемым");
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

        var handler = new AddPoolCommandHandler(poolRepository.Object,
                                                cryptoRepository.Object,
                                                _mapper);

        // Act
        var result = await handler.Handle(GetCommand(), default);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False,
                "Операция завершилась успешно, когда ожидалась неудача");
            Assert.That(result.Errors, Is.Not.Null,
                "Список ошибок пуст");
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Invalid),
                "Статус результата не 400");
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

        var handler = new AddPoolCommandHandler(poolRepository.Object,
                                                cryptoRepository.Object,
                                                _mapper);

        // Act
        var result = await handler.Handle(GetCommand(), default);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False,
                "Операция завершилась успешно, когда ожидалась неудача");
            Assert.That(result.Errors, Is.Not.Null,
                "Список ошибок пуст");
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Invalid),
                "Статус результата не 400");
            Assert.That(result.Errors?.ElementAt(0), Is.EqualTo("Cryptocurrency wasn't found"),
                "Сообщение об ошибке отличается от ожидаемого");
        });

        poolRepository.Verify(x => x.Add(It.IsAny<Pool>()), Times.Never);
    }

    private static AddPoolCommand GetCommand()
    {
        return new AddPoolCommand(new PoolInputModel("domain", 8000, TestHelper.Cryptocurrency.Id));
    }

    private static PoolModel GetPoolModel()
    {
        return new PoolModel()
        {
            Id = _poolId,
            Domain = "domain",
            Port = 8000,
            Cryptocurrency = TestHelper.Cryptocurrency.FullName
        };
    }
}