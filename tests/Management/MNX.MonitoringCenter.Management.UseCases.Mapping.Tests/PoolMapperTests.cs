using MNX.MonitoringCenter.Management.UseCases.Mapping.Pool;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool.Commands;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool.Commands.AddPool;
using MNX.MonitoringCenter.Management.UseCases.Mining.Pool.Commands.EditPool;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests;

using Pool = Core.Mining.Pool;

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
            new PoolInputModel(
                true,
                "www.domain.com",
                8080,
                Guid.Parse("11111111-1111-1111-1111-111111111111")
            ),
            Guid.Parse("00000000-0000-0000-0000-000000000001")
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
            Guid.Parse("00000000-0000-0000-0000-000000000001"),
            new PoolInputModel(
                false,
                "www.domain.com",
                8080,
                Guid.Parse("22222222-2222-2222-2222-222222222222")
            ),
            Guid.Parse("11111111-1111-1111-1111-111111111111")
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

        var pool = new Pool()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Domain = "www.domain.com",
            Port = 8080,
            CryptocurrencyId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            OwnerId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Tls = true,
            Cryptocurrency = new()
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                FullName = "Crypto",
                ShortName = "Cr",
                OwnerId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                AlgorithmId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Algorithm = new()
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Name = "Algo",
                    OwnerId = Guid.Parse("22222222-2222-2222-2222-222222222222")
                }
            }
        };

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
