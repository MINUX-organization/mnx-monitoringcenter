using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MNX.MonitoringCenter.Inventory.IntegrationTests;
using MNX.MonitoringCenter.Management.UseCases.SetRigDevices;
using NUnit.Framework;

namespace MNX.MonitoringCenter.Inventory.UseCases.IntegrationTests;

public class SetRigDevicesTests : BaseTest
{
    private IMediator _mediator;

    private readonly static Guid OWNER_ID = Guid.Parse("59929758-14d1-43b5-a80c-7e84bfc45145");

    [SetUp]
    public void SetUp()
    {
        _mediator = ServiceProvider.GetRequiredService<IMediator>();
    }

    [TestCaseSource(typeof(SetRigDevicesTests), nameof(SaveCommandTestCase.InventoryMessages))]
    public async Task SetRigDevices(SetRigDevicesCommand message)
    {

    }
}
