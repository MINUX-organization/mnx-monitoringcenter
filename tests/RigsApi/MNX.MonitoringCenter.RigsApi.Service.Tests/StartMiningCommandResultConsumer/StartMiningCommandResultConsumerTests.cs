using FluentAssertions;
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

namespace MNX.MonitoringCenter.RigsApi.Service.Tests.StartMiningCommandResultConsumer;

internal class StartMiningCommandResultConsumerTests
{
    private IMessageBrokerFixture _broker;
    private Mock<IRigRepository> _repositoryMock;
    private Mock<IRigGrainFactory> _grainFactoryMock;

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
        return MiningStartedEventHandler.ClearEvents();
    }

    [Test]
    public async Task ConsumeSuccessfully_RigFound_ShouldProcessMessage()
    {
        // Arrange

        var rig = new Rig(new RigId(Guid.NewGuid()), Guid.NewGuid(), "RigName_1");

        _repositoryMock
            .Setup(x => x.Update(rig, default))
            .Returns(Task.CompletedTask);

        IRigGrain? capturedRigGrain = null;

        var grain = new RigGrain(rig, _repositoryMock.Object);
        _grainFactoryMock
            .Setup(x => x.GetGrain(rig.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(grain)
            .Callback<RigId, CancellationToken>((id, token) =>
            {
                capturedRigGrain = grain;
            });

        var host = ConsumerService.CreateHost<StartMiningCommandResult>(
            _broker, Registrations(), typeof(UseCases.RigState.Mining.StartMiningCommand));
        var queueName = $"rig_inventory_queue_{rig.Id}";

        await host.StartAsync(queueName);

        var message = new StartMiningCommandResult(rig.Id, true);


        // Act

        await ConsumerService.PublishAsync(queueName, ConsumerService.Serialize(message)!, _broker);


        // Assert

        Assert.That(capturedRigGrain, Is.Not.Null);
        MiningStartedEventHandler.Events.Should().HaveCount(1);

        _repositoryMock.Verify(x => x.Update(rig, default), Times.Once);
        _grainFactoryMock.Verify(x => x.GetGrain(rig.Id, It.IsAny<CancellationToken>()), Times.Once);

        await host.DisposeAsync();
    }

    [Test]
    public async Task ConsumeSuccessfully_RigFound_ShouldThrowException()
    {
        // Arrange

        var rig = new Rig(new RigId(Guid.NewGuid()), Guid.NewGuid(), "RigName_1");

        _repositoryMock.Setup(x => x.Update(
            It.Is<Rig>(x => x == rig), default))
            .ThrowsAsync(new Exception("Some exception"));

        IRigGrain? capturedRigGrain = null!;

        var grain = new RigGrain(rig, _repositoryMock.Object);
        _grainFactoryMock.Setup(x => x.GetGrain(
            It.Is<RigId>(x => x == rig.Id),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(grain)
            .Callback<RigId, CancellationToken>((id, token) =>
            {
                capturedRigGrain = grain;
            });

        var host = ConsumerService.CreateHost<StartMiningCommandResult>(
            _broker, Registrations(), typeof(UseCases.RigState.Mining.StartMiningCommand));

        var queueName = $"rig_inventory_queue_{rig.Id}";
        await host.StartAsync(queueName);

        var message = new StartMiningCommandResult(rig.Id, true);


        // Act

        await ConsumerService.PublishAsync(queueName, ConsumerService.Serialize(message)!, _broker);


        // Assert

        Assert.That(capturedRigGrain, Is.Not.Null);
        MiningStartedEventHandler.Events.Should().BeEmpty();

        _repositoryMock.Verify(x => x.Update(
            It.Is<Rig>(x => x == rig), default),
            Times.Once);

        _grainFactoryMock.Verify(x => x.GetGrain(
            It.Is<RigId>(x => x == rig.Id),
            It.IsAny<CancellationToken>()),
            Times.Once);

        await host.DisposeAsync();
    }

    [Test]
    public async Task ConsumeSuccessfully_RigNotFound_ShouldNotProcessMessage()
    {
        // Arrange

        var rig = new Rig(new RigId(Guid.NewGuid()), Guid.NewGuid(), "RigName_1");

        _repositoryMock.Setup(x => x.Update(
            It.Is<Rig>(x => x == rig),
            default))
            .Returns(Task.CompletedTask);

        _grainFactoryMock.Setup(x => x.GetGrain(
            It.Is<RigId>(x => x == rig.Id),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync((IRigGrain?)null);

        var host = ConsumerService.CreateHost<StartMiningCommandResult>(
            _broker, Registrations(), typeof(UseCases.RigState.Mining.StartMiningCommand));

        var queueName = $"rig_inventory_queue_{rig.Id}";
        await host.StartAsync(queueName);

        var message = new StartMiningCommandResult(rig.Id, true);


        // Act

        await ConsumerService.PublishAsync(queueName, ConsumerService.Serialize(message)!, _broker);


        // Assert

        MiningStartedEventHandler.Events.Should().BeEmpty();

        _repositoryMock.Verify(x => x.Update(
            It.Is<Rig>(x => x == rig), default),
            Times.Never);

        _grainFactoryMock.Verify(x => x.GetGrain(
            It.Is<RigId>(x => x == rig.Id),
            It.IsAny<CancellationToken>()),
            Times.Once);

        await host.DisposeAsync();
    }

    [Test]
    public async Task ConsumeUnsuccessfully_RigFound_ShouldProcessMessage()
    {
        // Arrange

        var rig = new Rig(new RigId(Guid.NewGuid()), Guid.NewGuid(), "RigName_1");
        rig.InitiateStartMining(rig.OwnerId);

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

        var host = ConsumerService.CreateHost<StartMiningCommandResult>(
            _broker, Registrations(), typeof(TerminateStartMiningCommand));
        var queueName = $"rig_{rig.Id}";

        await host.StartAsync(queueName);
        var message = new StartMiningCommandResult(rig.Id, false);


        // Act

        await ConsumerService.PublishAsync(
            queueName, ConsumerService.Serialize(message)!, _broker);


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

        var host = ConsumerService.CreateHost<StartMiningCommandResult>(
            _broker, Registrations(), typeof(TerminateStartMiningCommand));
        var queueName = $"rig_{rig.Id}";

        await host.StartAsync(queueName);
        var message = new StartMiningCommandResult(rig.Id, false);


        // Act

        await ConsumerService.PublishAsync(queueName, ConsumerService.Serialize(message)!, _broker);


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

        var host = ConsumerService.CreateHost<StartMiningCommandResult>(
            _broker, Registrations(), typeof(TerminateStartMiningCommand));
        var queueName = $"rig_{rigId}";

        await host.StartAsync(queueName);
        var message = new StartMiningCommandResult(rigId, false);


        // Act

        await ConsumerService.PublishAsync(queueName, ConsumerService.Serialize(message)!, _broker);


        // Assert

        _grainFactoryMock.Verify(x => x.GetGrain(rigId, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(x => x.Update(It.IsAny<Rig>(), default), Times.Never);

        await host.DisposeAsync();
    }

    private Action<IServiceCollection> Registrations()
    {
        return services =>
        {
            services.AddEventHandler<MiningStartedEvent, MiningStartedEventHandler>();
            services.AddConsumer<StartMiningCommandResult, AgentMsgConsumer>();
            services.AddScoped(_ => _repositoryMock.Object);
            services.AddScoped(_ => _grainFactoryMock.Object);
        };
    }
}
