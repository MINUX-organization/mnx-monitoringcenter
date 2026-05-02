using EasyNetQ.AutoSubscribe;
using MessagePack;
using Microsoft.Extensions.DependencyInjection;
using MNX.Application.UseCases.DI;
using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;
using MNX.MonitoringCenter.Inventory.UseCases;
using MNX.MonitoringCenter.Inventory.UseCases.RigInventory;
using MNX.MonitoringCenter.RigsApi.Service.Consumers;
using Moq;
using RabbitMQ.Client;

namespace MNX.MonitoringCenter.RigsApi.Service.Tests;

public class RigInventoryMsgConsumerTests
{
    private RabbitFixture _rabbit;
    private Mock<IRigRepository> _repositoryMock;

    [SetUp]
    public async Task SetUp()
    {
        _repositoryMock = new Mock<IRigRepository>();
        _rabbit = new RabbitFixture();
        await _rabbit.StartAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        await _rabbit.StopAsync();
    }

    [TestCaseSource(typeof(RigInventoryTestCaseSources), nameof(RigInventoryTestCaseSources.ValidRigInventoryMessages))]
    public async Task Should_Process_Message_EndToEnd(RigInventoryMsg data)
    {
        // Arrange

        _repositoryMock
            .Setup(x => x.Exists(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        Guid capturedRigId = default!;
        RigInventoryModel capturedInventory = null!;
        var saved = new List<RigInventoryModel>();
        _repositoryMock
            .Setup(x => x.SaveInventory(
                It.IsAny<Guid>(),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<RigInventoryModel>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(1)
            .Callback<Guid, DateTimeOffset, RigInventoryModel, CancellationToken>((id, _, inventory, _) =>
            {
                capturedRigId = id;
                capturedInventory = inventory;
                saved.Add(inventory);
            });

        var host = new ConsumerHost<RigInventoryMsg>(
            CreateServiceProvider(),
            _rabbit.Host,
            _rabbit.Port
        );

        var queueName = $"rig_inventory_queue_{Guid.NewGuid()}";

        await host.StartAsync(queueName);
        await host.WaitUntilStartedAsync();

        var factory = new ConnectionFactory
        {
            HostName = _rabbit.Host,
            Port = _rabbit.Port,
            UserName = "guest",
            Password = "guest",
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        var body = Serialize(data);


        // Act

        channel.BasicPublish(
            exchange: "",
            routingKey: queueName,
            basicProperties: null,
            body: body);

        await Task.Delay(TimeSpan.FromSeconds(1));

        // Assert

        var result = channel.MessageCount(queueName);

        _repositoryMock.Verify(x => x.SaveInventory(
            It.Is<Guid>(id => id == data.RigId),
            It.IsAny<DateTimeOffset>(),
            It.IsAny<RigInventoryModel>(),
            It.IsAny<CancellationToken>()),
            Times.Once);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result, Is.Zero);
            Assert.That(capturedRigId, Is.EqualTo(data.RigId));
            Assert.That(capturedInventory.Software.AgentVersion, Is.EqualTo("1.0.1"));
            Assert.That(saved, Has.Count.EqualTo(1));
        }

        
        // Cleanup
        await host.StopAsync();
        await host.DisposeAsync();
    }

    [TestCaseSource(typeof(RigInventoryTestCaseSources), nameof(RigInventoryTestCaseSources.InvalidRigInventoryMessages))]
    public async Task Should_Not_Process_When_Rig_Does_Not_Exist(RigInventoryMsg data)
    {
        // Arrange

        _repositoryMock.Setup(x => x.Exists(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _repositoryMock.Setup(x => x.SaveInventory(
            It.IsAny<Guid>(),
            It.IsAny<DateTimeOffset>(),
            It.IsAny<RigInventoryModel>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var host = new ConsumerHost<RigInventoryMsg>(
            CreateServiceProvider(),
            _rabbit.Host,
            _rabbit.Port);

        var queueName = $"rig_inventory_queue_{Guid.NewGuid()}";

        await host.StartAsync(queueName);
        await host.WaitUntilStartedAsync();

        var factory = new ConnectionFactory()
        {
            HostName = _rabbit.Host,
            Port = _rabbit.Port,
            UserName = "guest",
            Password = "guest",
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        var body = Serialize(data);


        // Act

        channel.BasicPublish(
            exchange: "",
            routingKey: queueName,
            basicProperties: null,
            body: body);

        await Task.Delay(TimeSpan.FromSeconds(1));


        // Assert

        _repositoryMock.Verify(x => x.Exists(
            It.IsAny<Guid>(),
            It.IsAny<CancellationToken>()),
            Times.Once);

        _repositoryMock.Verify(
            x => x.SaveInventory(
                It.IsAny<Guid>(),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<RigInventoryModel>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        await host.StopAsync();
        await host.DisposeAsync();
    }

    private static byte[]? Serialize(object message)
    {
        var options = MessagePackSerializerOptions.Standard
            .WithResolver(MessagePack.Resolvers.ContractlessStandardResolver.Instance);
        return MessagePackSerializer.Serialize(message, options);
    }

    private ServiceProvider CreateServiceProvider()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddLogging();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(
                typeof(SaveRigInventoryCommandHandler).Assembly,
                typeof(SaveRigInventoryCommand).Assembly);
        });
        services.AddValidationPipelines(typeof(SaveRigInventoryCommand).Assembly,
            typeof(SaveRigInventoryCommandValidator).Assembly);

        services.AddTransient<IConsumeAsync<RigInventoryMsg>, AgentMsgConsumer>();
        services.AddScoped(_ => _repositoryMock.Object);

        return services.BuildServiceProvider();
    }
}
