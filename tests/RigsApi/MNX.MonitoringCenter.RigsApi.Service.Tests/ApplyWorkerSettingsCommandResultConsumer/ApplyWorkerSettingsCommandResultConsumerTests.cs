using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MNX.MonitoringCenter.Management.Agent.Commands.Mining.ApplySettings;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice.Commands.ConfirmFlightSheet;
using MNX.MonitoringCenter.RigsApi.Service.Consumers;
using Moq;

namespace MNX.MonitoringCenter.RigsApi.Service.Tests.ApplyWorkerSettingsCommandResultConsumer;

internal class ApplyWorkerSettingsCommandResultConsumerTests
{
    private IMessageBrokerFixture _broker;
    private Mock<IMiningDeviceRepository> _repositoryMock;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _broker = GlobalMessageBrokerFixture.Broker;
    }

    [SetUp]
    public Task SetUp()
    {
        _repositoryMock = new Mock<IMiningDeviceRepository>();
        return Task.CompletedTask;
    }

    [TearDown]
    public Task TearDown()
    {
        return MiningDeviceStateChangedEventHandler.ClearEvents();
    }

    [Test]
    public async Task Consume_ShouldProcessMessage()
    {
        // Arrange

        var successfullyIds = new Guid[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
        var unsuccessfullyIds = Array.Empty<Guid>();

        _repositoryMock.Setup(x => x.ConfirmFlightSheet(successfullyIds));
        _repositoryMock.Setup(x => x.SetFlightSheetConfirmationStateToError(unsuccessfullyIds));

        MiningDeviceInfo? capturedDevice = null;
        var miningDeviceInfo = new MiningDeviceInfo()
        {
            Manufacturer = "Manufacturer",
            Model = "Model",
            OwnerId = Guid.NewGuid(),
        };
        _repositoryMock.Setup(x => x.GetById(successfullyIds[0],
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(miningDeviceInfo)
            .Callback<Guid, CancellationToken>((id, token) =>
            {
                capturedDevice = miningDeviceInfo;
            });

        var host = TestsEnvironmentService.CreateHost<ApplyWorkerSettingsCommandResult>(
            _broker, DefaultRegistrations(), typeof(ConfirmFlightSheetCommand));
        var queueName = $"queue_{successfullyIds[0]}";

        await host.StartAsync(queueName);

        var message = new ApplyWorkerSettingsCommandResult([.. successfullyIds], [.. unsuccessfullyIds]);


        // Act

        await TestsEnvironmentService.Publish(queueName, TestsEnvironmentService.Serialize(message)!, _broker);


        // Assert

        _repositoryMock.Verify(x => x.ConfirmFlightSheet(successfullyIds), Times.Once);
        _repositoryMock.Verify(x => x.SetFlightSheetConfirmationStateToError(unsuccessfullyIds), Times.Never);
        _repositoryMock.Verify(x => x.GetById(successfullyIds[0], It.IsAny<CancellationToken>()), Times.Once);

        MiningDeviceStateChangedEventHandler.Events.Should().HaveCount(1);

        await host.DisposeAsync();
    }

    private Action<IServiceCollection> DefaultRegistrations()
    {
        return services =>
        {
            services.AddConsumer<ApplyWorkerSettingsCommandResult, AgentMsgConsumer>();
            services.AddScoped(_ => _repositoryMock.Object);
            services.AddTransient<INotificationHandler<MiningDeviceStateChangedEvent>, MiningDeviceStateChangedEventHandler>();
        };
    }
}
