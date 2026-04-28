using EasyNetQ.AutoSubscribe;
using FluentAssertions;
using MediatR;
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

namespace MNX.MonitoringCenter.RigsApi.Service.Tests.RigInventoryMsgConsumer;

internal class RigInventoryMsgConsumerTests
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
        await RigInventorySavedEventNotificationHandler.ClearEvents();
    }

    [TestCaseSource(typeof(RigInventoryTestCaseSources), nameof(RigInventoryTestCaseSources.ValidRigInventoryMessages))]
    public async Task Consume_RigExists_ShouldProcessMessage(RigInventoryMsg data)
    {
        // Arrange

        var rigId = data.RigId;

        _repositoryMock
            .Setup(x => x.Exists(
                It.Is<Guid>(x => x == rigId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        Guid capturedRigId = default!;
        RigInventoryModel capturedInventory = null!;
        var saved = new List<RigInventoryModel>();
        _repositoryMock
            .Setup(x => x.SaveInventory(
                It.Is<Guid>(x => x == rigId),
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

        var host = CreateHost();
        var queueName = $"rig_inventory_queue_{Guid.NewGuid()}";

        await host.StartAsync(queueName);


        // Act

        await PublishAsync(queueName, Serialize(data)!);
        await Task.Delay(TimeSpan.FromSeconds(1));


        // Assert

        _repositoryMock.Verify(x => x.Exists(
            It.Is<Guid>(id => id == rigId),
            It.IsAny<CancellationToken>()),
            Times.Once);

        _repositoryMock.Verify(x => x.SaveInventory(
            It.Is<Guid>(id => id == rigId),
            It.IsAny<DateTimeOffset>(),
            It.IsAny<RigInventoryModel>(),
            It.IsAny<CancellationToken>()),
            Times.Once);

        using (Assert.EnterMultipleScope())
        {
            Assert.That(capturedRigId, Is.EqualTo(data.RigId));
            Assert.That(capturedInventory.Software.AgentVersion, Is.EqualTo("1.0.1"));
            Assert.That(saved, Has.Count.EqualTo(1));

            RigInventorySavedEventNotificationHandler.Events.Should().HaveCount(1);
        }

        await host.DisposeAsync();
    }

    [TestCaseSource(typeof(RigInventoryTestCaseSources), nameof(RigInventoryTestCaseSources.ValidRigInventoryMessages))]
    public async Task Consume_RigDoesNotExist_ShouldNotProcess(RigInventoryMsg data)
    {
        // Arrange

        var rigId = data.RigId;

        _repositoryMock.Setup(x => x.Exists(
                It.Is<Guid>(x => x == rigId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _repositoryMock.Setup(x => x.SaveInventory(
            It.Is<Guid>(x => x == rigId),
            It.IsAny<DateTimeOffset>(),
            It.IsAny<RigInventoryModel>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var host = CreateHost();
        var queueName = $"rig_inventory_queue_{Guid.NewGuid()}";

        await host.StartAsync(queueName);


        // Act

        await PublishAsync(queueName, Serialize(data)!);
        await Task.Delay(TimeSpan.FromSeconds(1));


        // Assert

        _repositoryMock.Verify(x => x.Exists(
            It.Is<Guid>(x => x == rigId),
            It.IsAny<CancellationToken>()),
            Times.Once);

        _repositoryMock.Verify(
            x => x.SaveInventory(
                It.IsAny<Guid>(),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<RigInventoryModel>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        RigInventorySavedEventNotificationHandler.Events.Should().HaveCount(0);

        await host.DisposeAsync();
    }

    [TestCaseSource(typeof(RigInventoryTestCaseSources), nameof(RigInventoryTestCaseSources.InvalidRigInventoryMessages))]
    public async Task Consume_RigIsInvalid_ShouldNotProcess(RigInventoryMsg data)
    {
        // Arrange

        var rigId = data.RigId;

        _repositoryMock.Setup(x => x.Exists(
                It.Is<Guid>(x => x == rigId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _repositoryMock.Setup(x => x.SaveInventory(
            It.Is<Guid>(x => x == rigId),
            It.IsAny<DateTimeOffset>(),
            It.IsAny<RigInventoryModel>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var host = CreateHost();
        var queueName = $"rig_inventory_queue_{Guid.NewGuid()}";

        await host.StartAsync(queueName);


        // Act

        await PublishAsync(queueName, Serialize(data)!);
        await Task.Delay(TimeSpan.FromSeconds(1));


        // Assert

        _repositoryMock.Verify(x => x.Exists(
            It.Is<Guid>(x => x == rigId),
            It.IsAny<CancellationToken>()),
            Times.Once);

        _repositoryMock.Verify(
            x => x.SaveInventory(
                It.IsAny<Guid>(),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<RigInventoryModel>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        RigInventorySavedEventNotificationHandler.Events.Should().HaveCount(0);

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

        services.AddTransient<INotificationHandler<RigInventorySavedEvent>, RigInventorySavedEventNotificationHandler>();

        return services.BuildServiceProvider();
    }

    private ConsumerHost<RigInventoryMsg> CreateHost()
    {
        return new ConsumerHost<RigInventoryMsg>(
            CreateServiceProvider(),
            _rabbit.Host,
            _rabbit.Port);
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
}
