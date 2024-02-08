using Kernel.UseCases;
using MediatR;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using MNX.MonitoringCenter.Management.UseCases.Commands.Pools.RemovePool;
using Moq;

namespace MNX.MonitoringCenter.Management.UseCases.Tests.Commands.Pools;

[TestFixture]
public class RemovePoolCommandHandlerTests
{
    [Test]
    public async Task RemovePool_ReturnsEmptyResult()
    {
        //Arrange
        var poolRepository = new Mock<IPoolRepository>();

        poolRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(new Pool());

        var handler = new RemovePoolCommandHandler(poolRepository.Object);

        // Act
        var result = await handler.Handle(GetCommand(), default);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Errors, Is.Null);
            Assert.That(result, Is.EqualTo(Result<Unit>.Empty()));
            Assert.That(result, Is.TypeOf<Result<Unit>>());
            Assert.That(result.Status, Is.EqualTo(ResultStatus.NoContent));
        });
    }

    [Test]
    public async Task RemovePool_WhenPoolIsNull_ReturnsError()
    {
        var poolRepository = new Mock<IPoolRepository>();

        poolRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync((Pool)null);

        var handler = new RemovePoolCommandHandler(poolRepository.Object);

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

    private static RemovePoolCommand GetCommand()
    {
        return new RemovePoolCommand(It.IsAny<Guid>());
    }
}