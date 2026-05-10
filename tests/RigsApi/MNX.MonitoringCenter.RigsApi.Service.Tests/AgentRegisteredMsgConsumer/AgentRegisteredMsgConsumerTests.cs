using Microsoft.Extensions.DependencyInjection;
using MNX.MonitoringCenter.RigsApi.Core;
using MNX.MonitoringCenter.RigsApi.Core.Services;
using MNX.MonitoringCenter.RigsApi.Service.Consumers;
using MNX.MonitoringCenter.RigsApi.UseCases;
using MNX.SecurityManagement.Authentication.Contracts;
using Moq;

namespace MNX.MonitoringCenter.RigsApi.Service.Tests.AgentRegisteredMsgConsumer;

internal class AgentRegisteredMsgConsumerTests
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

    [Test]
    public async Task Consume_ValidRig_ShouldProcessMessage()
    {
        // Arrange

        var rigId = Guid.NewGuid();
        _repositoryMock.Setup(x => x.Add(It.IsAny<Rig>(), It.IsAny<CancellationToken>()));

        var host = TestsEnvironmentService.CreateHost<AgentRegisteredMsg>(
            _broker, DefaultRegistrations(), typeof(AddRigCommand));
        var queueName = $"rig_{rigId}";

        await host.StartAsync(queueName);

        var message = new AgentRegisteredMsg()
        {
            Id = rigId,
            OwnerId = Guid.NewGuid(),
            Nickname = "UserNickname_1"
        };


        // Act

        await TestsEnvironmentService.Publish(queueName, message, _broker);


        // Assert

        _repositoryMock.Verify(x => x.Add(It.IsAny<Rig>(), It.IsAny<CancellationToken>()));

        await host.DisposeAsync();
    }

    private Action<IServiceCollection> DefaultRegistrations()
    {
        return services =>
        {
            services.AddConsumer<AgentRegisteredMsg, AgentLifeCycleMsgConsumer>();
            services.AddScoped(_ => _repositoryMock.Object);
        };
    }
}
