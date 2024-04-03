using AutoMapper;
using EasyNetQ;
using MNX.MonitoringCenter.Monitoring.Contracts.Models;
using MNX.MonitoringCenter.Monitoring.Core;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;
using MNX.MonitoringCenter.Monitoring.UseCases.Commands.Clients;
using MNX.MonitoringCenter.Monitoring.UseCases.Commands.Clients.SubscribeUsers;
using MNX.MonitoringCenter.Monitoring.UseCases.Commands.SubscribeClient;
using Moq;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Tests.Commands.SubscribeClient;

[TestFixture]
public class SubscribeClientCommandHandlerTests
{
    [Test]
    public async Task SubscribeClient_ReturnsEmpty()
    {
        // Arrange
        var expectedRigs = new List<Rig>
        {
            new Rig()
            {
                Id = Guid.Parse("00c731c6-131d-4529-b728-e8963ba74af8"),
                UserId = 12
            },
            new Rig()
            {
                Id = Guid.Parse("ecf91bf9-f09d-4db6-ac56-10171edfb00a"),
                UserId = 24
            }
        };

        var rigRepository = new Mock<IRigRepository>();
        rigRepository.Setup(x => x.GetAvailable(123)).Returns(expectedRigs.ToAsyncEnumerable());

        var pubSub = new Mock<IPubSub>();

        var config = new MapperConfiguration(cfg =>
                    cfg.CreateMap<Rig, RigInformation>());

        var mapper = new Mapper(config);

        var rigInfo = mapper.Map<List<Rig>, List<RigInformation>>(expectedRigs);

        int index = 1;
        foreach (var rig in rigInfo)
        {
            rig.Index = index++;
        }

        // Act
        var handler = new SubscribeClientCommandHandler(pubSub.Object,
                                                rigRepository.Object,
                                                mapper);

        var result = await handler.Handle(GetCommand(), default);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(ConnectionCounter.AddConnectionInGroup(123), Is.EqualTo(1));
            Assert.That(ConnectionCounter.AddConnectionInGroup(123), Is.EqualTo(2));
            Assert.That(rigInfo[0].Id, Is.EqualTo(expectedRigs[0].Id));
            Assert.That(rigInfo[1].Id, Is.EqualTo(expectedRigs[1].Id));
            Assert.That(rigInfo[0].Index, Is.EqualTo(1));
            Assert.That(rigInfo[1].Index, Is.EqualTo(2));
        });
    }

    private static SubscribeClientCommand GetCommand()
    {
        return new SubscribeClientCommand(new ConnectionModel
        {
            UserId = It.IsAny<long>(),
            ConnectionId = It.IsAny<string>()
        });
    }
}
