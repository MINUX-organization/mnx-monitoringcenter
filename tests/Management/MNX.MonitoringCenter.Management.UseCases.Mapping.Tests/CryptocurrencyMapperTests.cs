using MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Cryptocurrency;
using MNX.MonitoringCenter.Management.UseCases.Mining.Cryptocurrency;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests;

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

        var model = new CryptocurrencyInputModelBuilder().Build();
        var userId = Guid.NewGuid();


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
        
        var cryptocurrency = new CryptocurrencyBuilder()
            .WithOwner()
            .WithAlgorithm(algo => algo.WithName("Algo1"))
            .Build();


        // Act

        var mappedModel = _cryptocurrencyMapper.MapToModel(cryptocurrency);


        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(mappedModel, Is.Not.Null);
            Assert.That(mappedModel.Id, Is.EqualTo(cryptocurrency.Id));
            Assert.That(mappedModel.ShortName, Is.EqualTo(cryptocurrency.ShortName));
            Assert.That(mappedModel.FullName, Is.EqualTo(cryptocurrency.FullName));
            Assert.That(mappedModel.OwnerId, Is.EqualTo(cryptocurrency.OwnerId));
            Assert.That(mappedModel.Algorithm, Is.Not.Null);
            Assert.That(mappedModel.Algorithm.Id, Is.EqualTo(cryptocurrency.AlgorithmId));
            Assert.That(mappedModel.Algorithm.Name, Is.EqualTo(cryptocurrency.Algorithm.Name));
            Assert.That(mappedModel.Algorithm.OwnerId, Is.EqualTo(cryptocurrency.Algorithm.OwnerId));
        });
    }
}
