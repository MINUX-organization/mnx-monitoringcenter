using Microsoft.Extensions.DependencyInjection;
using MNX.MonitoringCenter.Management.Agent.Commands.Mining;
using MNX.MonitoringCenter.RigsApi.Core;
using MNX.MonitoringCenter.RigsApi.Core.DomainEvents.Mining;
using MNX.MonitoringCenter.RigsApi.Core.Services;
using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;
using MNX.MonitoringCenter.RigsApi.GrainWrapper;
using MNX.MonitoringCenter.RigsApi.GrainWrapper.Services;
using MNX.MonitoringCenter.RigsApi.Service.Consumers;
using MNX.MonitoringCenter.RigsApi.UseCases.RigState.Mining;
using Moq;

namespace MNX.MonitoringCenter.RigsApi.Service.Tests.StopMiningCommandResultConsumer;

internal class StopMiningCommandResultConsumerTests
{
    private IMessageBrokerFixture _broker;
    private Mock<IRigGrainFactory> _grainFactoryMock;
    private Mock<IRigRepository> _repositoryMock;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _broker = GlobalMessageBrokerFixture.Broker;
    }

    [SetUp]
    public Task SetUp()
    {
        _grainFactoryMock = new Mock<IRigGrainFactory>();
        _repositoryMock = new Mock<IRigRepository>();
        return Task.CompletedTask;
    }

    [TearDown]
    public Task TearDown()
    {
        return MiningStoppedEventHandler.ClearEvents();
    }

    [Test]
    public async Task Consume_RigExists_ShouldProcessMessage()
    {
        // Arrange

        var rig = new Rig(new RigId(Guid.NewGuid()), Guid.NewGuid(), "RigName_1");
        rig.InitiateStartMining(rig.OwnerId);
        rig.StartMining();

        _repositoryMock.Setup(x => x.Update(
            It.Is<Rig>(x => x == rig),
            It.Is<CancellationToken>(x => x == default)));

        IRigGrain? capturedGrain = null;

        var grain = new RigGrain(rig, _repositoryMock.Object);
        _grainFactoryMock.Setup(x => x.GetGrain(
            It.Is<RigId>(x => x == rig.Id),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(grain)
            .Callback<RigId, CancellationToken>((id, token) =>
            {
                capturedGrain = grain;
            });

        var host = TestsEnvironmentService.CreateHost<StopMiningCommandResult>(
            _broker, Registrations(), typeof(UseCases.RigState.Mining.StopMiningCommand));
        var queueName = $"queue_{rig.Id}";

        await host.StartAsync(queueName);

        var message = new StopMiningCommandResult(rig.Id, true);


        // Act

        await TestsEnvironmentService.Publish(queueName, message, _broker);


        // Assert

        Assert.That(capturedGrain, Is.Not.Null);

        _repositoryMock.Verify(x => x.Update(
            It.Is<Rig>(x => x == rig),
            It.Is<CancellationToken>(x => x == default)),
            Times.Once);
        _grainFactoryMock.Verify(x => x.GetGrain(
            It.Is<RigId>(x => x == rig.Id),
            It.IsAny<CancellationToken>()),
            Times.Once);

        await host.DisposeAsync();
    }

    [Test]
    public async Task Consume_RigExists_ShouldThrowException()
    {
        // Arrange

        var rig = new Rig(new RigId(Guid.NewGuid()), Guid.NewGuid(), "RigName_1");
        rig.InitiateStartMining(rig.OwnerId);
        rig.StartMining();

        _repositoryMock.Setup(x => x.Update(
            It.Is<Rig>(x => x == rig),
            It.Is<CancellationToken>(x => x == default)))
            .ThrowsAsync(new Exception("Some exception"));

        IRigGrain? capturedGrain = null;

        var grain = new RigGrain(rig, _repositoryMock.Object);
        _grainFactoryMock.Setup(x => x.GetGrain(
            It.Is<RigId>(x => x == rig.Id),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(grain)
            .Callback<RigId, CancellationToken>((id, token) =>
            {
                capturedGrain = grain;
            });

        var host = TestsEnvironmentService.CreateHost<StopMiningCommandResult>(
            _broker, Registrations(), typeof(UseCases.RigState.Mining.StopMiningCommand));
        var queueName = $"queue_{rig.Id}";

        await host.StartAsync(queueName);

        var message = new StopMiningCommandResult(rig.Id, true);


        // Act

        await TestsEnvironmentService.Publish(queueName, message, _broker);


        // Assert

        Assert.That(capturedGrain, Is.Not.Null);

        _repositoryMock.Verify(x => x.Update(
            It.Is<Rig>(x => x == rig),
            It.Is<CancellationToken>(x => x == default)),
            Times.Once);
        _grainFactoryMock.Verify(x => x.GetGrain(
            It.Is<RigId>(x => x == rig.Id),
            It.IsAny<CancellationToken>()),
            Times.Once);

        await host.DisposeAsync();
    }

    [Test]
    public async Task Consume_RigDoesNotExists_ShouldNotProcessMessage()
    {
        // Arrange

        var rigId = new RigId(Guid.NewGuid());

        _grainFactoryMock.Setup(x => x.GetGrain(
            It.Is<RigId>(x => x == rigId),
            It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<IRigGrain?>(null));

        _repositoryMock.Setup(x => x.Update(
            It.IsAny<Rig>(),
            It.Is<CancellationToken>(x => x == default)));

        var host = TestsEnvironmentService.CreateHost<StopMiningCommandResult>(
            _broker, Registrations(), typeof(UseCases.RigState.Mining.StopMiningCommand));
        var queueName = $"queue_{Guid.NewGuid()}";

        await host.StartAsync(queueName);

        var message = new StopMiningCommandResult(rigId, true);


        // Act

        await TestsEnvironmentService.Publish(queueName, message, _broker);


        // Assert

        _grainFactoryMock.Verify(x => x.GetGrain(
            It.Is<RigId>(x => x == rigId),
            It.IsAny<CancellationToken>()),
            Times.Once);
        _repositoryMock.Verify(x => x.Update(
            It.IsAny<Rig>(),
            It.Is<CancellationToken>(x => x == default)),
            Times.Never);

        await host.DisposeAsync();
    }

    [Test]
    public async Task ConsumeUnsuccessfully_RigFound_ShouldProcessMessage()
    {
        // Arrange

        var rig = new Rig(new RigId(Guid.NewGuid()), Guid.NewGuid(), "RigName_1");
        rig.InitiateStartMining(rig.OwnerId);
        rig.StartMining();
        rig.InitiateStopMining(rig.OwnerId);

        IRigGrain? capturedGrain = null;

        var grain = new RigGrain(rig, _repositoryMock.Object);
        _grainFactoryMock
            .Setup(x => x.GetGrain(rig.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(grain)
            .Callback<RigId, CancellationToken>((id, token) =>
            {
                capturedGrain = grain;
            });

        _repositoryMock.Setup(x => x.Update(rig, default)).Returns(Task.CompletedTask);

        var host = TestsEnvironmentService.CreateHost<StopMiningCommandResult>(
            _broker, Registrations(), typeof(TerminateStopMiningCommand));
        var queueName = $"rig_{rig.Id}";

        await host.StartAsync(queueName);
        var message = new StopMiningCommandResult(rig.Id, false);


        // Act

        await TestsEnvironmentService.Publish(
            queueName, message, _broker);


        // Assert

        Assert.That(capturedGrain, Is.Not.Null);

        _grainFactoryMock.Verify(x => x.GetGrain(rig.Id, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(x => x.Update(rig, default), Times.Once);

        await host.DisposeAsync();
    }

    [Test]
    public async Task ConsumeUnsuccessfully_RigFound_ShouldThrowException()
    {
        // Arrange

        var rig = new Rig(new RigId(Guid.NewGuid()), Guid.NewGuid(), "RigName_1");
        rig.InitiateStartMining(rig.OwnerId);
        rig.StartMining();
        rig.InitiateStopMining(rig.OwnerId);

        IRigGrain? capturedGrain = null;

        var grain = new RigGrain(rig, _repositoryMock.Object);
        _grainFactoryMock
            .Setup(x => x.GetGrain(rig.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(grain)
            .Callback<RigId, CancellationToken>((id, token) =>
            {
                capturedGrain = grain;
            });

        _repositoryMock.Setup(x => x.Update(rig, default)).ThrowsAsync(new Exception("Test exception"));

        var host = TestsEnvironmentService.CreateHost<StopMiningCommandResult>(
            _broker, Registrations(), typeof(TerminateStopMiningCommand));
        var queueName = $"rig_{rig.Id}";

        await host.StartAsync(queueName);
        var message = new StopMiningCommandResult(rig.Id, false);


        // Act

        await TestsEnvironmentService.Publish(queueName, message, _broker);


        // Assert

        Assert.That(capturedGrain, Is.Not.Null);

        _grainFactoryMock.Verify(x => x.GetGrain(rig.Id, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(x => x.Update(rig, default), Times.Once);

        await host.DisposeAsync();
    }

    [Test]
    public async Task ConsumeUnsuccessfully_RigNotFound_ShouldNotProcessMessage()
    {
        // Arrange

        var rigId = new RigId(Guid.NewGuid());

        _grainFactoryMock.Setup(x => x.GetGrain(rigId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((IRigGrain?)null);
        _repositoryMock.Setup(x => x.Update(It.IsAny<Rig>(), default)).Returns(Task.CompletedTask);

        var host = TestsEnvironmentService.CreateHost<StopMiningCommandResult>(
            _broker, Registrations(), typeof(TerminateStopMiningCommand));
        var queueName = $"rig_{rigId}";

        await host.StartAsync(queueName);
        var message = new StopMiningCommandResult(rigId, false);


        // Act

        await TestsEnvironmentService.Publish(queueName, message, _broker);


        // Assert

        _grainFactoryMock.Verify(x => x.GetGrain(rigId, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(x => x.Update(It.IsAny<Rig>(), default), Times.Never);

        await host.DisposeAsync();
    }

    private Action<IServiceCollection> Registrations()
    {
        return services =>
        {
            services.AddEventHandler<MiningStoppedEvent, MiningStoppedEventHandler>();
            services.AddConsumer<StopMiningCommandResult, AgentMsgConsumer>();
            services.AddScoped(_ => _repositoryMock.Object);
            services.AddScoped(_ => _grainFactoryMock.Object);
        };
    }
}
