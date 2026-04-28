using EasyNetQ.AutoSubscribe;
using MessagePack;
using Microsoft.Extensions.DependencyInjection;
using MNX.MonitoringCenter.Management.Agent.Commands.Mining;
using MNX.MonitoringCenter.RigsApi.Core;
using MNX.MonitoringCenter.RigsApi.Core.Services;
using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;
using MNX.MonitoringCenter.RigsApi.GrainWrapper;
using MNX.MonitoringCenter.RigsApi.GrainWrapper.Services;
using MNX.MonitoringCenter.RigsApi.Service.Consumers;
using Moq;
using RabbitMQ.Client;

namespace MNX.MonitoringCenter.RigsApi.Service.Tests.StopMiningCommandResultConsumer;

internal class StopMiningCommandResultConsumerTests
{
    private RabbitFixture _rabbit;
    private Mock<IRigGrainFactory> _grainFactoryMock;
    private Mock<IRigRepository> _repositoryMock;

    [SetUp]
    public async Task SetUp()
    {
        _rabbit = new RabbitFixture();
        _grainFactoryMock = new Mock<IRigGrainFactory>();
        _repositoryMock = new Mock<IRigRepository>();
        await _rabbit.StartAsync();
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

        var host = CreateHost();
        var queueName = $"queue_{rig.Id}";

        await host.StartAsync(queueName);

        var message = new StopMiningCommandResult(rig.Id, true);


        // Act

        await PublishAsync(queueName, Serialize(message)!);
        await Task.Delay(1000);


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

        var host = CreateHost();
        var queueName = $"queue_{rig.Id}";

        await host.StartAsync(queueName);

        var message = new StopMiningCommandResult(rig.Id, true);


        // Act

        await PublishAsync(queueName, Serialize(message)!);
        await Task.Delay(1000);


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

        var host = CreateHost();
        var queueName = $"queue_{Guid.NewGuid()}";

        await host.StartAsync(queueName);

        var message = new StopMiningCommandResult(rigId, true);


        // Act

        await PublishAsync(queueName, Serialize(message)!);
        await Task.Delay(1000);


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

    private ServiceProvider CreateServiceProvider()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddLogging();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(
                typeof(UseCases.RigState.Mining.StopMiningCommand).Assembly
            );
        });
        services.AddTransient<IConsumeAsync<StopMiningCommandResult>, AgentMsgConsumer>();
        services.AddScoped(_ => _repositoryMock.Object);
        services.AddScoped(_ => _grainFactoryMock.Object);

        return services.BuildServiceProvider();
    }

    private static byte[]? Serialize(object message)
    {
        var options = MessagePackSerializerOptions.Standard
            .WithResolver(MessagePack.Resolvers.ContractlessStandardResolver.Instance);
        return MessagePackSerializer.Serialize(message, options);
    }

    private async Task PublishAsync(string queueName, byte[] body)
    {
        var factory = new ConnectionFactory
        {
            HostName = _rabbit.Host,
            Port = _rabbit.Port,
            UserName = "guest",
            Password = "guest",
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.BasicPublish(
            exchange: "",
            routingKey: queueName,
            basicProperties: null,
            body: body);
    }

    private ConsumerHost<StopMiningCommandResult> CreateHost()
    {
        return new ConsumerHost<StopMiningCommandResult>(
            CreateServiceProvider(),
            _rabbit.Host,
            _rabbit.Port
        );
    }
}
