using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts.AlgorithmBinding;
using MNX.MonitoringCenter.Management.Core.Mining;
using MNX.MonitoringCenter.Management.Core.Mining.Miner;
using MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm;
using MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm.Commands.AddAlgorithmCommand;
using Moq;

namespace MNX.MonitoringCenter.Management.UseCases.Tests;

public class AlgorithmUseCasesTests
{
    [Test]
    public static async Task Handle_ShouldReturnConflict_WhenAlgorithmExists()
    {
        // Arrange

        var algorithmRepoMock = new Mock<IAlgorithmRepository>();
        var minerRepoMock = new Mock<IMinerAlgorithmRepository>();

        algorithmRepoMock
            .Setup(x => x.Exists(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new AddAlgorithmCommandHandler(
            minerRepoMock.Object,
            algorithmRepoMock.Object);

        var command = new AddAlgorithmCommand(
            Guid.NewGuid(),
            new AlgorithmBindingModel(
                "TestAlgo",
                []));


        // Act

        var result = await handler.Handle(command, CancellationToken.None);


        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Conflict));
        });
        minerRepoMock.Verify(x => x.AddRangeAsync(It.IsAny<List<MinerAlgorithm>>()), Times.Never);
    }

    [Test]
    public async Task Handle_ShouldCreateAlgorithm_WhenNotExists()
    {
        // Arrange

        var algorithmRepoMock = new Mock<IAlgorithmRepository>();
        var minerRepoMock = new Mock<IMinerAlgorithmRepository>();

        algorithmRepoMock
            .Setup(x => x.Exists(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        algorithmRepoMock
            .Setup(x => x.AddAsync(It.IsAny<Algorithm>()))
            .Returns(Task.CompletedTask);

        minerRepoMock
            .Setup(x => x.AddRangeAsync(It.IsAny<List<MinerAlgorithm>>()))
            .Returns(Task.CompletedTask);

        var handler = new AddAlgorithmCommandHandler(
            minerRepoMock.Object,
            algorithmRepoMock.Object);

        var command = new AddAlgorithmCommand(
            Guid.NewGuid(),
            new AlgorithmBindingModel(
                "TestAlgorithm",
                [
                    new("MinerAlgorithm1", Guid.NewGuid()),
                    new("MinerAlgorithm2", Guid.NewGuid()),
                    new("MinerAlgorithm3", Guid.NewGuid())
                ]));


        // Act

        var result = await handler.Handle(command, CancellationToken.None);


        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Created));
        });
        algorithmRepoMock.Verify(x => x.AddAsync(It.IsAny<Algorithm>()), Times.Once);
        minerRepoMock.Verify(x => x.AddRangeAsync(It.IsAny<List<MinerAlgorithm>>()), Times.Once);
    }
}