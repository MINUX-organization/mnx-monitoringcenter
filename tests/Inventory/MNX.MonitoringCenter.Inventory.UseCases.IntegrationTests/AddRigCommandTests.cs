using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MNX.MonitoringCenter.Inventory.Contracts.Requests;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;
using MNX.MonitoringCenter.Inventory.IntegrationTests;
using MNX.SecurityManagement.Authentication.Contracts;
using NUnit.Framework;

namespace MNX.MonitoringCenter.Inventory.UseCases.IntegrationTests;

public class AddRigCommandTests : BaseTest
{
    private IMediator _mediator;

    [SetUp]
    public void SetUp()
    {
        _mediator = ServiceProvider.GetRequiredService<IMediator>();
    }

    //[TestCaseSource(typeof(AddRigCommandTestCase), nameof(AddRigCommandTestCase.Rigs))]
    public async Task AddRigCommandTest(AgentRegisteredMsg message)
    {
        var result = await _mediator.Send(new AddRigCommand(message.Id, message.OwnerId, message.Nickname));

        Assert.That(result.IsSuccess);

        var rigRepository = ServiceProvider.GetRequiredService<IRigRepository>();
        var rigs = rigRepository.GetRigs(new InventorySpecification(message.OwnerId, message.Id))
                                .ToBlockingEnumerable()
                                .ToList();

        Assert.That(rigs.Any(x => x.Id == message.Id && x.OwnerId == message.OwnerId));
    }

    /*[TestCaseSource(typeof(AddRigCommandTestCase), nameof(AddRigCommandTestCase.Rigs))]
    public async Task AddRegisteredCommandWithRabbitMQ(RigRegisteredMsg message)
    {
        using var bus = RabbitHutch.CreateBus("host=77.37.200.24:5672;username=guest;password=guest;publisherConfirms=true");
        await bus.PubSub.PublishAsync(message);
    }*/

    private class AddRigCommandTestCase
    {
        private readonly static Guid OWNER_ID = Guid.Parse("0b8e36f9-bf02-4c88-97f8-cb5a81715000");

        public static IEnumerable<AgentRegisteredMsg> Rigs
        {
            get
            {
                yield return new AgentRegisteredMsg()
                {
                    Id = Guid.NewGuid(),
                    OwnerId = OWNER_ID,
                    Nickname = "Minux_1"
                };
                yield return new AgentRegisteredMsg()
                {
                    Id = Guid.NewGuid(),
                    OwnerId = OWNER_ID,
                    Nickname = "Minux_2"
                };
                yield return new AgentRegisteredMsg()
                {
                    Id = Guid.NewGuid(),
                    OwnerId = OWNER_ID,
                    Nickname = "Minux_3"
                };
            }
        }
    }
}
