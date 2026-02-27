using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu.Fan;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.Overclockings.Fan;
using MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.Overclockings.Fans;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Fan.Model;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Model;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Fan;

[TestFixture]
public class FanTargetTemperatureModelMapperTests
{
    private IFanOverclockingModelMapper<FanOverclockingWithTargetTemperatureModel, FanOverclockingWithTargetTemperature>
        _fanOverclockingWithTargetTemperatureMapper;

    [SetUp]
    public void SetUp()
    {
        _fanOverclockingWithTargetTemperatureMapper = new FanTargetTemperatureModelMapper();
    }

    [Test]
    public void MapToCoreEntity_ValidModel_ReturnsCoreEntity()
    {
        // Arrange

        var fanOverclockingModel = new FanOverclockingWithTargetTemperatureModelBuilder()
            .WithMaxTargetSpeed(100)
            .WithMinTargetSpeed(20)
            .WithTargetCoreTemperature(70)
            .WithTargetMemoryTemperature(60)
            .Build();


        // Act

        var mappedFanOverclocking = _fanOverclockingWithTargetTemperatureMapper.MapToCoreEntity(fanOverclockingModel);


        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(mappedFanOverclocking, Is.Not.Null);
            Assert.That(mappedFanOverclocking.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(mappedFanOverclocking.Type, Is.EqualTo(fanOverclockingModel.FanOverclockingType));
            Assert.That(mappedFanOverclocking, Is.TypeOf<FanOverclockingWithTargetTemperature>());

            var fanOverclocking = (FanOverclockingWithTargetTemperature)mappedFanOverclocking;
            Assert.That(fanOverclocking.MaxTargetSpeed, Is.EqualTo(fanOverclockingModel.MaxTargetSpeed));
            Assert.That(fanOverclocking.MinTargetSpeed, Is.EqualTo(fanOverclockingModel.MinTargetSpeed));
            Assert.That(fanOverclocking.TargetCoreTemperature, Is.EqualTo(fanOverclockingModel.TargetCoreTemperature));
            Assert.That(fanOverclocking.TargetMemoryTemperature, Is.EqualTo(fanOverclockingModel.TargetMemoryTemperature));
        });
    }

    [Test]
    public void MapToCoreEntity_ValidModelAndId_ReturnsCoreEntity()
    {
        // Arrange

        var fanOverclockingId = Guid.NewGuid();
        var fanOverclockingModel = new FanOverclockingWithTargetTemperatureModelBuilder()
            .WithMaxTargetSpeed(100)
            .WithMinTargetSpeed(20)
            .WithTargetCoreTemperature(70)
            .WithTargetMemoryTemperature(60)
            .Build();


        // Act

        var mappedFanOverclocking = _fanOverclockingWithTargetTemperatureMapper.MapToCoreEntity(fanOverclockingModel, fanOverclockingId);


        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(mappedFanOverclocking, Is.Not.Null);
            Assert.That(mappedFanOverclocking.Id, Is.EqualTo(fanOverclockingId));
            Assert.That(mappedFanOverclocking.Type, Is.EqualTo(fanOverclockingModel.FanOverclockingType));
            Assert.That(mappedFanOverclocking, Is.TypeOf<FanOverclockingWithTargetTemperature>());

            var fanOverclocking = (FanOverclockingWithTargetTemperature)mappedFanOverclocking;
            Assert.That(fanOverclocking.MaxTargetSpeed, Is.EqualTo(fanOverclockingModel.MaxTargetSpeed));
            Assert.That(fanOverclocking.MinTargetSpeed, Is.EqualTo(fanOverclockingModel.MinTargetSpeed));
            Assert.That(fanOverclocking.TargetCoreTemperature, Is.EqualTo(fanOverclockingModel.TargetCoreTemperature));
            Assert.That(fanOverclocking.TargetMemoryTemperature, Is.EqualTo(fanOverclockingModel.TargetMemoryTemperature));
        });
    }

    [Test]
    public void MapToModel_ValidCoreEntity_ReturnsModel()
    {
        // Arrange

        var fanOverclockingCore = new FanOverclockingWithTargetTemperatureBuilder()
            .WithMaxTargetSpeed(100)
            .WithMinTargetSpeed(20)
            .WithTargetCoreTemperature(70)
            .WithTargetMemoryTemperature(60)
            .Build();


        // Act

        var mappedFanOverclockingModel = _fanOverclockingWithTargetTemperatureMapper.MapToModel(fanOverclockingCore);


        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(mappedFanOverclockingModel, Is.Not.Null);
            Assert.That(mappedFanOverclockingModel.FanOverclockingType, Is.EqualTo(fanOverclockingCore.Type));
            Assert.That(mappedFanOverclockingModel, Is.TypeOf<FanOverclockingWithTargetTemperatureModel>());

            var fanOverclockingModel = (FanOverclockingWithTargetTemperatureModel)mappedFanOverclockingModel;
            Assert.That(fanOverclockingModel.MaxTargetSpeed, Is.EqualTo(fanOverclockingCore.MaxTargetSpeed));
            Assert.That(fanOverclockingModel.MinTargetSpeed, Is.EqualTo(fanOverclockingCore.MinTargetSpeed));
            Assert.That(fanOverclockingModel.TargetCoreTemperature, Is.EqualTo(fanOverclockingCore.TargetCoreTemperature));
            Assert.That(fanOverclockingModel.TargetMemoryTemperature, Is.EqualTo(fanOverclockingCore.TargetMemoryTemperature));
        });
    }
}
