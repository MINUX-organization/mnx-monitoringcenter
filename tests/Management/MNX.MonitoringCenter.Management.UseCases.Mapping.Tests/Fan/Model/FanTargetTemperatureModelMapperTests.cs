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

        Assert.That(mappedFanOverclocking, Is.Not.Null);
        Assert.That(mappedFanOverclocking, Is.TypeOf<FanOverclockingWithTargetTemperature>());
        Assert.Multiple(() =>
        {
            Assert.That(mappedFanOverclocking.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(mappedFanOverclocking.Type, Is.EqualTo(fanOverclockingModel.FanOverclockingType));

            var fanOverclocking = (FanOverclockingWithTargetTemperature)mappedFanOverclocking;
            Assert.That(fanOverclocking.MaxTargetSpeed, Is.EqualTo(fanOverclockingModel.MaxTargetSpeed));
            Assert.That(fanOverclocking.MinTargetSpeed, Is.EqualTo(fanOverclockingModel.MinTargetSpeed));
            Assert.That(fanOverclocking.TargetCoreTemperature, Is.EqualTo(fanOverclockingModel.TargetCoreTemperature));
            Assert.That(fanOverclocking.TargetMemoryTemperature, Is.EqualTo(fanOverclockingModel.TargetMemoryTemperature));
        });
    }

    [TestCaseSource(typeof(FanOverclockingTestCases), nameof(FanOverclockingTestCases.FanOverclockingModels))]
    public void MapToCoreEntity_ValidModelAndId_ReturnsCoreEntity(FanOverclockingWithTargetTemperatureModel fanOverclockingModel)
    {
        // Arrange

        var fanOverclockingId = Guid.NewGuid();


        // Act

        var mappedFanOverclocking = _fanOverclockingWithTargetTemperatureMapper.MapToCoreEntity(fanOverclockingModel, fanOverclockingId);


        // Assert

        Assert.That(mappedFanOverclocking, Is.Not.Null);
        Assert.That(mappedFanOverclocking, Is.TypeOf<FanOverclockingWithTargetTemperature>());
        Assert.Multiple(() =>
        {
            Assert.That(mappedFanOverclocking.Id, Is.EqualTo(fanOverclockingId));
            Assert.That(mappedFanOverclocking.Type, Is.EqualTo(fanOverclockingModel.FanOverclockingType));

            var fanOverclocking = (FanOverclockingWithTargetTemperature)mappedFanOverclocking;
            Assert.That(fanOverclocking.MaxTargetSpeed, Is.EqualTo(fanOverclockingModel.MaxTargetSpeed));
            Assert.That(fanOverclocking.MinTargetSpeed, Is.EqualTo(fanOverclockingModel.MinTargetSpeed));
            Assert.That(fanOverclocking.TargetCoreTemperature, Is.EqualTo(fanOverclockingModel.TargetCoreTemperature));
            Assert.That(fanOverclocking.TargetMemoryTemperature, Is.EqualTo(fanOverclockingModel.TargetMemoryTemperature));
        });
    }

    [TestCaseSource(typeof(FanOverclockingTestCases), nameof(FanOverclockingTestCases.FanOverclockings))]
    public void MapToModel_ValidCoreEntity_ReturnsModel(FanOverclockingWithTargetTemperature fanOverclockingCore)
    {
        // Arrange
        // Act

        var mappedFanOverclockingModel = _fanOverclockingWithTargetTemperatureMapper.MapToModel(fanOverclockingCore);


        // Assert

        Assert.That(mappedFanOverclockingModel, Is.Not.Null);
        Assert.That(mappedFanOverclockingModel, Is.TypeOf<FanOverclockingWithTargetTemperatureModel>());
        Assert.Multiple(() =>
        {
            Assert.That(mappedFanOverclockingModel.FanOverclockingType, Is.EqualTo(fanOverclockingCore.Type));

            var fanOverclockingModel = (FanOverclockingWithTargetTemperatureModel)mappedFanOverclockingModel;
            Assert.That(fanOverclockingModel.MaxTargetSpeed, Is.EqualTo(fanOverclockingCore.MaxTargetSpeed));
            Assert.That(fanOverclockingModel.MinTargetSpeed, Is.EqualTo(fanOverclockingCore.MinTargetSpeed));
            Assert.That(fanOverclockingModel.TargetCoreTemperature, Is.EqualTo(fanOverclockingCore.TargetCoreTemperature));
            Assert.That(fanOverclockingModel.TargetMemoryTemperature, Is.EqualTo(fanOverclockingCore.TargetMemoryTemperature));
        });
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
