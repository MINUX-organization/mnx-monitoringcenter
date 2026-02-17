using MNX.MonitoringCenter.Management.UseCases.Mapping.Cryptocurrency;
using MNX.MonitoringCenter.Management.UseCases.Mining.Cryptocurrency;
using MNX.MonitoringCenter.Management.UseCases.Mining.Cryptocurrency.Commands;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests;

using Algorithm = Core.Mining.Algorithm;
using Cryptocurrency = Core.Mining.Cryptocurrency;

[TestFixture]
public sealed class CryptocurrencyMapperTests
{
    private ICryptocurrencyMapper _cryptocurrencyMapper;

    [SetUp]
    public void SetUp()
    {
        _cryptocurrencyMapper = new CryptocurrencyMapper();
    }

    [Test]
    public void MapToCoreEntity_ValidCryptocurrencyInputModel_ReturnCryptocurrency()
    {
        // Arrange

        var model = new CryptocurrencyInputModel(
            "Crypto1",
            "Cryptocurrency1",
            Guid.Parse("00000000-0000-0000-0000-000000000001")
        );
        var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");


        // Act

        var mappedEntity = _cryptocurrencyMapper.MapToCoreEntity(model, userId);


        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(mappedEntity, Is.Not.Null);
            Assert.That(mappedEntity.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(mappedEntity.ShortName, Is.EqualTo(model.ShortName));
            Assert.That(mappedEntity.FullName, Is.EqualTo(model.FullName));
            Assert.That(mappedEntity.OwnerId, Is.EqualTo(userId));
            Assert.That(mappedEntity.AlgorithmId, Is.EqualTo(model.AlgorithmId));
            Assert.That(mappedEntity.Algorithm, Is.Null);
        });
    }

    [Test]
    public void MapToModel_ValidCryptocurrency_ReturnCryptocurrencyModel()
    {
        // Arrange

        var entity = new Cryptocurrency()
        {
            Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
            ShortName = "Crypto1",
            FullName = "Cryptocurrency1",
            OwnerId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            AlgorithmId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Algorithm = new Algorithm()
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Algo1",
                OwnerId = null
            }
        };


        // Act

        var mappedModel = _cryptocurrencyMapper.MapToModel(entity);


        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(mappedModel, Is.Not.Null);
            Assert.That(mappedModel.Id, Is.EqualTo(entity.Id));
            Assert.That(mappedModel.ShortName, Is.EqualTo(entity.ShortName));
            Assert.That(mappedModel.FullName, Is.EqualTo(entity.FullName));
            Assert.That(mappedModel.OwnerId, Is.EqualTo(entity.OwnerId));
            Assert.That(mappedModel.Algorithm, Is.Not.Null);
            Assert.That(mappedModel.Algorithm.Id, Is.EqualTo(entity.AlgorithmId));
            Assert.That(mappedModel.Algorithm.Name, Is.EqualTo(entity.Algorithm.Name));
            Assert.That(mappedModel.Algorithm.OwnerId, Is.EqualTo(entity.Algorithm.OwnerId));
        });
    }
}
