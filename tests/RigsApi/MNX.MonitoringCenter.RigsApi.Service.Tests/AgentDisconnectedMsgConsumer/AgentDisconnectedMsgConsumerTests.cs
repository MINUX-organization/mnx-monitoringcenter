using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MNX.MonitoringCenter.RigsApi.Core;
using MNX.MonitoringCenter.RigsApi.Core.DomainEvents.Power;
using MNX.MonitoringCenter.RigsApi.Core.Services;
using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;
using MNX.MonitoringCenter.RigsApi.GrainWrapper;
using MNX.MonitoringCenter.RigsApi.GrainWrapper.Services;
using MNX.MonitoringCenter.RigsApi.Service.Consumers;
using MNX.MonitoringCenter.RigsApi.UseCases.RigState.Power;
using MNX.RigCommander.Contracts;
using Moq;

namespace MNX.MonitoringCenter.RigsApi.Service.Tests.AgentDisconnectedMsgConsumer;

internal class AgentDisconnectedMsgConsumerTests
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
        return RigPoweredOffEventHandler.ClearEvents();
    }

    [Test]
    public async Task Consume_RigFound_ShouldProcessMessage()
    {
        // Arrange

        var agentId = Guid.NewGuid();
        var rig = new Rig(new RigId(agentId), Guid.NewGuid(), "RigName_1");
        rig.InitiateTurnOn(rig.OwnerId);
        rig.TurnOn();

        IRigGrain? capturedGrain = null;

        var grain = new RigGrain(rig, _repositoryMock.Object);
        _grainFactoryMock
            .Setup(x => x.GetGrain(rig.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(grain)
            .Callback<RigId, CancellationToken>((id, token) =>
            {
                capturedGrain = grain;
            });

        _repositoryMock.Setup(x => x.Update(rig, default));

        var host = ConsumerService.CreateHost<AgentDisconnectedMsg>(_broker, Registrations(), typeof(PowerOffCommand));
        var queueName = $"rig_{rig.Id}";

        await host.StartAsync(queueName);
        var message = new AgentDisconnectedMsg(agentId);


        // Act

        await ConsumerService.PublishAsync(queueName, ConsumerService.Serialize(message)!, _broker);


        // Assert

        Assert.That(capturedGrain, Is.Not.Null);
        RigPoweredOffEventHandler.Events.Should().HaveCount(1);

        _grainFactoryMock.Verify(x => x.GetGrain(rig.Id, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(x => x.Update(rig, default), Times.Once);

        await host.DisposeAsync();
    }

    [Test]
    public async Task Consume_RigNotFound_ShouldNotProcessMessage()
    {
        // Arrange

        var agentId = Guid.NewGuid();
        var rigId = new RigId(agentId);

        _grainFactoryMock
            .Setup(x => x.GetGrain(rigId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((IRigGrain?)null);

        _repositoryMock.Setup(x => x.Update(It.IsAny<Rig>(), default));

        var host = ConsumerService.CreateHost<AgentDisconnectedMsg>(_broker, Registrations(), typeof(PowerOffCommand));
        var queueName = $"rig_{rigId}";

        await host.StartAsync(queueName);
        var message = new AgentDisconnectedMsg(agentId);


        // Act

        await ConsumerService.PublishAsync(queueName, ConsumerService.Serialize(message)!, _broker);


        // Assert

        RigPoweredOffEventHandler.Events.Should().BeEmpty();

        _grainFactoryMock.Verify(x => x.GetGrain(rigId, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(x => x.Update(It.IsAny<Rig>(), default), Times.Never);

        await host.DisposeAsync();
    }

    [Test]
    public async Task Consume_RigFound_ShouldThrowException()
    {
        // Arrange

        var agentId = Guid.NewGuid();
        var rig = new Rig(new RigId(agentId), Guid.NewGuid(), "RigName_1");
        rig.InitiateTurnOn(rig.OwnerId);
        rig.TurnOn();

        IRigGrain? capturedGrain = null;

        var grain = new RigGrain(rig, _repositoryMock.Object);
        _grainFactoryMock
            .Setup(x => x.GetGrain(rig.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(grain)
            .Callback<RigId, CancellationToken>((id, token) =>
            {
                capturedGrain = grain;
            });

        _repositoryMock
            .Setup(x => x.Update(rig, default))
            .ThrowsAsync(new Exception("Test exception"));

        var host = ConsumerService.CreateHost<AgentDisconnectedMsg>(_broker, Registrations(), typeof(PowerOffCommand));
        var queueName = $"rig_{rig.Id}";

        await host.StartAsync(queueName);
        var message = new AgentDisconnectedMsg(agentId);


        // Act

        await ConsumerService.PublishAsync(queueName, ConsumerService.Serialize(message)!, _broker);


        // Assert

        Assert.That(capturedGrain, Is.Not.Null);
        RigPoweredOffEventHandler.Events.Should().BeEmpty();

        _grainFactoryMock.Verify(x => x.GetGrain(rig.Id, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(x => x.Update(rig, default), Times.Once);

        await host.DisposeAsync();
    }

    private Action<IServiceCollection> Registrations()
    {
        return services =>
        {
            services.AddEventHandler<RigPoweredOffEvent, RigPoweredOffEventHandler>();
            services.AddConsumer<AgentDisconnectedMsg, AgentLifeCycleMsgConsumer>();
            services.AddScoped(_ => _repositoryMock.Object);
            services.AddScoped(_ => _grainFactoryMock.Object);
        };
    }
}
