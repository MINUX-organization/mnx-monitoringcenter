using MNX.MonitoringCenter.Management.Tests.Service.Assertions;
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

        mappedPool.ShouldBeEqualTo(addPoolCommand);
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

        mappedPool.ShouldBeEqualTo(editPoolCommand);
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

        mappedModel.ShouldBeEqualTo(pool);
    }
}
