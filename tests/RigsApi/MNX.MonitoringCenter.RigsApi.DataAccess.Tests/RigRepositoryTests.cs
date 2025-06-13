using Microsoft.Extensions.DependencyInjection;
using MNX.MonitoringCenter.RigsApi.Core;
using MNX.MonitoringCenter.RigsApi.Core.Services;
using MNX.MonitoringCenter.RigsApi.Core.ValueObjects;
using NUnit.Framework;

namespace MNX.MonitoringCenter.RigsApi.DataAccess.Tests;

public class RigRepositoryTests : BaseTest
{
    [Test]
    public async Task GetRigById_WithExistsRig_ReturnsRig()
    {
        var repository = ServiceProvider.GetRequiredService<IRigRepository>();

        var rigId = new RigId(Guid.NewGuid());
        var rig = new Rig(rigId, Guid.NewGuid(), "Rig_1");

        await repository.Add(rig);

        //

        var rigFromDb = await repository.GetById(rigId);

        //

        Assert.That(rigFromDb, Is.Not.Null);
        Assert.That(rigFromDb.Id, Is.EqualTo(rigId));
    }

    [Test]
    public async Task GetAvailable_WithExistsRigsOwner_ReturnsRigs()
    {
        var repository = ServiceProvider.GetRequiredService<IRigRepository>();

        var rigId = new RigId(Guid.NewGuid());
        var ownerId = Guid.NewGuid();
        var rig = new Rig(rigId, ownerId, "Rig_1");

        await repository.Add(rig);

        //

        var rigsFromDb = new List<Rig>();

        await foreach (var rigFromDb in repository.GetAvailable(ownerId))
        {
            rigsFromDb.Add(rigFromDb);
        };

        //

        Assert.That(rigsFromDb, Has.Count.EqualTo(1));
    }

    [Test]
    public async Task GetAvailable_WithNotExistsRigsOwner_ReturnsRigs()
    {
        var repository = ServiceProvider.GetRequiredService<IRigRepository>();

        var rigId = new RigId(Guid.NewGuid());
        var ownerId = Guid.NewGuid();
        var rig = new Rig(rigId, ownerId, "Rig_1");

        await repository.Add(rig);

        //

        var rigsFromDb = new List<Rig>();

        await foreach (var rigFromDb in repository.GetAvailable(Guid.NewGuid()))
        {
            rigsFromDb.Add(rigFromDb);
        };

        //

        Assert.That(rigsFromDb, Has.Count.EqualTo(0));
    }
}