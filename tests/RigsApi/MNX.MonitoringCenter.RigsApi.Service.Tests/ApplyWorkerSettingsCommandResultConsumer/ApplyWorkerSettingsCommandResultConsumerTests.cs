using EasyNetQ.AutoSubscribe;
using FluentAssertions;
using MediatR;
using MessagePack;
using Microsoft.Extensions.DependencyInjection;
using MNX.MonitoringCenter.Management.Agent.Commands.Mining.ApplySettings;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice.Commands.ConfirmFlightSheet;
using MNX.MonitoringCenter.RigsApi.Service.Consumers;
using Moq;
using RabbitMQ.Client;

namespace MNX.MonitoringCenter.RigsApi.Service.Tests.ApplyWorkerSettingsCommandResultConsumer;

internal class ApplyWorkerSettingsCommandResultConsumerTests
{
    private RabbitFixture _rabbit;
    private Mock<IMiningDeviceRepository> _repositoryMock;

    [SetUp]
    public async Task SetUp()
    {
        _repositoryMock = new Mock<IMiningDeviceRepository>();
        _rabbit = new RabbitFixture();
        await _rabbit.StartAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        await MiningDeviceStateChangedEventNotificationHandler.ClearEvents();
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

        var host = new ConsumerHost<ApplyWorkerSettingsCommandResult>(
            CreateServiceProvider(),
            _rabbit.Host,
            _rabbit.Port);
        var queueName = $"queue_{successfullyIds[0]}";

        await host.StartAsync(queueName);

        var message = new ApplyWorkerSettingsCommandResult([.. successfullyIds], [.. unsuccessfullyIds]);


        // Act

        await PublishAsync(queueName, Serialize(message)!);
        await Task.Delay(1000);


        // Assert

        _repositoryMock.Verify(x => x.ConfirmFlightSheet(successfullyIds), Times.Once);
        _repositoryMock.Verify(x => x.SetFlightSheetConfirmationStateToError(unsuccessfullyIds), Times.Never);
        _repositoryMock.Verify(x => x.GetById(successfullyIds[0], It.IsAny<CancellationToken>()), Times.Once);

        MiningDeviceStateChangedEventNotificationHandler.Events.Should().HaveCount(1);

        await host.DisposeAsync();
    }

    private ServiceProvider CreateServiceProvider()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddLogging();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(
                typeof(ConfirmFlightSheetCommand).Assembly
            );
        });
        services.AddTransient<IConsumeAsync<ApplyWorkerSettingsCommandResult>, AgentMsgConsumer>();
        services.AddScoped(_ => _repositoryMock.Object);

        services.AddTransient<INotificationHandler<MiningDeviceStateChangedEvent>, MiningDeviceStateChangedEventNotificationHandler>();

        return services.BuildServiceProvider();
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

    private static byte[]? Serialize(object message)
    {
        var options = MessagePackSerializerOptions.Standard
            .WithResolver(MessagePack.Resolvers.ContractlessStandardResolver.Instance);
        return MessagePackSerializer.Serialize(message, options);
    }
}
