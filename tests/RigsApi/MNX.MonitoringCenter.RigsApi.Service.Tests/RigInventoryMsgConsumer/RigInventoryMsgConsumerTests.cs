using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MNX.Application.UseCases.DI;
using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;
using MNX.MonitoringCenter.Inventory.UseCases;
using MNX.MonitoringCenter.Inventory.UseCases.RigInventory;
using MNX.MonitoringCenter.RigsApi.Service.Consumers;
using Moq;

namespace MNX.MonitoringCenter.RigsApi.Service.Tests.RigInventoryMsgConsumer;

internal class RigInventoryMsgConsumerTests
{
    private IMessageBrokerFixture _broker;
    private Mock<IRigRepository> _repositoryMock;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _broker = GlobalMessageBrokerFixture.Broker;
    }

    [SetUp]
    public Task SetUp()
    {
        _repositoryMock = new Mock<IRigRepository>();
        return Task.CompletedTask;
    }

    [TearDown]
    public Task TearDown()
    {
        return RigInventorySavedEventHandler.ClearEvents();
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

        var host = TestsEnvironmentService.CreateHost<RigInventoryMsg>(
            _broker, DefaultRegistrations(), typeof(SaveRigInventoryCommand), typeof(SaveRigInventoryCommandHandler));
        var queueName = $"rig_inventory_queue_{Guid.NewGuid()}";

        await host.StartAsync(queueName);


        // Act

        await TestsEnvironmentService.Publish(queueName, data, _broker);


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

            RigInventorySavedEventHandler.Events.Should().HaveCount(1);
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

        var host = TestsEnvironmentService.CreateHost<RigInventoryMsg>(
            _broker, DefaultRegistrations(), typeof(SaveRigInventoryCommand), typeof(SaveRigInventoryCommandHandler));
        var queueName = $"rig_inventory_queue_{Guid.NewGuid()}";

        await host.StartAsync(queueName);


        // Act

        await TestsEnvironmentService.Publish(queueName, data, _broker);


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

        RigInventorySavedEventHandler.Events.Should().HaveCount(0);

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

        var host = TestsEnvironmentService.CreateHost<RigInventoryMsg>(
            _broker, DefaultRegistrations(), typeof(SaveRigInventoryCommand), typeof(SaveRigInventoryCommandHandler));
        var queueName = $"rig_inventory_queue_{Guid.NewGuid()}";

        await host.StartAsync(queueName);


        // Act

        await TestsEnvironmentService.Publish(queueName, data, _broker);


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

        RigInventorySavedEventHandler.Events.Should().HaveCount(0);

        await host.DisposeAsync();
    }

    private Action<IServiceCollection> DefaultRegistrations()
    {
        return services =>
        {
            services.AddConsumer<RigInventoryMsg, AgentMsgConsumer>();
            services.AddValidationPipelines(typeof(SaveRigInventoryCommand).Assembly,
                typeof(SaveRigInventoryCommandValidator).Assembly);
            services.AddScoped(_ => _repositoryMock.Object);
            services.AddTransient<INotificationHandler<RigInventorySavedEvent>, RigInventorySavedEventHandler>();
        };
    }
}
