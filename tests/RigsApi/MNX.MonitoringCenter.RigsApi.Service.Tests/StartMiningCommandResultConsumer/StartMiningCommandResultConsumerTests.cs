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
using MNX.MonitoringCenter.RigsApi.UseCases.RigState.Mining;
using Moq;
using RabbitMQ.Client;

namespace MNX.MonitoringCenter.RigsApi.Service.Tests.StartMiningCommandResultConsumer;

internal class StartMiningCommandResultConsumerTests
{
    private RabbitFixture _rabbit;
    private Mock<IRigRepository> _repositoryMock;
    private Mock<IRigGrainFactory> _grainFactoryMock;

    [SetUp]
    public async Task SetUp()
    {
        _grainFactoryMock = new Mock<IRigGrainFactory>();
        _repositoryMock = new Mock<IRigRepository>();
        _rabbit = new RabbitFixture();
        await _rabbit.StartAsync();
    }

    [TearDown]

    [Test]
    public async Task Consume_MessageIsSuccess_ShouldProcessMessage()
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

        var host = CreateHost();
        var queueName = $"rig_inventory_queue_{rig.Id}";

        await host.StartAsync(queueName);

        var message = new StartMiningCommandResult(rig.Id, true);


        // Act

        await PublishAsync(queueName, Serialize(message)!);
        await Task.Delay(1000);


        // Assert

        Assert.That(capturedRigGrain, Is.Not.Null);

        _repositoryMock.Verify(x => x.Update(rig, default), Times.Once);
        _grainFactoryMock.Verify(x => x.GetGrain(rig.Id, It.IsAny<CancellationToken>()), Times.Once);

        await host.DisposeAsync();
    }

    [Test]
    public async Task Consume_MessageIsSuccess_ShouldThrowException()
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

        var host = new ConsumerHost<StartMiningCommandResult>(
            CreateServiceProvider(),
            _rabbit.Host,
            _rabbit.Port);

        var queueName = $"rig_inventory_queue_{rig.Id}";
        await host.StartAsync(queueName);

        var message = new StartMiningCommandResult(rig.Id, true);


        // Act

        await PublishAsync(queueName, Serialize(message)!);
        await Task.Delay(TimeSpan.FromSeconds(1));


        // Assert

        Assert.That(capturedRigGrain, Is.Not.Null);

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
    public async Task Consume_MessageIsSuccess_ShouldNotProcess()
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

        var host = new ConsumerHost<StartMiningCommandResult>(
            CreateServiceProvider(),
            _rabbit.Host,
            _rabbit.Port);

        var queueName = $"rig_inventory_queue_{rig.Id}";
        await host.StartAsync(queueName);

        var message = new StartMiningCommandResult(rig.Id, true);


        // Act

        await PublishAsync(queueName, Serialize(message)!);
        await Task.Delay(TimeSpan.FromSeconds(1));


        // Assert

        _repositoryMock.Verify(x => x.Update(
            It.Is<Rig>(x => x == rig), default),
            Times.Never);

        _grainFactoryMock.Verify(x => x.GetGrain(
            It.Is<RigId>(x => x == rig.Id),
            It.IsAny<CancellationToken>()),
            Times.Once);

        await host.DisposeAsync();
    }

    private ServiceProvider CreateServiceProvider()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddLogging();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(
                typeof(UseCases.RigState.Mining.StartMiningCommand).Assembly,
                typeof(TerminateStartMiningCommand).Assembly
            );
        });
        services.AddTransient<IConsumeAsync<StartMiningCommandResult>, AgentMsgConsumer>();
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

    private ConsumerHost<StartMiningCommandResult> CreateHost()
    {
        return new ConsumerHost<StartMiningCommandResult>(
            CreateServiceProvider(),
            _rabbit.Host,
            _rabbit.Port
        );
    }
}
