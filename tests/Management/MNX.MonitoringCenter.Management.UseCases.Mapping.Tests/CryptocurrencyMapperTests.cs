using FluentAssertions;
using MNX.MonitoringCenter.Management.Contracts;
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

        mappedEntity.Should().NotBeNull();
        mappedEntity.Should().Satisfy<Core.Mining.Cryptocurrency>(x =>
        {
            x.Id.Should().NotBe(Guid.Empty);
            x.ShortName.Should().Be(model.ShortName);
            x.FullName.Should().Be(model.FullName);
            x.OwnerId.Should().Be(userId);
            x.AlgorithmId.Should().Be(model.AlgorithmId);
            x.Algorithm.Should().BeNull();
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

        mappedModel.Should().NotBeNull();
        mappedModel.Should().Satisfy<CryptocurrencyModel>(x =>
        {
            x.Id.Should().Be(cryptocurrency.Id);
            x.ShortName.Should().Be(cryptocurrency.ShortName);
            x.FullName.Should().Be(cryptocurrency.FullName);
            x.OwnerId.Should().Be(cryptocurrency.OwnerId);
            x.Algorithm.Should().NotBeNull();
            x.Algorithm.Should().Satisfy<Core.Mining.Algorithm>(y =>
            {
                y.Id.Should().Be(cryptocurrency.AlgorithmId);
                y.Name.Should().Be(cryptocurrency.Algorithm!.Name);
                y.OwnerId.Should().Be(cryptocurrency.Algorithm!.OwnerId);
            });
        });
    }
}
