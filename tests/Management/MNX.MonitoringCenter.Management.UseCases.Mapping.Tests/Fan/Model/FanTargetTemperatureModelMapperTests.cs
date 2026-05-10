using FluentAssertions;
using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu.Fan;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.Overclockings.Fan;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.Overclockings.Fans;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Fan.Model;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Model;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Fan.Model;

[TestFixture]
public sealed class FanTargetTemperatureModelMapperTests
{
    private IFanOverclockingModelMapper<FanOverclockingWithTargetTemperatureModel, FanOverclockingWithTargetTemperature>
        _fanOverclockingWithTargetTemperatureMapper;

    [SetUp]
    public void SetUp()
    {
        _fanOverclockingWithTargetTemperatureMapper = new FanTargetTemperatureModelMapper();
    }

    [TestCaseSource(typeof(FanOverclockingTestCases), nameof(FanOverclockingTestCases.FanOverclockingModels))]
    public void MapToCoreEntity_ValidModel_ReturnsCoreEntity(FanOverclockingWithTargetTemperatureModel fanOverclockingModel)
    {
        // Arrange
        // Act

        var mappedFanOverclocking = _fanOverclockingWithTargetTemperatureMapper.MapToCoreEntity(fanOverclockingModel);


        // Assert
        var fan = mappedFanOverclocking
            .Should().BeOfType<FanOverclockingWithTargetTemperature>()
            .Which;

        fan.Id.Should().NotBe(Guid.Empty);
        fan.Type.Should().Be(fanOverclockingModel.FanOverclockingType);

        fan.MaxTargetSpeed.Should().Be(fanOverclockingModel.MaxTargetSpeed);
        fan.MinTargetSpeed.Should().Be(fanOverclockingModel.MinTargetSpeed);
        fan.TargetCoreTemperature.Should().Be(fanOverclockingModel.TargetCoreTemperature);
        fan.TargetMemoryTemperature.Should().Be(fanOverclockingModel.TargetMemoryTemperature);
    }

    [TestCaseSource(typeof(FanOverclockingTestCases), nameof(FanOverclockingTestCases.FanOverclockingModels))]
    public void MapToCoreEntity_ValidModelAndId_ReturnsCoreEntity(FanOverclockingWithTargetTemperatureModel fanOverclockingModel)
    {
        // Arrange

        var fanOverclockingId = Guid.NewGuid();


        // Act

        var mappedFanOverclocking = _fanOverclockingWithTargetTemperatureMapper.MapToCoreEntity(fanOverclockingModel, fanOverclockingId);


        // Assert

        var fan = mappedFanOverclocking
            .Should().BeOfType<FanOverclockingWithTargetTemperature>()
            .Which;

        fan.Id.Should().Be(fanOverclockingId);
        fan.Type.Should().Be(fanOverclockingModel.FanOverclockingType);

        fan.MaxTargetSpeed.Should().Be(fanOverclockingModel.MaxTargetSpeed);
        fan.MinTargetSpeed.Should().Be(fanOverclockingModel.MinTargetSpeed);
        fan.TargetCoreTemperature.Should().Be(fanOverclockingModel.TargetCoreTemperature);
        fan.TargetMemoryTemperature.Should().Be(fanOverclockingModel.TargetMemoryTemperature);
    }

    [TestCaseSource(typeof(FanOverclockingTestCases), nameof(FanOverclockingTestCases.FanOverclockings))]
    public void MapToModel_ValidCoreEntity_ReturnsModel(FanOverclockingWithTargetTemperature fanOverclockingCore)
    {
        // Arrange
        // Act

        var mappedFanOverclockingModel = _fanOverclockingWithTargetTemperatureMapper.MapToModel(fanOverclockingCore);


        // Assert

        var fan = mappedFanOverclockingModel
            .Should().BeOfType<FanOverclockingWithTargetTemperatureModel>()
            .Which;

        fan.FanOverclockingType.Should().Be(fanOverclockingCore.Type);
        fan.MaxTargetSpeed.Should().Be(fanOverclockingCore.MaxTargetSpeed);
        fan.MinTargetSpeed.Should().Be(fanOverclockingCore.MinTargetSpeed);
        fan.TargetCoreTemperature.Should().Be(fanOverclockingCore.TargetCoreTemperature);
        fan.TargetMemoryTemperature.Should().Be(fanOverclockingCore.TargetMemoryTemperature);
    }
    
    private static class FanOverclockingTestCases
    {
        public static IEnumerable<FanOverclockingWithTargetTemperatureModel> FanOverclockingModels
        {
            get
            {
                yield return new FanOverclockingWithTargetTemperatureModelBuilder()
                    .WithMaxTargetSpeed(100)
                    .WithMinTargetSpeed(20)
                    .WithTargetCoreTemperature(70)
                    .WithTargetMemoryTemperature(60)
                    .Build();
            }
        }

        public static IEnumerable<FanOverclockingWithTargetTemperature> FanOverclockings
        {
            get
            {
                yield return new FanOverclockingWithTargetTemperatureBuilder()
                    .WithMaxTargetSpeed(100)
                    .WithMinTargetSpeed(20)
                    .WithTargetCoreTemperature(70)
                    .WithTargetMemoryTemperature(60)
                    .Build();
            }
        }
    }
}
