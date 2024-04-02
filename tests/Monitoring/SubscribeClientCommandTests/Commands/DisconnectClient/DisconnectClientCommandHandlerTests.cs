using MNX.MonitoringCenter.Monitoring.UseCases.Commands.Clients;
using Moq;
using MNX.MonitoringCenter.Monitoring.UseCases.Commands.Clients.DisconnectUsers;
using EasyNetQ;
using MNX.MonitoringCenter.Monitoring.UseCases.Commands.DisconnectClient;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Tests.Commands.DisconnectClient;

[TestFixture]
public class DisconnectClientCommandHandlerTests
{
    [Test]
    public void DisconnectClient_ReturnsEmpty()
    {
        // Arrange
        var pubSub = new Mock<IPubSub>();

        // Act
        var handler = new DisconnectClientCommandHandler(pubSub.Object);

        var result = handler.Handle(GetCommand(), default);

        Assert.Multiple(() =>
        {

            // Assert
            Assert.That(ConnectionCounter.AddConnectionInGroup(25), Is.EqualTo(1));
            Assert.That(ConnectionCounter.RemoveConnectionInGroup(25), Is.EqualTo(0));
            Assert.That(ConnectionCounter.RemoveConnectionInGroup(25), Is.EqualTo(0));
        });
    }

    private static DisconnectClientCommand GetCommand()
    {
        return new DisconnectClientCommand(new ConnectionModel
        {
            UserId = It.IsAny<long>(),
            ConnectionId = It.IsAny<string>()
        });
    }
}
