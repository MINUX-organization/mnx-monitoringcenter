using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MNX.MonitoringCenter.Inventory.Contracts.Requests;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rig;
using MNX.MonitoringCenter.Inventory.IntegrationTests;
using MNX.MonitoringCenter.Inventory.UseCases.Rigs;
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

    [TestCaseSource(typeof(AddRigCommandTestCase), nameof(AddRigCommandTestCase.Rigs))]
    public async Task AddRigTest(RigModel rig)
    {
        var result = await _mediator.Send(new AddRigCommand(rig.RigId, rig.OwnerId));

        Assert.That(result.IsSuccess);

        var rigRepository = ServiceProvider.GetRequiredService<IRigRepository>();
        var rigs = rigRepository.GetRigs(new InventorySpecification(rig.OwnerId, rig.RigId))
                                .ToBlockingEnumerable()
                                .ToList();

        Assert.That(rigs.Any(x => x.Id == rig.RigId && x.OwnerId == rig.OwnerId));
    }

    public class RigModel
    {
        public Guid RigId { get; set; }

        public Guid OwnerId { get; set; }

        public RigModel(Guid rigId, Guid ownerId)
        {
            RigId = rigId;
            OwnerId = ownerId;
        }
    }

    private class AddRigCommandTestCase
    {
        private readonly static Guid OWNER_ID = Guid.Parse("1dcc87f7-a326-4765-befa-680ed6cb7649");

        public static IEnumerable<RigModel> Rigs
        {
            get
            {
                yield return new RigModel(Guid.NewGuid(), OWNER_ID);
                yield return new RigModel(Guid.NewGuid(), OWNER_ID);
                yield return new RigModel(Guid.NewGuid(), OWNER_ID);
            }
        }
    }
}
