using AutoMapper;
using Kernel.UseCases;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using MNX.MonitoringCenter.Management.UseCases.Commands.Pools;
using MNX.MonitoringCenter.Management.UseCases.Commands.Pools.AddPool;
using Moq;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Pools;

public class AddPoolCommandHandlerTests
{
    [Test]
    public async Task AddPool_ReturnsId()
    {
        Guid id = Guid.Parse("4d0b4812-6d2e-4d38-85c5-ac2c7871e000");

        var poolRepository = new Mock<IPoolRepository>();

        poolRepository.Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<int>()))
                      .ReturnsAsync(false);

        poolRepository.Setup(x => x.Add(It.IsAny<Pool>()))
                      .ReturnsAsync(id);

        var cryptoRepository = new Mock<ICryptocurrencyRepository>();
        cryptoRepository.Setup(x => x.Exists(It.IsAny<string>(), null))
                        .ReturnsAsync(true);

        var mapper = new Mock<IMapper>();
        mapper.Setup(x => x.Map<Pool>(It.IsAny<PoolModel>())).Returns(new Pool());

        var handler = new AddPoolCommandHandler(poolRepository.Object,
                                                cryptoRepository.Object,
                                                mapper.Object);

        var result = await handler.Handle(GetCommand(), default);

        Assert.NotNull(result);
        Assert.IsTrue(result.IsSuccess);
        Assert.That(result.GetValue(), Is.EqualTo(id));
    }

    [Test]
    public async Task AddPool_WhenPoolAlreadyExists_ReturnsError()
    {
        var poolRepository = new Mock<IPoolRepository>();

        poolRepository.Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<int>()))
                      .ReturnsAsync(true);

        var cryptoRepository = new Mock<ICryptocurrencyRepository>();
        var mapper = new Mock<IMapper>();

        var handler = new AddPoolCommandHandler(poolRepository.Object,
                                                cryptoRepository.Object,
                                                mapper.Object);

        var result = await handler.Handle(GetCommand(), default);

        Assert.NotNull(result);
        Assert.IsFalse(result.IsSuccess);
        Assert.NotNull(result.Errors);
        Assert.That(result.Status, Is.EqualTo(ResultStatus.Invalid));
        Assert.That(result.Errors?.ElementAt(0), Is.EqualTo("Pool already exists"));
    }

    [Test]
    public async Task AddPool_WhenCoinDoesNotExist_ReturnsError()
    {
        var poolRepository = new Mock<IPoolRepository>();

        poolRepository.Setup(x => x.Exists(It.IsAny<string>(), It.IsAny<int>()))
                      .ReturnsAsync(false);

        var cryptoRepository = new Mock<ICryptocurrencyRepository>();
        cryptoRepository.Setup(x => x.Exists(It.IsAny<string>(), null))
                        .ReturnsAsync(false);

        var mapper = new Mock<IMapper>();

        var handler = new AddPoolCommandHandler(poolRepository.Object,
                                                cryptoRepository.Object,
                                                mapper.Object);

        var result = await handler.Handle(GetCommand(), default);

        Assert.NotNull(result);
        Assert.IsFalse(result.IsSuccess);
        Assert.NotNull(result.Errors);
        Assert.That(result.Status, Is.EqualTo(ResultStatus.Invalid));
        Assert.That(result.Errors?.ElementAt(0), Is.EqualTo("Cryptocurrency wasn't found"));
    }

    private static AddPoolCommand GetCommand()
    {
        return new AddPoolCommand(new PoolModel("domain",  8000, "Bitcoin"));
    }
}
