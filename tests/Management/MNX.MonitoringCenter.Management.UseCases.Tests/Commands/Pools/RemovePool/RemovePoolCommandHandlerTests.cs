using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using MNX.MonitoringCenter.Management.UseCases.Commands.Pools.RemovePool;
using Moq;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Pools.RemovePool;

[TestFixture]
public class RemovePoolCommandHandlerTests
{
    [Test]
    public async Task RemovePool_ReturnsEmptyResult()
    {
        //Arrange
        var poolRepository = new Mock<IPoolRepository>();

        poolRepository.Setup(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId)).ReturnsAsync(new Pool());

        poolRepository.Setup(x => x.Remove(It.IsAny<Pool>()));

        var handler = new RemovePoolCommandHandler(poolRepository.Object);

        // Act
        var result = await handler.Handle(GetCommand(), default);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True,
                "Операция завершилась неудачно");
            Assert.That(result.Status, Is.EqualTo(ResultStatus.NoContent),
                "Статус результата не 204");
        });

        poolRepository.Verify(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId), Times.Once);
        poolRepository.Verify(x => x.Remove(It.IsAny<Pool>()), Times.Once);
    }

    [Test]
    public async Task RemovePool_WhenPoolIsNull_ReturnsError()
    {
        var poolRepository = new Mock<IPoolRepository>();

        poolRepository.Setup(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId)).ReturnsAsync(null as Pool);

        var handler = new RemovePoolCommandHandler(poolRepository.Object);

        // Act
        var result = await handler.Handle(GetCommand(), default);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False,
                "Операция была успешной, когда ожидалась неудача");
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Invalid),
                "Статус результата не 400");
            Assert.That(result.Errors?.ElementAt(0), Is.EqualTo("Pool with this id wasn`t found"),
                "Сообщение об ошибке отличается от ожидаемого");
        });

        poolRepository.Verify(x => x.GetAvailableById(It.IsAny<Guid>(), TestHelper.UserId), Times.Once);
        poolRepository.Verify(x => x.Remove(It.IsAny<Pool>()), Times.Never);
    }

    private static RemovePoolCommand GetCommand()
    {
        return new RemovePoolCommand(It.IsAny<Guid>(), TestHelper.Cryptocurrency.UserId);
    }
}