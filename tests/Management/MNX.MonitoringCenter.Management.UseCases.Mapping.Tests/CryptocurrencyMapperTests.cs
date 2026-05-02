using MNX.MonitoringCenter.Management.Tests.Service.Assertions;
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

        mappedEntity.ShouldBeEqualTo(model, userId);
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

        mappedModel.ShouldBeEqualTo(cryptocurrency);
    }
}
