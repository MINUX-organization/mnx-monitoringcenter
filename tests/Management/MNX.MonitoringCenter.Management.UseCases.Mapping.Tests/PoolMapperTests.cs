using MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Pool;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool.Commands.AddPool;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool.Commands.EditPool;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests;
[TestFixture]
public sealed class PoolMapperTests
{
    private IPoolMapper _poolMapper;

    [SetUp]
    public void SetUp()
    {
        _poolMapper = new PoolMapper();
    }

    [Test]
    public void MapToCoreEntity_ValidAddPoolCommand_ReturnPool()
    {
        // Arrange

        var addPoolCommand = new AddPoolCommand(
            new PoolInputModelBuilder()
                .WithTls()
                .Build(),
            Guid.NewGuid()
        );

        // Act

        var mappedPool = _poolMapper.MapToCoreEntity(addPoolCommand);

        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(mappedPool, Is.Not.Null);
            Assert.That(mappedPool.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(mappedPool.Tls, Is.EqualTo(addPoolCommand.Model.Tls));
            Assert.That(mappedPool.IsDomain(), Is.EqualTo(false));
            Assert.That(mappedPool.CryptocurrencyId, Is.EqualTo(addPoolCommand.Model.CryptocurrencyId));
            Assert.That(mappedPool.Cryptocurrency, Is.Null);
            Assert.That(mappedPool.OwnerId, Is.EqualTo(addPoolCommand.UserId));
            Assert.That(mappedPool.Domain, Is.EqualTo(addPoolCommand.Model.Domain));
            Assert.That(mappedPool.Port, Is.EqualTo(addPoolCommand.Model.Port));
        });
    }

    [Test]
    public void MapToCoreEntity_ValidEditPoolCommand_ReturnPool()
    {
        // Arrange

        var editPoolCommand = new EditPoolCommand(
            Guid.NewGuid(),
            new PoolInputModelBuilder().Build(),
            Guid.NewGuid()
        );

        // Act

        var mappedPool = _poolMapper.MapToCoreEntity(editPoolCommand);

        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(mappedPool, Is.Not.Null);
            Assert.That(mappedPool.Id, Is.EqualTo(editPoolCommand.Id));
            Assert.That(mappedPool.Tls, Is.EqualTo(editPoolCommand.Model.Tls));
            Assert.That(mappedPool.IsDomain(), Is.EqualTo(false));
            Assert.That(mappedPool.OwnerId, Is.EqualTo(editPoolCommand.UserId));
            Assert.That(mappedPool.Domain, Is.EqualTo(editPoolCommand.Model.Domain));
            Assert.That(mappedPool.Port, Is.EqualTo(editPoolCommand.Model.Port));
            Assert.That(mappedPool.CryptocurrencyId, Is.EqualTo(editPoolCommand.Model.CryptocurrencyId));
            Assert.That(mappedPool.Cryptocurrency, Is.Null);
        });
    }

    [Test]
    public void MapToModel_ValidPool_ReturnPoolModel()
    {
        // Arrange

        var pool = new PoolBuilder()
            .WithCryptocurrency(crypto =>
                crypto.WithAlgorithm(algo =>
                    algo.WithOwner()))
            .Build();
        

        // Act

        var mappedModel = _poolMapper.MapToModel(pool);


        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(mappedModel, Is.Not.Null);
            Assert.That(mappedModel.Id, Is.EqualTo(pool.Id));
            Assert.That(mappedModel.OwnerId, Is.EqualTo(pool.OwnerId));
            Assert.That(mappedModel.Tls, Is.EqualTo(pool.Tls));
            Assert.That(mappedModel.Domain, Is.EqualTo(pool.Domain));
            Assert.That(mappedModel.Port, Is.EqualTo(pool.Port));
            Assert.That(mappedModel.CryptocurrencyId, Is.EqualTo(pool.CryptocurrencyId));
            Assert.That(mappedModel.Cryptocurrency, Is.EqualTo(pool.Cryptocurrency.FullName));
        });
    }
}
